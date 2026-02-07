using UnityEngine;
using TMPro;

/// <summary>
/// 보스 스폰 타이머 UI 컨트롤러
/// 타이머와 함께 추가적인 UI 요소들을 제어합니다.
/// </summary>
public class BossSpawnTimerUI : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private BossSpawnTimer bossSpawnTimer;
    
    [Header("UI 요소")]
    [SerializeField] private GameObject warningPanel; // 경고 패널 (타이머 시작 시 표시)
    [SerializeField] private TextMeshProUGUI warningText; // 경고 메시지
    [SerializeField] private CanvasGroup timerCanvasGroup; // 타이머 페이드 효과용
    
    [Header("애니메이션 설정")]
    [SerializeField] private bool useFadeEffect = true; // 페이드 효과 사용
    [SerializeField] private float fadeSpeed = 2f; // 페이드 속도
    [SerializeField] private bool useScaleEffect; // 스케일 애니메이션 사용
    [SerializeField] private float pulseSpeed = 1f; // 펄스 속도
    [SerializeField] private float pulseScale = 1.2f; // 펄스 스케일
    
    [Header("경고 메시지")]
    [SerializeField] private string warningMessage = "⚠️ 강력한 적이 다가오고 있습니다! ⚠️";
    [SerializeField] private float warningDisplayTime = 3f; // 경고 표시 시간
    
    [Header("색상 설정")]
    [SerializeField] private bool changeColorByTime = true; // 시간에 따라 색상 변경
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow; // 10초 이하
    [SerializeField] private Color urgentColor = Color.red; // 5초 이하
    [SerializeField] private float warningThreshold = 10f; // 경고 색상 임계값
    [SerializeField] private float urgentThreshold = 5f; // 긴급 색상 임계값
    
    private TextMeshProUGUI timerText;
    private Vector3 originalScale;
    private bool isWarningShown;
    
    private void Awake()
    {
        // BossSpawnTimer 자동 찾기
        if (bossSpawnTimer == null)
        {
            bossSpawnTimer = FindFirstObjectByType<BossSpawnTimer>();
        }
        
        // 타이머 텍스트 참조 가져오기
        if (timerCanvasGroup != null)
        {
            timerText = timerCanvasGroup.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (timerText != null)
        {
            originalScale = timerText.transform.localScale;
        }
        
        // 경고 패널 초기 비활성화
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
    }
    
    private void Start()
    {
        // 이벤트 구독
        if (bossSpawnTimer != null)
        {
            // 타이머 시작 시 경고 표시
            var onTimerStart = typeof(BossSpawnTimer)
                .GetField("onTimerStart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (onTimerStart != null)
            {
                var timerStartEvent = onTimerStart.GetValue(bossSpawnTimer) as UnityEngine.Events.UnityEvent;
                timerStartEvent?.AddListener(OnTimerStart);
            }
            
            // 보스 스폰 시 UI 정리
            var onBossSpawned = typeof(BossSpawnTimer)
                .GetField("onBossSpawned", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (onBossSpawned != null)
            {
                var bossSpawnedEvent = onBossSpawned.GetValue(bossSpawnTimer) as UnityEngine.Events.UnityEvent;
                bossSpawnedEvent?.AddListener(OnBossSpawned);
            }
        }
    }
    
    private void Update()
    {
        if (bossSpawnTimer == null || !bossSpawnTimer.IsTimerRunning())
            return;
        
        float remainingTime = bossSpawnTimer.GetRemainingTime();
        
        // 색상 변경
        if (changeColorByTime && timerText != null)
        {
            UpdateTimerColor(remainingTime);
        }
        
        // 스케일 애니메이션
        if (useScaleEffect && timerText != null)
        {
            UpdateTimerScale(remainingTime);
        }
        
        // 페이드 효과
        if (useFadeEffect && timerCanvasGroup != null)
        {
            UpdateFadeEffect(remainingTime);
        }
    }
    
    /// <summary>
    /// 타이머 시작 시 호출
    /// </summary>
    private void OnTimerStart()
    {
        isWarningShown = false;
        ShowWarning();
    }
    
    /// <summary>
    /// 보스 스폰 시 호출
    /// </summary>
    private void OnBossSpawned()
    {
        HideWarning();
    }
    
    /// <summary>
    /// 경고 메시지 표시
    /// </summary>
    private void ShowWarning()
    {
        if (warningPanel != null && !isWarningShown)
        {
            warningPanel.SetActive(true);
            
            if (warningText != null)
            {
                warningText.text = warningMessage;
            }
            
            isWarningShown = true;
            
            // 일정 시간 후 경고 숨기기
            Invoke(nameof(HideWarning), warningDisplayTime);
        }
    }
    
    /// <summary>
    /// 경고 메시지 숨기기
    /// </summary>
    private void HideWarning()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 남은 시간에 따라 색상 업데이트
    /// </summary>
    private void UpdateTimerColor(float remainingTime)
    {
        Color targetColor;
        
        if (remainingTime <= urgentThreshold)
        {
            targetColor = urgentColor;
        }
        else if (remainingTime <= warningThreshold)
        {
            targetColor = warningColor;
        }
        else
        {
            targetColor = normalColor;
        }
        
        timerText.color = Color.Lerp(timerText.color, targetColor, Time.deltaTime * fadeSpeed);
    }
    
    /// <summary>
    /// 타이머 스케일 애니메이션
    /// </summary>
    private void UpdateTimerScale(float remainingTime)
    {
        if (remainingTime <= urgentThreshold)
        {
            // 긴급 시 펄스 효과
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed * 5f) * (pulseScale - 1f) * 0.5f;
            timerText.transform.localScale = originalScale * pulse;
        }
        else if (remainingTime <= warningThreshold)
        {
            // 경고 시 느린 펄스
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed * 2f) * (pulseScale - 1f) * 0.3f;
            timerText.transform.localScale = originalScale * pulse;
        }
        else
        {
            timerText.transform.localScale = originalScale;
        }
    }
    
    /// <summary>
    /// 페이드 효과
    /// </summary>
    private void UpdateFadeEffect(float remainingTime)
    {
        if (remainingTime <= urgentThreshold)
        {
            // 긴급 시 깜빡임
            float alpha = 0.5f + Mathf.Sin(Time.time * pulseSpeed * 10f) * 0.5f;
            timerCanvasGroup.alpha = alpha;
        }
        else
        {
            timerCanvasGroup.alpha = 1f;
        }
    }
    
    /// <summary>
    /// 타이머 즉시 시작 (외부 호출용)
    /// </summary>
    public void StartBossTimer()
    {
        if (bossSpawnTimer != null)
        {
            bossSpawnTimer.StartTimer();
        }
    }
    
    /// <summary>
    /// 타이머 중지 (외부 호출용)
    /// </summary>
    public void StopBossTimer()
    {
        if (bossSpawnTimer != null)
        {
            bossSpawnTimer.StopTimer();
        }
        HideWarning();
    }
}

