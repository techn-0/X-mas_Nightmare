using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    
    [Header("Combat Integration")]
    [SerializeField] private bool allowRotationDuringAttack = false;
    
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private GameObject dashEffectPrefab;
    [SerializeField] private float dashEffectDuration = 0.5f;

    private CharacterController _controller;
    private Animator _animator;
    private bool isAttacking = false;
    private bool isFiring = false;
    private Vector3 velocity;
    
    // Dash variables
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection = Vector3.zero;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 쿨타임 타이머 감소
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        
        // 공격/발사 상태 체크
        CheckCombatState();
        
        // 입력 처리
        Vector2 inputVector = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) inputVector.y += 1;
            if (Keyboard.current.sKey.isPressed) inputVector.y -= 1;
            if (Keyboard.current.aKey.isPressed) inputVector.x -= 1;
            if (Keyboard.current.dKey.isPressed) inputVector.x += 1;
        }

        // 이동 방향 계산
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
        
        // 대시 입력 체크 (스페이스바)
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame 
            && !isDashing && dashCooldownTimer <= 0f && moveDirection.magnitude >= 0.1f)
        {
            StartDash(moveDirection);
        }
        
        // 대시 중일 때
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
            else
            {
                // 대시 이동
                _controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            }
        }
        // 일반 이동
        else if (moveDirection.magnitude >= 0.1f)
        {
            // 이동
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // 공격/발사 중이 아니거나 허용된 경우에만 이동 방향을 바라보도록 회전
            bool canRotate = allowRotationDuringAttack || (!isAttacking && !isFiring);
            if (canRotate)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
            // 애니메이션 Speed 파라미터 설정 (Run 상태)
            if (_animator != null)
            {
                _animator.SetFloat("Speed", 1f);
            }
        }
        else
        {
            // 정지 상태일 때 애니메이션 Speed 파라미터 설정 (Idle 상태)
            if (_animator != null)
            {
                _animator.SetFloat("Speed", 0f);
            }
        }
        
        // 중력 적용
        if (_controller.isGrounded)
        {
            // 땅에 닿아 있으면 y축 속도를 약간의 음수값으로 설정 (경사면에서도 안정적으로 지면에 붙어있도록)
            velocity.y = -2f;
        }
        else
        {
            // 공중에 있으면 중력 가속도 적용
            velocity.y += gravity * Time.deltaTime;
        }
        
        // 수직 이동 적용
        _controller.Move(velocity * Time.deltaTime);
    }
    
    /// <summary>
    /// 공격/발사 상태를 체크합니다
    /// </summary>
    private void CheckCombatState()
    {
        // 근접 공격 상태 체크
        var meleeAttack = GetComponent<Game.Combat.Examples.PlayerMeleeAttack>();
        if (meleeAttack != null)
        {
            isAttacking = meleeAttack.IsAttacking;
        }
        
        // 원거리 공격 상태 체크
        var flamethrower = GetComponent<Game.Combat.Examples.PlayerFlamethrower>();
        if (flamethrower != null)
        {
            isFiring = flamethrower.IsFiring;
        }
    }
    
    /// <summary>
    /// 대시를 시작합니다
    /// </summary>
    private void StartDash(Vector3 direction)
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashDirection = direction;
        
        // 대시 이펙트 활성화
        if (dashEffectPrefab != null)
        {
            dashEffectPrefab.SetActive(true);
            StartCoroutine(DeactivateDashEffectAfterDelay());
        }
    }
    
    /// <summary>
    /// 대시 이펙트를 일정 시간 후 비활성화합니다
    /// </summary>
    private System.Collections.IEnumerator DeactivateDashEffectAfterDelay()
    {
        yield return new WaitForSeconds(dashEffectDuration);
        
        if (dashEffectPrefab != null)
        {
            dashEffectPrefab.SetActive(false);
        }
    }
}