using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// 보스 스폰 타이머 시스템
/// 30초 카운트다운 후 보스를 특정 위치에 스폰합니다.
/// </summary>
public class BossSpawnTimer : MonoBehaviour
{
    [Header("타이머 설정")]
    [SerializeField] private float spawnTime = 30f;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private bool startOnAwake = true;
    
    [Header("보스 설정")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;
    
    private float currentTime;
    private bool isTimerRunning;
    private GameObject spawnedBoss;
    
    private void Start()
    {
        if (startOnAwake)
        {
            StartTimer();
        }
    }
    
    /// <summary>
    /// 타이머 시작
    /// </summary>
    public void StartTimer()
    {
        if (isTimerRunning) return;
        if (bossPrefab == null || spawnPoint == null) return;
        
        currentTime = spawnTime;
        isTimerRunning = true;
        
        // 타이머 텍스트 활성화
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }
        
        StartCoroutine(TimerCoroutine());
    }
    
    /// <summary>
    /// 타이머 중지
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;
        StopAllCoroutines();
    }
    
    /// <summary>
    /// 타이머 리셋
    /// </summary>
    public void ResetTimer()
    {
        StopTimer();
        currentTime = spawnTime;
        UpdateTimerUI();
    }
    
    /// <summary>
    /// 타이머 코루틴
    /// </summary>
    private IEnumerator TimerCoroutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }
        
        currentTime = 0;
        UpdateTimerUI();
        isTimerRunning = false;
        
        SpawnBoss();
    }
    
    /// <summary>
    /// 타이머 UI 업데이트
    /// </summary>
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int displayTime = Mathf.CeilToInt(currentTime);
            if (displayTime < 0) displayTime = 0;
            timerText.text = displayTime.ToString();
        }
    }
    
    /// <summary>
    /// 보스 스폰
    /// </summary>
    private void SpawnBoss()
    {
        // 타이머 텍스트 비활성화
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
        
        spawnedBoss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    
    /// <summary>
    /// 즉시 보스 스폰 (테스트용)
    /// </summary>
    public void SpawnBossImmediately()
    {
        StopTimer();
        SpawnBoss();
    }
    
    /// <summary>
    /// 스폰된 보스 가져오기
    /// </summary>
    public GameObject GetSpawnedBoss()
    {
        return spawnedBoss;
    }
    
    /// <summary>
    /// 현재 남은 시간 가져오기
    /// </summary>
    public float GetRemainingTime()
    {
        return currentTime;
    }
    
    /// <summary>
    /// 타이머 실행 중 여부
    /// </summary>
    public bool IsTimerRunning()
    {
        return isTimerRunning;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
        }
    }
}

