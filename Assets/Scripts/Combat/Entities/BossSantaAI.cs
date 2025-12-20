using System.Collections;
using UnityEngine;
using Game.Combat;

namespace Combat.Entities
{
    /// <summary>
    /// 풀아머 산타 보스 AI 시스템
    /// 머신건 난사 + 미사일 포격 패턴을 가진 보스
    /// </summary>
    public class BossSantaAI : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform player; // 플레이어 대상
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 1f; // 매우 느린 이동 속도
        [SerializeField] private float rotationSpeed = 3f; // 회전 속도
        [SerializeField] private float attackStopDistance = 8f; // 공격 시작 거리
        
        [Header("Machine Gun Settings")]
        [SerializeField] private GameObject[] bulletPrefabs; // 총알 프리팹 배열 (5개 권장, 번갈아 발사)
        [SerializeField] private Transform leftArmFirePoint; // 왼쪽 팔 발사 위치
        [SerializeField] private Transform rightArmFirePoint; // 오른쪽 팔 발사 위치
        [SerializeField] private float bulletsPerSecond = 10f; // 초당 10발 (0.1초마다 1발)
        [SerializeField] private int shotsPerSalvo = 2; // 2회 난사
        [SerializeField] private int bulletsPerSalvo = 10; // 각 난사당 10발 (총 20발)
        [SerializeField] private float reloadTime = 0.5f; // 재장전 그로기 0.5초
        
        [Header("Missile Settings")]
        [SerializeField] private GameObject missilePrefab; // 미사일 프리팹
        [SerializeField] private Transform missileFirePoint; // 미사일 발사 위치
        [SerializeField] private float missileFireInterval = 1f; // 미사일 발사 간격 (1초마다 1발)
        [SerializeField] private float healthThresholdForMissiles = 0.5f; // 체력 50% 이하에서 미사일 발사 시작
        
        [Header("Vulnerability")]
        [SerializeField] private bool isVulnerableWhileFiring = true; // 발사 중 무방비 상태
        
        private EnemyHealth enemyHealth;
        private BossDeathSequence deathSequence;
        private bool isDead;
        private bool isAttacking;
        private int currentSalvoCount;
        private int currentBulletIndex; // 현재 사용할 총알 프리팹 인덱스
        private float nextMissileTime; // 다음 미사일 발사 시간
        private bool canUseMissiles; // 미사일 사용 가능 여부 (체력 50% 이하)
        
        private void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
            deathSequence = GetComponent<BossDeathSequence>();
            
