using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 스포너 제어 UI 예제
/// AdvancedSnowmanSpawner의 웨이브 정보를 표시하고 제어합니다
/// </summary>
public class SpawnerUIController : MonoBehaviour
{
    [Header("참조")]
    [Tooltip("제어할 스포너")]
    public AdvancedSnowmanSpawner spawner;

    [Header("UI 요소 (선택사항)")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI monsterCountText;
    public TextMeshProUGUI statusText;
    public Button startButton;
    public Button stopButton;
    public Button clearButton;

    void Start()
    {
        if (spawner != null)
        {
            // 이벤트 구독
            spawner.OnWaveStart += OnWaveStarted;
            spawner.OnWaveComplete += OnWaveCompleted;
            spawner.OnAllWavesComplete += OnAllWavesCompleted;
        }

        // 버튼 이벤트 연결
        if (startButton != null)
            startButton.onClick.AddListener(() => spawner?.StartSpawning());
        
        if (stopButton != null)
            stopButton.onClick.AddListener(() => spawner?.StopSpawning());
        
        if (clearButton != null)
            clearButton.onClick.AddListener(() => spawner?.ClearAllMonsters());
    }

    void Update()
    {
        if (spawner == null) return;

        // UI 업데이트
        if (waveText != null)
            waveText.text = $"웨이브: {spawner.GetCurrentWave()}";

        if (monsterCountText != null)
            monsterCountText.text = $"몬스터: {spawner.GetActiveMonsterCount()}";

        if (statusText != null)
            statusText.text = spawner.IsSpawning() ? "스폰 중..." : "대기 중";
    }

    void OnWaveStarted(int wave)
    {
        Debug.Log($"🎮 웨이브 {wave} 시작!");
        // 여기에 UI 애니메이션, 사운드 등 추가
    }

    void OnWaveCompleted(int wave)
    {
        Debug.Log($"✅ 웨이브 {wave} 완료!");
        // 여기에 보상 지급, UI 표시 등 추가
    }

    void OnAllWavesCompleted()
    {
        Debug.Log($"🎉 모든 웨이브 클리어!");
        // 여기에 승리 화면, 최종 보상 등 추가
    }

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnWaveStart -= OnWaveStarted;
            spawner.OnWaveComplete -= OnWaveCompleted;
            spawner.OnAllWavesComplete -= OnAllWavesCompleted;
        }
    }
}

