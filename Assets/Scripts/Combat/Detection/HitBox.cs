using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 근접/원거리 공격의 히트박스 컴포넌트
    /// 공격 판정을 처리합니다
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HitBox : MonoBehaviour
    {
        [Header("Damage Settings")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private DamageType damageType = DamageType.Physical;
        [SerializeField] private bool dealsDotDamage = false;
        [SerializeField] private float dotDuration = 3f;
        [SerializeField] private float dotDamagePerTick = 5f;
        [SerializeField] private float dotTickInterval = 0.5f;
        
        [Header("Hit Detection")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private bool hitOnce = true;
        
        private HashSet<Collider> hitTargets = new HashSet<Collider>();
        private bool isActive = false;
        private GameObject owner;
        
        private void Awake()
        {
            // Collider가 Trigger인지 확인
            Collider col = GetComponent<Collider>();
            if (col != null && !col.isTrigger)
            {
                Debug.LogWarning($"HitBox on {gameObject.name}: Collider should be set to Trigger!");
            }
        }
        
        /// <summary>
        /// 히트박스 소유자 설정
        /// </summary>
        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
        }
        
        /// <summary>
        /// 공격 시작 - 히트박스 활성화
        /// </summary>
        public void Activate()
        {
            isActive = true;
            gameObject.SetActive(true); // GameObject도 활성화
            Debug.Log($"[HitBox] {gameObject.name} ACTIVATED - ready to hit! GameObject enabled.");
        }
        
        /// <summary>
        /// 공격 종료 - 히트박스 비활성화
        /// </summary>
        public void Deactivate()
        {
            isActive = false;
            gameObject.SetActive(false); // GameObject도 비활성화
            Debug.Log($"[HitBox] {gameObject.name} DEACTIVATED - GameObject disabled.");
        }
        
        /// <summary>
        /// 피격 목록 초기화 (다음 공격 전 호출)
        /// </summary>
        public void ResetHitTargets()
        {
            hitTargets.Clear();
            Debug.Log($"[HitBox] {gameObject.name} hit targets reset");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[HitBox] OnTriggerEnter - {other.gameObject.name}, isActive: {isActive}");
            
            if (!isActive)
            {
                Debug.Log($"[HitBox] Trigger ignored - HitBox is not active");
                return;
            }
            
            ProcessHit(other);
        }
        
        private void OnTriggerStay(Collider other)
        {
            // hitOnce가 false일 때만 지속 판정 (화염방사기 등)
            if (!isActive || hitOnce)
                return;
            
            ProcessHit(other);
        }
        
        /// <summary>
        /// 충돌 처리 및 데미지 전달
        /// </summary>
        private void ProcessHit(Collider other)
        {
            Debug.Log($"[HitBox] ProcessHit called - Collider: {other.gameObject.name}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
            
            // 레이어 체크
            if (((1 << other.gameObject.layer) & targetLayer) == 0)
            {
                Debug.Log($"[HitBox] Layer mismatch! Target layer: {targetLayer.value}, Hit layer: {other.gameObject.layer}");
                return;
            }
            
            Debug.Log($"[HitBox] Layer check passed!");
            
            // 한 번만 맞는 설정이면 중복 체크
            if (hitOnce && hitTargets.Contains(other))
            {
                Debug.Log($"[HitBox] Already hit this target (Hit Once enabled)");
                return;
            }
            
            // IDamageable 인터페이스 찾기
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable == null)
                damageable = other.GetComponentInParent<IDamageable>();
            
            if (damageable != null && !damageable.IsDead)
            {
                Debug.Log($"[HitBox] Found IDamageable on {other.gameObject.name}!");
                
                // 중복 피격 방지
                hitTargets.Add(other);
                
                // 데미지 정보 생성
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitDirection = (other.transform.position - transform.position).normalized;
                
                DamageInfo damageInfo = new DamageInfo(
                    damage,
                    damageType,
                    hitPoint,
                    hitDirection,
                    owner != null ? owner : gameObject
                );
                
                Debug.Log($"[HitBox] Dealing {damage} {damageType} damage to {other.gameObject.name}!");
                
                // 데미지 전달
                damageable.TakeDamage(damageInfo);
                
                // DoT 적용
                if (dealsDotDamage)
                {
                    Debug.Log($"[HitBox] Applying DoT effect");
                    ApplyDot(damageable, other.gameObject);
                }
            }
            else
            {
                if (damageable == null)
                {
                    Debug.LogWarning($"[HitBox] No IDamageable found on {other.gameObject.name}! Make sure it has PlayerHealth or EnemyHealth component!");
                }
                else
                {
                    Debug.Log($"[HitBox] Target {other.gameObject.name} is already dead");
                }
            }
        }
        
        /// <summary>
        /// DoT 효과 적용
        /// </summary>
        private void ApplyDot(IDamageable target, GameObject targetObject)
        {
            // 기존 DoT 효과 찾기
            DotEffect existingDot = targetObject.GetComponent<DotEffect>();
            
            if (existingDot != null)
            {
                // 이미 DoT가 있으면 시간 갱신
                existingDot.RefreshDuration(dotDuration);
            }
            else
            {
                // 새로운 DoT 생성
                GameObject dotObject = new GameObject($"DoT_{damageType}");
                dotObject.transform.SetParent(targetObject.transform);
                
                DotEffect dotEffect = dotObject.AddComponent<DotEffect>();
                dotEffect.SetDamagePerTick(dotDamagePerTick);
                dotEffect.SetTickInterval(dotTickInterval);
                dotEffect.ApplyDot(target, dotDuration, damageType, owner != null ? owner : gameObject);
            }
        }
        
        private void OnDisable()
        {
            isActive = false;
        }
    }
}

