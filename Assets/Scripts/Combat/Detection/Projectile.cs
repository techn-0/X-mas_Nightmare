using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 투사체 클래스
    /// 원거리 공격의 투사체를 처리합니다
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [Header("Damage Settings")]
        [SerializeField] private float damage = 15f;
        [SerializeField] private DamageType damageType = DamageType.Physical;
        
        [Header("Movement Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 5f;
        
        [Header("Hit Detection")]
        [SerializeField] private LayerMask targetLayer;
        
        [Header("Effects")]
        [SerializeField] private GameObject hitEffectPrefab;
        
        private Rigidbody rb;
        private GameObject owner;
        private bool hasHit = false;
        
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
        /// 투사체 소유자 설정
        /// </summary>
        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
        }
        
        /// <summary>
        /// 투사체 발사
        /// </summary>
        /// <param name="direction">발사 방향 (정규화됨)</param>
        public void Launch(Vector3 direction)
        {
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * speed;
            }
            
            // 투사체 회전 (진행 방향을 향하도록)
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            
            // 수명 후 자동 파괴
            Destroy(gameObject, lifetime);
        }
        
        /// <summary>
        /// 투사체 발사 (위치와 방향 지정)
        /// </summary>
        public void Launch(Vector3 startPosition, Vector3 direction)
        {
            transform.position = startPosition;
            Launch(direction);
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
            
            // IDamageable 인터페이스 찾기
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable == null)
                damageable = other.GetComponentInParent<IDamageable>();
            
            if (damageable != null && !damageable.IsDead)
            {
                hasHit = true;
                
                // 데미지 정보 생성
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitDirection = rb != null ? rb.linearVelocity.normalized : transform.forward;
                
                DamageInfo damageInfo = new DamageInfo(
                    damage,
                    damageType,
                    hitPoint,
                    hitDirection,
                    owner != null ? owner : gameObject
                );
                
                // 데미지 전달
                damageable.TakeDamage(damageInfo);
                
                // 충돌 처리
                OnHit(hitPoint);
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
                Destroy(hitEffect, 2f);
            }
            
            // 투사체 파괴
            Destroy(gameObject);
        }
    }
}

