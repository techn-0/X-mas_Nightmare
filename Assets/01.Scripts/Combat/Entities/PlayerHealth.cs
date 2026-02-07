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
        
        [Header("Audio")]
        [SerializeField] private AudioClip damageSound;
        [SerializeField] private AudioClip deathSound;
        [Range(0f, 1f)]
        [SerializeField] private float soundVolume = 1f;
        
        private HitFeedback hitFeedback;
        private InvincibilityController invincibilityController;
        private Animator animator;
        private AudioSource audioSource;
        
        #region IDamageable Implementation
        
        public float CurrentHP => healthSystem.CurrentHP;
        public float MaxHP => healthSystem.MaxHP;
        public bool IsDead => healthSystem.IsDead;
        
        #endregion
        
        /// <summary>
        /// 외부에서 HealthSystem 이벤트에 접근하기 위한 프로퍼티
        /// </summary>
        public HealthSystem HealthSystemRef => healthSystem;
        
        private void Awake()
        {
            // 컴포넌트 초기화
            animator = GetComponentInChildren<Animator>();
            
            // AudioSource 초기화
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
            
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
            
            // 데미지 효과음 재생
            if (audioSource != null && damageSound != null && !healthSystem.IsDead)
            {
                audioSource.PlayOneShot(damageSound, soundVolume);
            }
            
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
            
            // 사망 효과음 재생
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound, soundVolume);
            }
            
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
            
            // 게임 매니저에 플레이어 사망 알림
            if (global::GameManager.Instance != null)
            {
                global::GameManager.Instance.PlayerDied();
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

