using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Text healthText;
        [SerializeField] private Image fillImage;
        
        [Header("Visual Settings")]
        [SerializeField] private Color fullHealthColor = Color.green;
        [SerializeField] private Color midHealthColor = Color.yellow;
        [SerializeField] private Color lowHealthColor = Color.red;
        [SerializeField] private float lowHealthThreshold = 0.3f;
        [SerializeField] private float midHealthThreshold = 0.6f;
        
        private IDamageable damageable;
        
        private void Start()
        {
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

