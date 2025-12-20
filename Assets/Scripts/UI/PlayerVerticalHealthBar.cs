using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Combat.UI
{
    /// <summary>
    /// 플레이어 전용 세로 체력바 UI
    /// 플레이어 오브젝트의 오른쪽에 월드 스페이스로 표시됩니다
    /// </summary>
    public class PlayerVerticalHealthBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerHealth playerHealth;
        
        [Header("UI Elements")]
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private CanvasGroup canvasGroup;
        
        [Header("Position Settings")]
        [Tooltip("플레이어로부터의 오프셋 (오른쪽 = X+, 위 = Y+, 아래 = Y-)")]
        [SerializeField] private Vector3 offsetFromPlayer = new Vector3(0f, -0.5f, 0f);
        [SerializeField] private bool followPlayer = true;
        [SerializeField] private bool faceCamera = true;
        
        [Header("Visual Settings")]
        [SerializeField] private Color fullHealthColor = new Color(0.2f, 1f, 0.2f);
        [SerializeField] private Color midHealthColor = new Color(1f, 1f, 0.2f);
        [SerializeField] private Color lowHealthColor = new Color(1f, 0.2f, 0.2f);
        [SerializeField] private float lowHealthThreshold = 0.3f;
        [SerializeField] private float midHealthThreshold = 0.6f;
        
        [Header("Animation Settings")]
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private bool showDamageFlash = true;
        [SerializeField] private float flashDuration = 0.2f;
        
        private float targetFillAmount;
        private float currentFillAmount;
        private Camera mainCamera;
        private bool isFlashing = false;
        private float flashTimer = 0f;
        
        private void Start()
        {
            mainCamera = Camera.main;
            
            // PlayerHealth 자동 찾기
            if (playerHealth == null)
            {
                playerHealth = FindObjectOfType<PlayerHealth>();
                if (playerHealth == null)
                {
                    Debug.LogError("PlayerVerticalHealthBar: PlayerHealth를 찾을 수 없습니다!");
                    enabled = false;
                    return;
                }
            }
            
            // HealthSystem 이벤트 구독
            SubscribeToHealthEvents();
            
            // 초기 체력바 설정
            UpdateHealthBar(immediate: true);
        }
        
        private void SubscribeToHealthEvents()
        {
            if (playerHealth != null && playerHealth.HealthSystemRef != null)
            {
                playerHealth.HealthSystemRef.OnHealthChanged.AddListener(OnHealthChanged);
                playerHealth.HealthSystemRef.OnDamaged.AddListener(OnDamaged);
                playerHealth.HealthSystemRef.OnDeath.AddListener(OnDeath);
            }
        }
        
        private void Update()
        {
            // 체력바 애니메이션
            if (Mathf.Abs(currentFillAmount - targetFillAmount) > 0.001f)
            {
                currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
                if (fillImage != null)
                {
                    fillImage.fillAmount = currentFillAmount;
                }
            }
            
            // 피격 플래시 효과
            if (isFlashing)
            {
                flashTimer += Time.deltaTime;
                if (flashTimer >= flashDuration)
                {
                    isFlashing = false;
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = 1f;
                    }
                }
                else
                {
                    float alpha = 0.5f + Mathf.Sin(flashTimer / flashDuration * Mathf.PI * 4f) * 0.5f;
                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = alpha;
                    }
                }
            }
        }
        
        private void LateUpdate()
        {
            // 플레이어 위치 추적 (카메라 업데이트 이후)
            if (followPlayer && playerHealth != null)
            {
                UpdatePosition();
            }
        }
        
        private void UpdatePosition()
        {
            if (playerHealth == null)
                return;
            
            // 플레이어의 월드 위치 가져오기
            Vector3 playerPosition = playerHealth.transform.position;
            
            // 오프셋 적용 (월드 좌표계 기준)
            transform.position = playerPosition + offsetFromPlayer;
            
            // 카메라를 향해 회전 (빌보드)
            if (faceCamera && mainCamera != null)
            {
                // 카메라를 정면으로 바라보도록 회전
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                    mainCamera.transform.rotation * Vector3.up);
            }
        }
        
        private void OnHealthChanged(float currentHP)
        {
            UpdateHealthBar(immediate: false);
        }
        
        private void OnDamaged(DamageInfo damageInfo)
        {
            if (showDamageFlash)
            {
                isFlashing = true;
                flashTimer = 0f;
            }
        }
        
        private void OnDeath()
        {
            UpdateHealthBar(immediate: true);
            
            // 사망 시 체력바를 서서히 사라지게
            if (canvasGroup != null)
            {
                StartCoroutine(FadeOut());
            }
        }
        
        private void UpdateHealthBar(bool immediate)
        {
            if (playerHealth == null || fillImage == null)
                return;
            
            float currentHP = playerHealth.CurrentHP;
            float maxHP = playerHealth.MaxHP;
            float healthPercent = maxHP > 0 ? currentHP / maxHP : 0f;
            
            targetFillAmount = healthPercent;
            
            if (immediate)
            {
                currentFillAmount = targetFillAmount;
                fillImage.fillAmount = currentFillAmount;
            }
            
            // 색상 업데이트
            fillImage.color = GetHealthColor(healthPercent);
            
            // 텍스트 업데이트
            if (healthText != null)
            {
                healthText.text = $"{currentHP:F0}\n{maxHP:F0}";
            }
        }
        
        private Color GetHealthColor(float healthPercent)
        {
            if (healthPercent <= lowHealthThreshold)
            {
                return lowHealthColor;
            }
            else if (healthPercent <= midHealthThreshold)
            {
                float t = (healthPercent - lowHealthThreshold) / (midHealthThreshold - lowHealthThreshold);
                return Color.Lerp(lowHealthColor, midHealthColor, t);
            }
            else
            {
                float t = (healthPercent - midHealthThreshold) / (1f - midHealthThreshold);
                return Color.Lerp(midHealthColor, fullHealthColor, t);
            }
        }
        
        private System.Collections.IEnumerator FadeOut()
        {
            float elapsed = 0f;
            float duration = 1f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                }
                yield return null;
            }
        }
        
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (playerHealth != null && playerHealth.HealthSystemRef != null)
            {
                playerHealth.HealthSystemRef.OnHealthChanged.RemoveListener(OnHealthChanged);
                playerHealth.HealthSystemRef.OnDamaged.RemoveListener(OnDamaged);
                playerHealth.HealthSystemRef.OnDeath.RemoveListener(OnDeath);
            }
        }
    }
}

