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
        
        private bool isFiring = false;
        
        private void Awake()
        {
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
            // 마우스 버튼을 누르고 있는 동안 발사 (New Input System)
            bool fireInput = false;
            
            if (useMouseRightClick && Mouse.current != null)
            {
                fireInput = Mouse.current.rightButton.isPressed;
            }
            
            if (!fireInput && Keyboard.current != null)
            {
                fireInput = Keyboard.current[alternativeKey].isPressed;
            }
            
            if (fireInput)
            {
                if (!isFiring)
                {
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

