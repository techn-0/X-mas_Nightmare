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
        
        [Header("Mouse Aiming")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private bool aimTowardsMouse = true;
        [SerializeField] private float rotationSpeed = 10f;
        
        private float lastAttackTime = 0f;
        private bool isAttacking = false;
        
        /// <summary>
        /// 현재 공격 중인지 확인
        /// </summary>
        public bool IsAttacking => isAttacking;
        
        private void Awake()
        {
            // 카메라 자동 할당
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            
            if (weaponHitBox != null)
            {
                weaponHitBox.SetOwner(gameObject);
                // 히트박스가 처음에 비활성화되어 있는지 확인
                weaponHitBox.Deactivate();
            }
            else
            {
                Debug.LogError("[PlayerMeleeAttack] Weapon HitBox is not assigned! Please assign it in the Inspector!");
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
            else if (!useMouseLeftClick && Keyboard.current != null)
            {
                // useMouseLeftClick가 비활성화일 때만 키보드 입력 체크
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
            
            // 공격하기 전에 마우스 방향으로 회전
            if (aimTowardsMouse)
            {
                AimTowardsMouse();
            }
            
            isAttacking = true;
            lastAttackTime = Time.time;
            
            Debug.Log("[PlayerMeleeAttack] Attack started!");
            
            // 애니메이션 재생
            if (animator != null)
            {
                Debug.Log("[PlayerMeleeAttack] Playing attack animation");
                animator.SetTrigger("MeleeAttack");
                
                // 애니메이션 이벤트가 없을 경우를 대비한 타이밍
                // 애니메이션 이벤트가 있으면 OnAttackStart/End가 이벤트에서 호출되고,
                // 없으면 이 Invoke가 호출됨
                Invoke(nameof(OnAttackStart), 0.1f);
                Invoke(nameof(OnAttackEnd), 0.5f);
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
        /// 마우스 위치를 향해 캐릭터 회전
        /// </summary>
        private void AimTowardsMouse()
        {
            if (mainCamera == null || Mouse.current == null)
                return;
            
            // 마우스 스크린 위치를 월드 위치로 변환
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            
            // 캐릭터와 같은 높이의 평면과 교차점 찾기
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float rayDistance;
            
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 targetPoint = ray.GetPoint(rayDistance);
                Vector3 direction = targetPoint - transform.position;
                direction.y = 0; // Y축 회전만 사용
                
                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    // 공격 시에는 즉시 회전 (부드러운 회전을 원하면 rotationSpeed 사용)
                    transform.rotation = targetRotation;
                }
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
                Debug.Log($"[PlayerMeleeAttack] HitBox activated! GameObject active: {weaponHitBox.gameObject.activeSelf}");
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
                Debug.Log($"[PlayerMeleeAttack] HitBox deactivated! GameObject active: {weaponHitBox.gameObject.activeSelf}");
            }
            
            isAttacking = false;
            Debug.Log($"[PlayerMeleeAttack] Attack finished. Can attack again after: {attackCooldown}s");
        }
    }
}

