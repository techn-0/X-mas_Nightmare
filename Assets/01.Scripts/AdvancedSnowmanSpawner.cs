using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 웨이브 시스템이 포함된 고급 눈사람 스포너
/// </summary>
public class AdvancedSnowmanSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableMonster
    {
        [Tooltip("스폰할 몬스터 프리팹")]
        public GameObject prefab;
        
        [Tooltip("스폰 확률")]
        [Range(1, 100)]
        public int spawnWeight = 50;

        [Tooltip("최소 웨이브 (이 웨이브부터 등장)")]
        [Min(1)]
        public int minWave = 1;
    }

    [System.Serializable]
    public class WaveSettings
    {
        [Tooltip("웨이브 번호")]
        public int waveNumber = 1;

        [Tooltip("이 웨이브에서 스폰할 총 몬스터 수")]
        [Min(1)]
        public int monstersToSpawn = 10;

        [Tooltip("스폰 간격")]
        [Range(0.5f, 10f)]
        public float spawnInterval = 2f;

        [Tooltip("웨이브 시작 전 대기 시간")]
        [Range(0f, 30f)]
        public float prepareTime = 5f;
    }

    [Header("스폰할 몬스터 설정")]
    public List<SpawnableMonster> monsters = new List<SpawnableMonster>();

    [Header("웨이브 모드")]
    [Tooltip("웨이브 모드 사용")]
    public bool useWaveMode;

    [Tooltip("웨이브 설정 (웨이브 모드 사용시)")]
    public List<WaveSettings> waves = new List<WaveSettings>();

    [Header("일반 스폰 설정 (웨이브 모드 미사용시)")]
    [Range(0.5f, 30f)]
    public float spawnInterval = 3f;

    [Tooltip("스폰 포인트 개수")]
    [Range(1, 20)]
    public int spawnPointCount = 4;

    [Tooltip("스폰 범위 반경")]
    [Range(5f, 100f)]
    public float spawnRadius = 20f;

    [Header("제한 설정")]
    [Tooltip("최대 동시 스폰 수 (0 = 무제한)")]
    [Range(0, 200)]
    public int maxActiveMonsters = 20;

    [Tooltip("게임 시작시 자동 스폰")]
    public bool autoStart = true;

    [Header("난이도 증가 (웨이브 모드 미사용시)")]
    [Tooltip("시간이 지날수록 스폰 간격 감소")]
    public bool increaseDifficulty;

    [Tooltip("난이도 증가율 (초당 감소량)")]
    [Range(0f, 0.1f)]
    public float difficultyIncreaseRate = 0.01f;

    [Tooltip("최소 스폰 간격")]
    [Range(0.1f, 5f)]
    public float minSpawnInterval = 0.5f;

    [Header("디버그")]
    public bool showSpawnPoints = true;
    public bool showDebugInfo = true;

    // 런타임 데이터
    private List<GameObject> activeMonsters = new List<GameObject>();
    private bool isSpawning;
    private Vector3[] spawnPoints;
    private int currentWave;
    private int monstersSpawnedThisWave;
    private float currentSpawnInterval;

    // 이벤트
    public System.Action<int> OnWaveStart;
    public System.Action<int> OnWaveComplete;
    public System.Action OnAllWavesComplete;

    void Start()
    {
        GenerateSpawnPoints();
        currentSpawnInterval = spawnInterval;
        
        if (autoStart)
        {
            StartSpawning();
        }
    }

    void Update()
    {
        if (showDebugInfo && isSpawning)
        {
            string info = useWaveMode
                ? $"웨이브: {currentWave}/{waves.Count} | 스폰: {monstersSpawnedThisWave} | 활성: {activeMonsters.Count}"
                : $"활성 몬스터: {activeMonsters.Count}/{maxActiveMonsters} | 간격: {currentSpawnInterval:F2}초";
            
            Debug.Log(info);
        }
    }

    void GenerateSpawnPoints()
    {
        spawnPoints = new Vector3[spawnPointCount];
        
        for (int i = 0; i < spawnPointCount; i++)
        {
            float angle = i * (360f / spawnPointCount);
            float radian = angle * Mathf.Deg2Rad;
            
            Vector3 offset = new Vector3(
                Mathf.Cos(radian) * spawnRadius,
                0,
                Mathf.Sin(radian) * spawnRadius
            );
            
            spawnPoints[i] = transform.position + offset;
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            
            if (useWaveMode)
            {
                StartCoroutine(WaveSpawnRoutine());
            }
            else
            {
                StartCoroutine(ContinuousSpawnRoutine());
            }
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    IEnumerator WaveSpawnRoutine()
    {
        currentWave = 0;

        foreach (var wave in waves)
        {
            currentWave = wave.waveNumber;
            monstersSpawnedThisWave = 0;

            Debug.Log($"웨이브 {currentWave} 준비 중... ({wave.prepareTime}초)");
            OnWaveStart?.Invoke(currentWave);
            
            yield return new WaitForSeconds(wave.prepareTime);

            Debug.Log($"웨이브 {currentWave} 시작!");

            while (monstersSpawnedThisWave < wave.monstersToSpawn && isSpawning)
            {
                CleanupDeadMonsters();
                
                if (maxActiveMonsters == 0 || activeMonsters.Count < maxActiveMonsters)
                {
                    SpawnMonster(currentWave);
                    monstersSpawnedThisWave++;
                }

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            // 모든 몬스터가 처치될 때까지 대기
            Debug.Log($"웨이브 {currentWave} - 남은 몬스터 처치 대기 중...");
            while (activeMonsters.Count > 0)
            {
                CleanupDeadMonsters();
                yield return new WaitForSeconds(0.5f);
            }

            Debug.Log($"웨이브 {currentWave} 완료!");
            OnWaveComplete?.Invoke(currentWave);
        }

        Debug.Log("모든 웨이브 완료!");
        OnAllWavesComplete?.Invoke();
        isSpawning = false;
    }

    IEnumerator ContinuousSpawnRoutine()
    {
        float elapsedTime = 0f;

        while (isSpawning)
        {
            yield return new WaitForSeconds(currentSpawnInterval);

            CleanupDeadMonsters();
            
            if (maxActiveMonsters == 0 || activeMonsters.Count < maxActiveMonsters)
            {
                SpawnMonster(1);
            }

            // 난이도 증가
            if (increaseDifficulty)
            {
                elapsedTime += currentSpawnInterval;
                currentSpawnInterval = Mathf.Max(
                    minSpawnInterval,
                    spawnInterval - (elapsedTime * difficultyIncreaseRate)
                );
            }
        }
    }

    void SpawnMonster(int currentWaveNumber)
    {
        GameObject selectedPrefab = SelectRandomMonster(currentWaveNumber);
        if (selectedPrefab == null) return;

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject monster = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        activeMonsters.Add(monster);
    }

    GameObject SelectRandomMonster(int waveNumber)
    {
        List<SpawnableMonster> availableMonsters = new List<SpawnableMonster>();
        int totalWeight = 0;

        foreach (var monster in monsters)
        {
            if (monster.prefab != null && monster.minWave <= waveNumber)
            {
                availableMonsters.Add(monster);
                totalWeight += monster.spawnWeight;
            }
        }

        if (totalWeight == 0) return null;

        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var monster in availableMonsters)
        {
            currentWeight += monster.spawnWeight;
            if (randomValue < currentWeight)
            {
                return monster.prefab;
            }
        }

        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 basePosition = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        return basePosition + new Vector3(
            Random.Range(-2f, 2f),
            0,
            Random.Range(-2f, 2f)
        );
    }

    void CleanupDeadMonsters()
    {
        activeMonsters.RemoveAll(monster => monster == null);
    }

    public void ClearAllMonsters()
    {
        foreach (var monster in activeMonsters)
        {
            if (monster != null)
            {
                Destroy(monster);
            }
        }
        activeMonsters.Clear();
    }

    public int GetCurrentWave() => currentWave;
    public int GetActiveMonsterCount() => activeMonsters.Count;
    public bool IsSpawning() => isSpawning;

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            GenerateSpawnPoints();
        }
    }

    void OnDrawGizmos()
    {
        if (!showSpawnPoints) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Vector3[] points = spawnPoints;
        if (points == null || points.Length != spawnPointCount)
        {
            points = new Vector3[spawnPointCount];
            for (int i = 0; i < spawnPointCount; i++)
            {
                float angle = i * (360f / spawnPointCount);
                float radian = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(
                    Mathf.Cos(radian) * spawnRadius,
                    0,
                    Mathf.Sin(radian) * spawnRadius
                );
                points[i] = transform.position + offset;
            }
        }

        Gizmos.color = useWaveMode ? Color.cyan : Color.green;
        foreach (var point in points)
        {
            Gizmos.DrawSphere(point, 0.5f);
            Gizmos.DrawLine(transform.position, point);
        }
    }
}

