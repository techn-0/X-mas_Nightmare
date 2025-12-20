using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private CharacterController _controller;
    private Animator _animator;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
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

        if (moveDirection.magnitude >= 0.1f)
        {
            // 이동
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // 이동 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
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
    }
}