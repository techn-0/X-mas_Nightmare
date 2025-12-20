using UnityEngine;
using Game.Combat;

namespace Combat.Entities
{
    /// <summary>
    /// 눈사람 몹 AI 시스템
    /// 플레이어를 추적하고 거리에 따라 원거리/근접 공격을 수행합니다
    /// </summary>
    public class SnowmanAI : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform player; // 플레이어 대상
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 2f; // 이동 속도
        [SerializeField] private float rotationSpeed = 5f; // 회전 속도
        
        [Header("Attack Settings")]
        [SerializeField] private float rangedAttackDistance = 10f; // 원거리 공격 거리
        [SerializeField] private float meleeAttackDistance = 2f; // 근접 공격 거리
        [SerializeField] private float rangedAttackCooldown = 2f; // 원거리 공격 쿨다운
        [SerializeField] private float meleeAttackCooldown = 1.5f; // 근접 공격 쿨다운
        
        [Header("Projectile Settings")]
        [SerializeField] private GameObject snowballPrefab; // 눈덩이 프리팹
        [SerializeField] private Transform throwPoint; // 눈덩이 발사 위치
        
        [Header("Melee Attack Settings")]
        [SerializeField] private float meleeDamage = 20f; // 근접 공격 데미지
        [SerializeField] private float meleeRange = 2.5f; // 근접 공격 범위
        
        private float lastRangedAttackTime = -999f; // 마지막 원거리 공격 시간
        private float lastMeleeAttackTime = -999f; // 마지막 근접 공격 시간
        private EnemyHealth enemyHealth;
        
        private void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
            
            // 플레이어를 자동으로 찾기
            if (player == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    player = playerObj.transform;
                }
            }
            
            // 발사 위치가 지정되지 않았으면 자신의 위치 사용
            if (throwPoint == null)
            {
                throwPoint = transform;
            }
        }
        
        private void Update()
        {
            // 사망했거나 플레이어가 없으면 동작 중지
            if (enemyHealth != null && enemyHealth.IsDead)
                return;
            
            if (player == null)
                return;
            
            // 플레이어와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            // 플레이어를 바라보기
            LookAtPlayer();
            
            // 거리에 따라 행동 결정
            if (distanceToPlayer <= meleeAttackDistance)
            {
                // 근접 공격 범위 - 공격만 수행
                PerformMeleeAttack();
            }
            else if (distanceToPlayer <= rangedAttackDistance)
            {
                // 원거리 공격 범위 - 다가가며 눈덩이 던지기
                MoveTowardsPlayer();
                PerformRangedAttack();
            }
            else
            {
                // 플레이어에게 다가가기
                MoveTowardsPlayer();
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
        /// 원거리 공격 (눈덩이 던지기)
        /// </summary>
        private void PerformRangedAttack()
        {
            if (Time.time - lastRangedAttackTime < rangedAttackCooldown)
                return;
            
            if (snowballPrefab == null)
            {
                Debug.LogWarning("눈덩이 프리팹이 설정되지 않았습니다!");
                return;
            }
            
            lastRangedAttackTime = Time.time;
            
            // 눈덩이 생성
            Vector3 spawnPosition = throwPoint.position;
            GameObject snowball = Instantiate(snowballPrefab, spawnPosition, Quaternion.identity);
            
            // Projectile 컴포넌트 설정
            Projectile projectile = snowball.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetOwner(gameObject);
                
                // 플레이어 방향으로 발사
                Vector3 direction = (player.position - spawnPosition).normalized;
                projectile.Launch(direction);
            }
            else
            {
                Debug.LogWarning("눈덩이 프리팹에 Projectile 컴포넌트가 없습니다!");
                Destroy(snowball);
            }
        }
        
        /// <summary>
        /// 근접 공격
        /// </summary>
        private void PerformMeleeAttack()
        {
            if (Time.time - lastMeleeAttackTime < meleeAttackCooldown)
                return;
            
            lastMeleeAttackTime = Time.time;
            
            // 플레이어에게 데미지 전달
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable != null && !damageable.IsDead)
            {
                Vector3 hitDirection = (player.position - transform.position).normalized;
                
                DamageInfo damageInfo = new DamageInfo(
                    meleeDamage,
                    DamageType.Physical,
                    player.position,
                    hitDirection,
                    gameObject
                );
                
                damageable.TakeDamage(damageInfo);
                Debug.Log("눈사람이 근접 공격을 수행했습니다!");
            }
        }
        
        /// <summary>
        /// 디버그용 Gizmos
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // 근접 공격 범위 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeAttackDistance);
            
            // 원거리 공격 범위 (노란색)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rangedAttackDistance);
            
            // 근접 공격 감지 범위 (주황색)
            Gizmos.color = new Color(1f, 0.5f, 0f);
            Gizmos.DrawWireSphere(transform.position, meleeRange);
        }
    }
}

