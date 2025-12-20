﻿using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 플레이어 체력 관리 클래스
    /// IDamageable 인터페이스를 구현하여 데미지 시스템과 통합됩니다
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [Header("Health System")]
        [SerializeField] private HealthSystem healthSystem = new HealthSystem();
        
        private HitFeedback hitFeedback;
        private InvincibilityController invincibilityController;
        private Animator animator;
        
        #region IDamageable Implementation
        
        public float CurrentHP => healthSystem.CurrentHP;
        public float MaxHP => healthSystem.MaxHP;
        public bool IsDead => healthSystem.IsDead;
        
        #endregion
        
        private void Awake()
        {
            // 컴포넌트 초기화
            animator = GetComponentInChildren<Animator>();
            
            hitFeedback = GetComponent<HitFeedback>();
            if (hitFeedback == null)
            {
                hitFeedback = gameObject.AddComponent<HitFeedback>();
            }
            
            invincibilityController = GetComponent<InvincibilityController>();
            if (invincibilityController == null)
            {
                invincibilityController = gameObject.AddComponent<InvincibilityController>();
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
            // 사망 상태이거나 무적 상태면 데미지 무시
            if (healthSystem.IsDead || (invincibilityController != null && invincibilityController.IsInvincible))
                return;
            
            // 체력 시스템에 데미지 전달
            healthSystem.TakeDamage(damageInfo);
            
            // 피격 피드백 재생
            if (hitFeedback != null)
            {
                hitFeedback.PlayHitFeedback(damageInfo.hitPoint);
            }
            
            // 무적 시간 활성화
            if (invincibilityController != null && !healthSystem.IsDead)
            {
                invincibilityController.ActivateInvincibility();
            }
            
            Debug.Log($"Player took {damageInfo.damage} {damageInfo.damageType} damage. HP: {CurrentHP}/{MaxHP}");
        }
        
        /// <summary>
        /// 체력 회복
        /// </summary>
        public void Heal(float amount)
        {
            healthSystem.Heal(amount);
            Debug.Log($"Player healed {amount}. HP: {CurrentHP}/{MaxHP}");
        }
        
        /// <summary>
        /// 사망 처리
        /// </summary>
        private void HandleDeath()
        {
            Debug.Log("Player died!");
            
            // 사망 애니메이션 재생
            if (animator != null)
            {
                animator.SetBool("isDead", true);
            }
            
            // 플레이어 컨트롤 비활성화
            var playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            
            // 공격 기능 비활성화
            var meleeAttack = GetComponent<Game.Combat.Examples.PlayerMeleeAttack>();
            if (meleeAttack != null)
            {
                meleeAttack.enabled = false;
            }
            
            var flamethrower = GetComponent<Game.Combat.Examples.PlayerFlamethrower>();
            if (flamethrower != null)
            {
                flamethrower.enabled = false;
            }
            
            // TODO: 게임오버 로직 호출
            // GameManager.Instance.OnPlayerDeath();
            
            // 임시: 일정 시간 후 오브젝트 비활성화
            // Invoke(nameof(DisablePlayer), 3f);
        }
        
        private void DisablePlayer()
        {
            gameObject.SetActive(false);
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

