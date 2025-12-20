using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 보스 미사일 투사체
    /// 플레이어를 추적하는 유도 미사일
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class BossMissile : MonoBehaviour
    {
        [Header("Damage Settings")]
        [SerializeField] private float damage = 30f;
        [SerializeField] private DamageType damageType = DamageType.Explosive;
        
        [Header("Movement Settings")]
        [SerializeField] private float speed = 8f;
        [SerializeField] private float rotationSpeed = 3f; // 회전 속도 (추적용)
        [SerializeField] private float lifetime = 10f;
        [SerializeField] private bool isHoming = true; // 유도 미사일 여부
        
        [Header("Hit Detection")]
        [SerializeField] private LayerMask targetLayer;
        
        [Header("Effects")]
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private GameObject trailEffectPrefab; // 미사일 궤적 이펙트
        
        [Header("Explosion")]
        [SerializeField] private float explosionRadius = 2f; // 폭발 범위
        [SerializeField] private bool dealsSplashDamage = true; // 범위 피해 여부
        
        private Rigidbody rb;
        private GameObject owner;
        private Transform target;
        private bool hasHit;
        private GameObject trailEffect;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            
            // Rigidbody 설정
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = false;
            }
            
            // Collider가 Trigger인지 확인
            Collider col = GetComponent<Collider>();
            if (col != null && !col.isTrigger)
            {
                col.isTrigger = true;
            }
        }
        
        /// <summary>
        /// 미사일 소유자 설정
        /// </summary>
        public void SetOwner(GameObject newOwner)
        {
            owner = newOwner;
        }
        
        /// <summary>
        /// 추적 대상 설정
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
        
        /// <summary>
        /// 미사일 발사
        /// </summary>
        public void Launch()
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                
                if (rb != null)
                {
                    rb.linearVelocity = direction * speed;
                }
                
                // 미사일 회전 (진행 방향을 향하도록)
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
            else
            {
                // 타겟이 없으면 전방으로 발사
                if (rb != null)
                {
                    rb.linearVelocity = transform.forward * speed;
                }
            }
            
            // 궤적 이펙트 생성
            if (trailEffectPrefab != null)
            {
                trailEffect = Instantiate(trailEffectPrefab, transform.position, Quaternion.identity);
                trailEffect.transform.SetParent(transform);
            }
            
            // 수명 후 자동 파괴
            Destroy(gameObject, lifetime);
        }
        
        private void FixedUpdate()
        {
            if (hasHit || !isHoming || target == null)
                return;
            
            // 유도 미사일: 타겟을 향해 회전
            Vector3 direction = (target.position - transform.position).normalized;
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
            
            // 회전된 방향으로 속도 업데이트
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // 이미 충돌했으면 무시
            if (hasHit)
                return;
            
            // 레이어 체크
            if (((1 << other.gameObject.layer) & targetLayer) == 0)
                return;
            
            // 소유자와의 충돌 무시
            if (owner != null && (other.gameObject == owner || other.transform.IsChildOf(owner.transform)))
                return;
            
            hasHit = true;
            
            // 범위 피해
            if (dealsSplashDamage)
            {
                DealSplashDamage(transform.position);
            }
            else
            {
                // 단일 대상 피해
                DealSingleTargetDamage(other);
            }
            
            // 충돌 처리
            OnHit(transform.position);
        }
        
        /// <summary>
        /// 단일 대상 피해
        /// </summary>
        private void DealSingleTargetDamage(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable == null)
                damageable = other.GetComponentInParent<IDamageable>();
            
            if (damageable != null && !damageable.IsDead)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitDirection = rb != null ? rb.linearVelocity.normalized : transform.forward;
                
                DamageInfo damageInfo = new DamageInfo(
                    damage,
                    damageType,
                    hitPoint,
                    hitDirection,
                    owner != null ? owner : gameObject
                );
                
                damageable.TakeDamage(damageInfo);
                Debug.Log($"[Boss Missile] {damage} 데미지를 {other.gameObject.name}에게!");
            }
        }
        
        /// <summary>
        /// 범위 피해
        /// </summary>
        private void DealSplashDamage(Vector3 explosionCenter)
        {
            Collider[] hitColliders = new Collider[10]; // 최대 10개의 콜라이더 감지
            int hitCount = Physics.OverlapSphereNonAlloc(explosionCenter, explosionRadius, hitColliders, targetLayer);
            
            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = hitColliders[i];
                
                // 소유자와의 충돌 무시
                if (owner != null && (hitCollider.gameObject == owner || hitCollider.transform.IsChildOf(owner.transform)))
                    continue;
                
                IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable == null)
                    damageable = hitCollider.GetComponentInParent<IDamageable>();
                
                if (damageable != null && !damageable.IsDead)
                {
                    Vector3 hitDirection = (hitCollider.transform.position - explosionCenter).normalized;
                    
                    // 거리에 따른 피해 감소 (선택 사항)
                    float distance = Vector3.Distance(explosionCenter, hitCollider.transform.position);
                    float damageMultiplier = 1f - (distance / explosionRadius);
                    float finalDamage = damage * damageMultiplier;
                    
                    DamageInfo damageInfo = new DamageInfo(
                        finalDamage,
                        damageType,
                        hitCollider.transform.position,
                        hitDirection,
                        owner != null ? owner : gameObject
                    );
                    
                    damageable.TakeDamage(damageInfo);
                    Debug.Log($"[Boss Missile] {finalDamage} 범위 데미지를 {hitCollider.gameObject.name}에게!");
                }
            }
        }
        
        /// <summary>
        /// 충돌 시 처리
        /// </summary>
        private void OnHit(Vector3 hitPoint)
        {
            // 히트 이펙트 생성
            if (hitEffectPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
                Destroy(hitEffect, 3f);
            }
            
            // 궤적 이펙트 분리
            if (trailEffect != null)
            {
                trailEffect.transform.SetParent(null);
                Destroy(trailEffect, 2f);
            }
            
            // 미사일 파괴
            Destroy(gameObject);
        }
        
        /// <summary>
        /// 디버그용 Gizmos
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (dealsSplashDamage)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
                Gizmos.DrawWireSphere(transform.position, explosionRadius);
            }
        }
    }
}

