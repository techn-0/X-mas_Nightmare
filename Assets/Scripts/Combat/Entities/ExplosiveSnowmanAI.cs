using System.Collections;
using UnityEngine;
using Game.Combat;

namespace Combat.Entities
{
    /// <summary>
    /// 연료통 눈사람 AI 시스템
    /// 플레이어에게 접근하여 자폭 공격을 수행합니다
    /// </summary>
    public class ExplosiveSnowmanAI : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform player; // 플레이어 대상
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 3f; // 이동 속도 (일반 눈사람보다 빠름)
        [SerializeField] private float rotationSpeed = 5f; // 회전 속도
        
        [Header("Explosion Settings")]
        [SerializeField] private float explosionDistance = 2f; // 폭발 시작 거리
        [SerializeField] private float explosionDamage = 20f; // 폭발 데미지
        [SerializeField] private float explosionRadius = 3f; // 폭발 범위
        [SerializeField] private float explosionDelay = 0.3f; // 폭발 대기 시간
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject explosionEffect; // 폭발 이펙트 프리팹
        
        private EnemyHealth enemyHealth;
        private bool isExploding; // 폭발 중인지 확인
        
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
            
            // 사망 이벤트 구독 (처치 시 폭발)
            if (enemyHealth != null)
            {
                // EnemyHealth의 OnDeath 이벤트에 구독
                StartCoroutine(SubscribeToDeathEvent());
            }
        }
        
        /// <summary>
        /// 사망 이벤트 구독 (HealthSystem 초기화 대기)
        /// </summary>
        private IEnumerator SubscribeToDeathEvent()
        {
            yield return null; // 한 프레임 대기
            
            // HealthSystem에서 OnDeath 이벤트 가져오기
            var healthSystemField = typeof(EnemyHealth).GetField("healthSystem", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (healthSystemField != null)
            {
                var healthSystem = healthSystemField.GetValue(enemyHealth) as HealthSystem;
                if (healthSystem != null && healthSystem.OnDeath != null)
                {
                    healthSystem.OnDeath.AddListener(OnKilled);
                }
            }
        }
        
        private void Update()
        {
            // 사망했거나 폭발 중이거나 플레이어가 없으면 동작 중지
            if (enemyHealth != null && enemyHealth.IsDead)
                return;
            
            if (isExploding || player == null)
                return;
            
            // 플레이어와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            // 플레이어를 바라보기
            LookAtPlayer();
            
            // 거리에 따라 행동 결정
            if (distanceToPlayer <= explosionDistance)
            {
                // 폭발 거리 도달 - 자폭 시작
                StartCoroutine(ExplodeCoroutine());
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
        /// 자폭 코루틴 (플레이어 접근 시)
        /// </summary>
        private IEnumerator ExplodeCoroutine()
        {
            if (isExploding)
                yield break;
            
            isExploding = true;
            
            // 0.3초 대기
            yield return new WaitForSeconds(explosionDelay);
            
            // 폭발 실행
            Explode();
        }
        
        /// <summary>
        /// 처치 시 호출되는 폭발
        /// </summary>
        private void OnKilled()
        {
            if (isExploding)
                return;
            
            StartCoroutine(ExplodeOnDeathCoroutine());
        }
        
        /// <summary>
        /// 처치 시 폭발 코루틴
        /// </summary>
        private IEnumerator ExplodeOnDeathCoroutine()
        {
            isExploding = true;
            
            // 0.3초 대기
            yield return new WaitForSeconds(explosionDelay);
            
            // 폭발 실행
            Explode();
        }
        
        /// <summary>
        /// 폭발 실행
        /// </summary>
        private void Explode()
        {
            Debug.Log($"{gameObject.name} 폭발!");
            
            // 폭발 이펙트 생성
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            
            // 폭발 범위 내의 모든 객체 찾기 (최적화 버전)
            Collider[] hitColliders = new Collider[10]; // 최대 10개의 콜라이더 감지
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hitColliders);
            
            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = hitColliders[i];
                
                // 플레이어인지 확인
                if (hitCollider.CompareTag("Player"))
                {
                    IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                    if (damageable != null && !damageable.IsDead)
                    {
                        Vector3 hitDirection = (hitCollider.transform.position - transform.position).normalized;
                        
                        DamageInfo damageInfo = new DamageInfo(
                            explosionDamage,
                            DamageType.Explosive,
                            hitCollider.transform.position,
                            hitDirection,
                            gameObject
                        );
                        
                        damageable.TakeDamage(damageInfo);
                        Debug.Log($"연료통 눈사람 폭발로 플레이어에게 {explosionDamage} 데미지!");
                    }
                }
            }
            
            // 오브젝트 파괴
            Destroy(gameObject);
        }
        
        /// <summary>
        /// 디버그용 Gizmos
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // 폭발 시작 거리 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionDistance);
            
            // 폭발 범위 (주황색)
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
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
                        healthSystem.OnDeath.RemoveListener(OnKilled);
                    }
                }
            }
        }
    }
}

