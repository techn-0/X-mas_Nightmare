using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 적 체력 관리 클래스
    /// IDamageable 인터페이스를 구현하여 데미지 시스템과 통합됩니다
    /// </summary>
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [Header("Health System")]
        [SerializeField] private HealthSystem healthSystem = new HealthSystem();
        
        [Header("Drop Settings")]
        [SerializeField] private GameObject[] dropItems;
        [SerializeField] private float dropChance = 0.5f;
        
        private HitFeedback hitFeedback;
        
        #region IDamageable Implementation
        
        public float CurrentHP => healthSystem.CurrentHP;
        public float MaxHP => healthSystem.MaxHP;
        public bool IsDead => healthSystem.IsDead;
        
        #endregion
        
        private void Awake()
        {
            // 컴포넌트 초기화
            hitFeedback = GetComponent<HitFeedback>();
            if (hitFeedback == null)
            {
                hitFeedback = gameObject.AddComponent<HitFeedback>();
            }
            
            // 체력 시스템 초기화
            healthSystem.Initialize();
            
            // 이벤트 구독
            healthSystem.OnDeath.AddListener(HandleDeath);
        }
        
        /// <summary>
        /// 데미지를 받습니다
        /// </summary>
        public void TakeDamage(DamageInfo damageInfo)
        {
            // 사망 상태면 데미지 무시
            if (healthSystem.IsDead)
                return;
            
            // 체력 시스템에 데미지 전달
            healthSystem.TakeDamage(damageInfo);
            
            // 피격 피드백 재생
            if (hitFeedback != null)
            {
                hitFeedback.PlayHitFeedback(damageInfo.hitPoint);
            }
            
            Debug.Log($"{gameObject.name} took {damageInfo.damage} {damageInfo.damageType} damage. HP: {CurrentHP}/{MaxHP}");
        }
        
        /// <summary>
        /// 체력 회복
        /// </summary>
        public void Heal(float amount)
        {
            healthSystem.Heal(amount);
        }
        
        /// <summary>
        /// 사망 처리
        /// </summary>
        private void HandleDeath()
        {
            Debug.Log($"{gameObject.name} died!");
            
            // 드롭 아이템 생성
            DropItems();
            
            // TODO: 사망 애니메이션 재생
            // animator.SetTrigger("Die");
            
            // TODO: 적 카운트 감소
            // EnemyManager.Instance.OnEnemyDeath(this);
            
            // 임시: 일정 시간 후 오브젝트 파괴
            Destroy(gameObject, 0.5f);
        }
        
        /// <summary>
        /// 드롭 아이템 생성
        /// </summary>
        private void DropItems()
        {
            if (dropItems == null || dropItems.Length == 0)
                return;
            
            // 드롭 확률 체크
            if (Random.value > dropChance)
                return;
            
            // 랜덤하게 아이템 선택
            GameObject itemToDrop = dropItems[Random.Range(0, dropItems.Length)];
            
            if (itemToDrop != null)
            {
                // 약간의 오프셋을 두고 생성
                Vector3 dropPosition = transform.position + Vector3.up * 0.5f;
                Instantiate(itemToDrop, dropPosition, Quaternion.identity);
            }
        }
        
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (healthSystem != null && healthSystem.OnDeath != null)
            {
                healthSystem.OnDeath.RemoveListener(HandleDeath);
            }
        }
    }
}

