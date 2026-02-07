
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Combat.Examples
{
    /// <summary>
    /// 체력바 UI 예시 클래스
    /// PlayerHealth 또는 EnemyHealth와 연동하여 체력을 표시합니다
    /// </summary>
    public class HealthBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MonoBehaviour healthComponent; // PlayerHealth 또는 EnemyHealth
        
        [Header("UI Elements")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image fillImage;
        
        [Header("Visual Settings")]
        [SerializeField] private Color fullHealthColor = Color.green;
        [SerializeField] private Color midHealthColor = Color.yellow;
        [SerializeField] private Color lowHealthColor = Color.red;
        [SerializeField] private float lowHealthThreshold = 0.3f;
        [SerializeField] private float midHealthThreshold = 0.6f;
        
        [Header("Position Settings")]
        [Tooltip("캐릭터로부터의 오프셋 (오른쪽 = X+, 위 = Y+, 아래 = Y-)")]
        [SerializeField] private Vector3 offsetFromCharacter = new Vector3(0f, -0.5f, 0f);
        [SerializeField] private bool followCharacter = true;
        [SerializeField] private bool faceCamera = true;
        
        private IDamageable damageable;
        private Camera mainCamera;
        
        private void Start()
        {
            mainCamera = Camera.main;
            
            // IDamageable 인터페이스 가져오기
            if (healthComponent != null)
            {
                damageable = healthComponent as IDamageable;
                
                if (damageable != null)
                {
                    UpdateHealthBar();
                    
                    // HealthSystem의 이벤트 구독
                    SubscribeToHealthEvents();
                }
                else
                {
                    Debug.LogError("HealthBarUI: healthComponent does not implement IDamageable!");
                }
            }
        }
        
        /// <summary>
        /// 매 프레임 위치 및 회전 업데이트 (카메라 업데이트 이후)
        /// </summary>
        private void LateUpdate()
        {
            if (followCharacter && healthComponent != null)
            {
                UpdatePosition();
            }
        }
        
        /// <summary>
        /// 체력바 위치 및 회전 업데이트
        /// </summary>
        private void UpdatePosition()
        {
            if (healthComponent == null)
                return;
            
            // 캐릭터의 월드 위치 가져오기
            Vector3 characterPosition = healthComponent.transform.position;
            
            // 오프셋 적용 (월드 좌표계 기준)
            transform.position = characterPosition + offsetFromCharacter;
            
            // 카메라를 향해 회전 (빌보드 효과)
            if (faceCamera && mainCamera != null)
            {
                // 카메라를 정면으로 바라보도록 회전
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                    mainCamera.transform.rotation * Vector3.up);
            }
        }
        
        /// <summary>
        /// HealthSystem의 이벤트 구독
        /// </summary>
        private void SubscribeToHealthEvents()
        {
            // Reflection을 사용하여 HealthSystem 가져오기
            var healthSystemField = healthComponent.GetType().GetField("healthSystem", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (healthSystemField != null)
            {
                HealthSystem healthSystem = healthSystemField.GetValue(healthComponent) as HealthSystem;
                
                if (healthSystem != null)
                {
                    healthSystem.OnHealthChanged.AddListener(OnHealthChanged);
                    healthSystem.OnDeath.AddListener(OnDeath);
                }
            }
        }
        
        /// <summary>
        /// 체력 변경 시 호출
        /// </summary>
        private void OnHealthChanged(float currentHP)
        {
            UpdateHealthBar();
        }
        
        /// <summary>
        /// 사망 시 호출
        /// </summary>
        private void OnDeath()
        {
            UpdateHealthBar();
        }
        
        /// <summary>
        /// 체력바 업데이트
        /// </summary>
        private void UpdateHealthBar()
        {
            if (damageable == null)
                return;
            
            float currentHP = damageable.CurrentHP;
            float maxHP = damageable.MaxHP;
            float healthPercent = currentHP / maxHP;
            
            // 슬라이더 업데이트
            if (healthSlider != null)
            {
                healthSlider.value = healthPercent;
            }
            
            // 텍스트 업데이트
            if (healthText != null)
            {
                healthText.text = $"{currentHP:F0} / {maxHP:F0}";
            }
            
            // 색상 업데이트
            if (fillImage != null)
            {
                fillImage.color = GetHealthColor(healthPercent);
            }
        }
        
        /// <summary>
        /// 체력 비율에 따른 색상 반환
        /// </summary>
        private Color GetHealthColor(float healthPercent)
        {
            if (healthPercent <= lowHealthThreshold)
            {
                return lowHealthColor;
            }
            else if (healthPercent <= midHealthThreshold)
            {
                return Color.Lerp(lowHealthColor, midHealthColor, 
                    (healthPercent - lowHealthThreshold) / (midHealthThreshold - lowHealthThreshold));
            }
            else
            {
                return Color.Lerp(midHealthColor, fullHealthColor, 
                    (healthPercent - midHealthThreshold) / (1f - midHealthThreshold));
            }
        }
        
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (healthComponent != null)
            {
                var healthSystemField = healthComponent.GetType().GetField("healthSystem", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (healthSystemField != null)
                {
                    HealthSystem healthSystem = healthSystemField.GetValue(healthComponent) as HealthSystem;
                    
                    if (healthSystem != null)
                    {
                        healthSystem.OnHealthChanged.RemoveListener(OnHealthChanged);
                        healthSystem.OnDeath.RemoveListener(OnDeath);
                    }
                }
            }
        }
    }
}

