using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Combat.Examples
{
    /// <summary>
    /// 플레이어 근접 공격 예시 클래스
    /// 성냥팔이 소녀의 횟불 공격을 처리합니다
    /// </summary>
    public class PlayerMeleeAttack : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private HitBox weaponHitBox;
        [SerializeField] private Animator animator;
        [SerializeField] private float attackCooldown = 0.5f;
        
        [Header("Input - New Input System")]
        [SerializeField] private bool useMouseLeftClick = true;
        [SerializeField] private Key alternativeKey = Key.Space;
        
        private float lastAttackTime = 0f;
        private bool isAttacking = false;
        
        private void Awake()
        {
            if (weaponHitBox != null)
            {
                weaponHitBox.SetOwner(gameObject);
            }
        }
        
        private void Update()
        {
            // 공격 입력 처리 (New Input System)
            bool attackInput = false;
            
            if (useMouseLeftClick && Mouse.current != null)
            {
                attackInput = Mouse.current.leftButton.wasPressedThisFrame;
                if (attackInput)
                {
                    Debug.Log("[PlayerMeleeAttack] Mouse left click detected!");
                }
            }
            
            if (!attackInput && Keyboard.current != null)
            {
                attackInput = Keyboard.current[alternativeKey].wasPressedThisFrame;
                if (attackInput)
                {
                    Debug.Log($"[PlayerMeleeAttack] {alternativeKey} key pressed!");
                }
            }
            
            if (attackInput)
            {
                if (CanAttack())
                {
                    Debug.Log("[PlayerMeleeAttack] Attempting to attack...");
                    Attack();
                }
                else
                {
                    Debug.Log("[PlayerMeleeAttack] Cannot attack - on cooldown or attacking");
                }
            }
        }
        
        /// <summary>
        /// 공격 가능 여부 확인
        /// </summary>
        private bool CanAttack()
        {
            return !isAttacking && Time.time >= lastAttackTime + attackCooldown;
        }
        
        /// <summary>
        /// 공격 실행
        /// </summary>
        public void Attack()
        {
            if (!CanAttack())
            {
                Debug.Log("[PlayerMeleeAttack] Attack blocked - already attacking or on cooldown");
                return;
            }
            
            isAttacking = true;
            lastAttackTime = Time.time;
            
            Debug.Log("[PlayerMeleeAttack] Attack started!");
            
            // 애니메이션 재생
            if (animator != null)
            {
                Debug.Log("[PlayerMeleeAttack] Playing attack animation");
                animator.SetTrigger("Attack");
            }
            else
            {
                Debug.Log("[PlayerMeleeAttack] No animator - using direct attack timing");
                // 애니메이션이 없으면 바로 공격 시작
                OnAttackStart();
                Invoke(nameof(OnAttackEnd), 0.3f);
            }
        }
        
        /// <summary>
        /// 애니메이션 이벤트에서 호출 - 공격 판정 시작
        /// </summary>
        public void OnAttackStart()
        {
            Debug.Log("[PlayerMeleeAttack] OnAttackStart - Activating HitBox");
            
            if (weaponHitBox != null)
            {
                weaponHitBox.ResetHitTargets();
                weaponHitBox.Activate();
                Debug.Log("[PlayerMeleeAttack] HitBox activated!");
            }
            else
            {
                Debug.LogError("[PlayerMeleeAttack] Weapon HitBox is NULL! Please assign in Inspector!");
            }
        }
        
        /// <summary>
        /// 애니메이션 이벤트에서 호출 - 공격 판정 종료
        /// </summary>
        public void OnAttackEnd()
        {
            Debug.Log("[PlayerMeleeAttack] OnAttackEnd - Deactivating HitBox");
            
            if (weaponHitBox != null)
            {
                weaponHitBox.Deactivate();
            }
            
            isAttacking = false;
        }
    }
}