            // 플레이어 자동 찾기
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
            }
            
            // 사망 이벤트 구독
            if (enemyHealth != null)
            {
                StartCoroutine(SubscribeToDeathEvent());
            }
        }
        
        /// <summary>
        /// 사망 이벤트 구독
        /// </summary>
        private IEnumerator SubscribeToDeathEvent()
        {
            yield return null; // 한 프레임 대기
            
            var healthSystemField = typeof(EnemyHealth).GetField("healthSystem", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (healthSystemField != null)
            {
                var healthSystem = healthSystemField.GetValue(enemyHealth) as HealthSystem;
                if (healthSystem != null && healthSystem.OnDeath != null)
                {
                    healthSystem.OnDeath.AddListener(OnBossDeath);
                }
            }
        }
        
        private void Update()
        {
            if (isDead || player == null)
                return;
            
            // 체력 체크 - 50% 이하면 미사일 사용 가능
            if (enemyHealth != null && !canUseMissiles)
            {
                float healthPercent = enemyHealth.CurrentHP / enemyHealth.MaxHP;
                if (healthPercent <= healthThresholdForMissiles)
                {
                    canUseMissiles = true;
                    Debug.Log("[Boss Santa] HP 50% 이하! 미사일 공격 활성화!");
                }
            }
            
            // 플레이어를 바라보기
            LookAtPlayer();
            
            // 항상 플레이어에게 천천히 이동 (공격 중에도!)
            MoveTowardsPlayer();
            
            // 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            // 공격 중이 아니고 공격 거리 안이면 공격 시작
            if (!isAttacking && distanceToPlayer <= attackStopDistance)
            {
                StartCoroutine(AttackPattern());
            }
            
            // 1초마다 미사일 발사 (체력 50% 이하일 때만)
            if (canUseMissiles && Time.time >= nextMissileTime)
            {
                FireMissile();
                nextMissileTime = Time.time + missileFireInterval;
            }
        }
        
        /// <summary>
        /// 플레이어를 향해 회전
        /// </summary>
        private void LookAtPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Y축 회전 무시 (수평 회전만)
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        /// <summary>
        /// 플레이어를 향해 이동
        /// </summary>
        private void MoveTowardsPlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // 수평 이동만
            
            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
        
        /// <summary>
        /// 공격 패턴 메인 코루틴
        /// </summary>
        private IEnumerator AttackPattern()
        {
            isAttacking = true;
            Debug.Log("[Boss Santa] ========== 공격 패턴 시작! ==========");
            
            // 머신건 공격 (2회 난사)
            currentSalvoCount = 0;
            Debug.Log($"[Boss Santa] 머신건 공격 시작 ({shotsPerSalvo}회 난사 예정)");
            while (currentSalvoCount < shotsPerSalvo)
            {
                Debug.Log($"[Boss Santa] 난사 {currentSalvoCount + 1}/{shotsPerSalvo}");
                yield return StartCoroutine(MachineGunSalvo());
                currentSalvoCount++;
            }
            Debug.Log("[Boss Santa] 머신건 공격 완료!");
            
            // 재장전 그로기
            Debug.Log($"[Boss Santa] 재장전 중... ({reloadTime}초 그로기)");
            yield return new WaitForSeconds(reloadTime);
            Debug.Log("[Boss Santa] 재장전 완료!");
            
            Debug.Log("[Boss Santa] ========== 공격 패턴 종료! 다시 시작 가능 ==========");
            isAttacking = false;
        }
        
        /// <summary>
        /// 머신건 난사 (양 팔에서 교차 발사)
        /// </summary>
        private IEnumerator MachineGunSalvo()
        {
            // 무방비 상태 활성화
            if (isVulnerableWhileFiring && enemyHealth != null)
            {
                var healthSystemField = typeof(EnemyHealth).GetField("healthSystem", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (healthSystemField != null)
                {
                    var healthSystem = healthSystemField.GetValue(enemyHealth) as HealthSystem;
                    if (healthSystem != null)
                    {
                        healthSystem.IsInvincible = false;
                    }
                }
            }
            
            float fireInterval = 1f / bulletsPerSecond;
            Debug.Log($"[Boss Santa] 난사 시작! {bulletsPerSalvo}발, 발사 간격: {fireInterval:F2}초");
            
            for (int i = 0; i < bulletsPerSalvo; i++)
            {
                // 교차 발사: 짝수는 왼팔, 홀수는 오른팔
                Transform firePoint = (i % 2 == 0) ? leftArmFirePoint : rightArmFirePoint;
                string armName = (i % 2 == 0) ? "왼팔" : "오른팔";
                
                if (firePoint != null && bulletPrefabs != null && bulletPrefabs.Length > 0 && player != null)
                {
                    FireBullet(firePoint);
                    Debug.Log($"[Boss Santa] {armName}에서 총알 {i + 1}/{bulletsPerSalvo} 발사!");
                }
                
                yield return new WaitForSeconds(fireInterval);
            }
            
            Debug.Log("[Boss Santa] 난사 1회 완료!");
        }
        
        /// <summary>
        /// 총알 발사
        /// </summary>
        private void FireBullet(Transform firePoint)
        {
            // 총알 프리팹 배열 체크
            if (bulletPrefabs == null || bulletPrefabs.Length == 0)
            {
                Debug.LogWarning("[Boss Santa] 총알 프리팹이 설정되지 않았습니다!");
                return;
            }
            
            if (firePoint == null || player == null)
                return;
            
            // 현재 인덱스의 총알 프리팹 선택
            GameObject currentBulletPrefab = bulletPrefabs[currentBulletIndex];
            
            // null 체크
            if (currentBulletPrefab == null)
            {
                Debug.LogWarning($"[Boss Santa] 총알 프리팹 [{currentBulletIndex}]가 null입니다!");
                // 다음 인덱스로 이동
                currentBulletIndex = (currentBulletIndex + 1) % bulletPrefabs.Length;
                return;
            }
            
            // 총알 생성
            GameObject bullet = Instantiate(currentBulletPrefab, firePoint.position, Quaternion.identity);
            
            // Projectile 컴포넌트 설정
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetOwner(gameObject);
                
                // 플레이어 방향으로 발사
                Vector3 direction = (player.position - firePoint.position).normalized;
                projectile.Launch(direction);
            }
            else
            {
                Debug.LogWarning($"[Boss Santa] 총알 프리팹 [{currentBulletIndex}]에 Projectile 컴포넌트가 없습니다!");
                Destroy(bullet);
            }
            
            // 다음 총알 프리팹으로 순환 (0, 1, 2, 3, 4, 0, 1, ...)
            currentBulletIndex = (currentBulletIndex + 1) % bulletPrefabs.Length;
        }
        
        /// <summary>
        /// 미사일 발사 (1초마다 플레이어를 향해 발사)
        /// </summary>
        private void FireMissile()
        {
            if (missilePrefab == null || missileFirePoint == null || player == null || isDead)
                return;
            
            // 미사일 생성
            GameObject missile = Instantiate(missilePrefab, missileFirePoint.position, Quaternion.identity);
            
            // Projectile 컴포넌트로 플레이어 방향 발사
            Projectile projectile = missile.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetOwner(gameObject);
                
                // 플레이어 방향 계산 및 발사
                Vector3 direction = (player.position - missileFirePoint.position).normalized;
                projectile.Launch(direction);
                
                Debug.Log($"[Boss Santa] 미사일 발사! (다음 발사: {missileFireInterval}초 후)");
            }
            else
            {
                Debug.LogWarning("[Boss Santa] 미사일 프리팹에 Projectile 컴포넌트가 없습니다!");
                Destroy(missile);
            }
        }
        
        /// <summary>
        /// 보스 사망 처리
        /// </summary>
        private void OnBossDeath()
        {
            isDead = true;
            Debug.Log("[Boss Santa] 보스 사망!");
            
            // 사망 시퀀스 실행
            if (deathSequence != null)
            {
                deathSequence.PlayDeathSequence();
            }
        }
        
        /// <summary>
        /// 디버그용 Gizmos
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // 공격 시작 거리 (노란색)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackStopDistance);
            
            // 미사일 발사 위치 (빨간색)
            if (missileFirePoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(missileFirePoint.position, 0.3f);
            }
        }
        
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (enemyHealth != null)
            {
                var healthSystemField = typeof(EnemyHealth).GetField("healthSystem", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (healthSystemField != null)
                {
                    var healthSystem = healthSystemField.GetValue(enemyHealth) as HealthSystem;
                    if (healthSystem != null && healthSystem.OnDeath != null)
                    {
                        healthSystem.OnDeath.RemoveListener(OnBossDeath);
                    }
                }
            }
        }
    }
}

