using System.Collections;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 무적 시간 관리 클래스
    /// </summary>
    public class InvincibilityController : MonoBehaviour
    {
        [Header("Invincibility Settings")]
        [SerializeField] private float invincibilityDuration = 0.2f;
        [SerializeField] private bool enableBlinking = true;
        [SerializeField] private float blinkInterval = 0.1f;
        
        private bool isInvincible = false;
        private Coroutine invincibilityCoroutine;
        private Renderer[] renderers;
        
        /// <summary>
        /// 무적 상태 여부
        /// </summary>
        public bool IsInvincible => isInvincible;
        
        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
        
        /// <summary>
        /// 무적 시간 활성화
        /// </summary>
        public void ActivateInvincibility()
        {
            ActivateInvincibility(invincibilityDuration);
        }
        
        /// <summary>
        /// 무적 시간 활성화 (커스텀 지속 시간)
        /// </summary>
        /// <param name="duration">무적 지속 시간</param>
        public void ActivateInvincibility(float duration)
        {
            if (invincibilityCoroutine != null)
            {
                StopCoroutine(invincibilityCoroutine);
            }
            
            invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine(duration));
        }
        
        /// <summary>
        /// 무적 시간 코루틴
        /// </summary>
        private IEnumerator InvincibilityCoroutine(float duration)
        {
            isInvincible = true;
            float elapsed = 0f;
            
            // 깜빡임 효과
            if (enableBlinking && renderers.Length > 0)
            {
                while (elapsed < duration)
                {
                    // 렌더러 껐다 켜기
                    SetRenderersEnabled(false);
                    yield return new WaitForSeconds(blinkInterval);
                    
                    SetRenderersEnabled(true);
                    yield return new WaitForSeconds(blinkInterval);
                    
                    elapsed += blinkInterval * 2f;
                }
                
                // 무적 종료 시 렌더러 확실히 켜기
                SetRenderersEnabled(true);
            }
            else
            {
                yield return new WaitForSeconds(duration);
            }
            
            isInvincible = false;
            invincibilityCoroutine = null;
        }
        
        /// <summary>
        /// 렌더러 활성화/비활성화
        /// </summary>
        private void SetRenderersEnabled(bool enabled)
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = enabled;
                }
            }
        }
        
        /// <summary>
        /// 무적 상태 강제 종료
        /// </summary>
        public void CancelInvincibility()
        {
            if (invincibilityCoroutine != null)
            {
                StopCoroutine(invincibilityCoroutine);
                invincibilityCoroutine = null;
            }
            
            isInvincible = false;
            SetRenderersEnabled(true);
        }
        
        private void OnDestroy()
        {
            if (invincibilityCoroutine != null)
            {
                StopCoroutine(invincibilityCoroutine);
                invincibilityCoroutine = null;
            }
        }
    }
}

