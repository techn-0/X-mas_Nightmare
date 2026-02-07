using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Combat.Examples
{
    /// <summary>
    /// 플레이어 원거리 공격 예시 클래스
    /// 화염방사기를 처리합니다
    /// </summary>
    public class PlayerFlamethrower : MonoBehaviour
    {
        [Header("Flamethrower Settings")]
        [SerializeField] private HitBox flameHitBox;
        [SerializeField] private ParticleSystem flameEffect;
        [SerializeField] private AudioSource flameSound;
        
        [Header("Input - New Input System")]
        [SerializeField] private bool useMouseRightClick = true;
        [SerializeField] private Key alternativeKey = Key.F;
        
        [Header("Mouse Aiming")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private bool aimTowardsMouse = true;
        [SerializeField] private float rotationSpeed = 10f;
        
        private bool isFiring = false;
        
        /// <summary>
        /// 현재 발사 중인지 확인
        /// </summary>
        public bool IsFiring => isFiring;
        
        private void Awake()
        {
            // 카메라 자동 할당
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            
            if (flameHitBox != null)
            {
                flameHitBox.SetOwner(gameObject);
                
                // 화염방사기는 DoT를 사용하도록 설정되어 있어야 함
                // Inspector에서 설정:
                // - Deals Dot Damage: true
                // - Dot Duration: 3
                // - Hit Once: false
            }
        }
        
        private void Update()
        {
            // 마우스 조준
            if (aimTowardsMouse && isFiring)
            {
                AimTowardsMouse();
            }
            
            // 마우스 버튼을 누르고 있는 동안 발사 (New Input System)
            bool fireInput = false;
            
            if (useMouseRightClick && Mouse.current != null)
            {
                fireInput = Mouse.current.rightButton.isPressed;
            }
            else if (!useMouseRightClick && Keyboard.current != null)
            {
                // useMouseRightClick가 비활성화일 때만 키보드 입력 체크
                fireInput = Keyboard.current[alternativeKey].isPressed;
            }
            
            if (fireInput)
            {
                if (!isFiring)
                {
                    // 발사 시작 시 마우스 방향으로 회전
                    if (aimTowardsMouse)
                    {
                        AimTowardsMouse();
                    }
                    StartFiring();
                }
            }
            else
            {
                if (isFiring)
                {
                    StopFiring();
                }
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
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
        
        /// <summary>
        /// 화염방사 시작
        /// </summary>
        public void StartFiring()
        {
            if (isFiring)
                return;
            
            isFiring = true;
            
            // 히트박스 활성화
            if (flameHitBox != null)
            {
                flameHitBox.Activate();
            }
            
            // 파티클 효과 재생
            if (flameEffect != null)
            {
                flameEffect.Play();
            }
            
            // 사운드 재생
            if (flameSound != null)
            {
                flameSound.loop = true;
                flameSound.Play();
            }
            
            Debug.Log("Flamethrower started!");
        }
        
        /// <summary>
        /// 화염방사 중지
        /// </summary>
        public void StopFiring()
        {
            if (!isFiring)
                return;
            
            isFiring = false;
            
            // 히트박스 비활성화
            if (flameHitBox != null)
            {
                flameHitBox.Deactivate();
                flameHitBox.ResetHitTargets();
            }
            
            // 파티클 효과 중지
            if (flameEffect != null)
            {
                flameEffect.Stop();
            }
            
            // 사운드 중지
            if (flameSound != null)
            {
                flameSound.Stop();
            }
            
            Debug.Log("Flamethrower stopped!");
        }
        
        private void OnDisable()
        {
            // 비활성화될 때 화염방사 중지
            if (isFiring)
            {
                StopFiring();
            }
        }
    }
}

