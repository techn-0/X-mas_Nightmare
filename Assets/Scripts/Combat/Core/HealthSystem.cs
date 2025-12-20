using UnityEngine;
using UnityEngine.Events;

namespace Game.Combat
{
    /// <summary>
    /// 체력 관리 시스템 클래스
    /// </summary>
    [System.Serializable]
    public class HealthSystem
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHP = 100f;
        [SerializeField] private bool isInvincible = false;
        
        private float currentHP;
        private bool isDead = false;
        
        [Header("Events")]
        public UnityEvent<DamageInfo> OnDamaged;
        public UnityEvent OnDeath;
        public UnityEvent<float> OnHealthChanged;
        
        /// <summary>
        /// 현재 체력
        /// </summary>
        public float CurrentHP => currentHP;
        
        /// <summary>
        /// 최대 체력
        /// </summary>
        public float MaxHP => maxHP;
        
        /// <summary>
        /// 사망 여부
        /// </summary>
        public bool IsDead => isDead;
        
        /// <summary>
        /// 무적 상태 여부
        /// </summary>
        public bool IsInvincible
        {
            get => isInvincible;
            set => isInvincible = value;
        }
        
        /// <summary>
        /// 체력 시스템 초기화
        /// </summary>
        public void Initialize()
        {
            currentHP = maxHP;
            isDead = false;
            OnHealthChanged?.Invoke(currentHP);
        }
        
        /// <summary>
        /// 데미지를 받습니다
        /// </summary>
        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            if (isDead || isInvincible)
                return;
            
            currentHP -= damageInfo.damage;
            currentHP = Mathf.Max(currentHP, 0f);
            
            OnDamaged?.Invoke(damageInfo);
            OnHealthChanged?.Invoke(currentHP);
            
            if (currentHP <= 0f && !isDead)
            {
                Die();
            }
        }
        
        /// <summary>
        /// 체력을 회복합니다
        /// </summary>
        public void Heal(float amount)
        {
            if (isDead)
                return;
            
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, maxHP);
            
            OnHealthChanged?.Invoke(currentHP);
        }
        
        /// <summary>
        /// 사망 처리
        /// </summary>
        public virtual void Die()
        {
            if (isDead)
                return;
            
            isDead = true;
            currentHP = 0f;
            
            OnDeath?.Invoke();
        }
        
        /// <summary>
        /// 최대 체력 설정
        /// </summary>
        public void SetMaxHP(float newMaxHP)
        {
            maxHP = newMaxHP;
            currentHP = Mathf.Min(currentHP, maxHP);
            OnHealthChanged?.Invoke(currentHP);
        }
    }
}

