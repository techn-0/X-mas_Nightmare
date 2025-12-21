using UnityEngine;

/// <summary>
/// 힐팩을 특정 위치에 주기적으로 스폰합니다
/// </summary>
public class HealthPackSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject healthPackPrefab;
    [SerializeField] private float spawnInterval = 30f; // 30초마다 스폰
    [SerializeField] private int maxHealthPacks = 3; // 최대 3개까지만 존재
    
    [Header("Spawn Area")]
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    [SerializeField] private bool useRandomPosition = true;
    
    [Header("Auto Start")]
    [SerializeField] private bool autoStart = true;
    [SerializeField] private float initialDelay = 5f; // 게임 시작 5초 후부터
    
    private float spawnTimer = 0f;
    private int currentHealthPackCount = 0;
    private bool isSpawning = false;
    
    private void Start()
    {
        if (autoStart)
        {
            Invoke(nameof(StartSpawning), initialDelay);
        }
    }
    
    private void Update()
    {
        if (!isSpawning) return;
        
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval)
        {
            SpawnHealthPack();
            spawnTimer = 0f;
        }
    }
    
    /// <summary>
    /// 스폰 시작
    /// </summary>
    public void StartSpawning()
    {
        isSpawning = true;
        Debug.Log("힐팩 스폰 시작!");
    }
    
    /// <summary>
    /// 스폰 중지
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log("힐팩 스폰 중지!");
    }
    
    /// <summary>
    /// 힐팩 스폰
    /// </summary>
    private void SpawnHealthPack()
    {
        if (healthPackPrefab == null)
        {
            Debug.LogWarning("HealthPack 프리팹이 할당되지 않았습니다!");
            return;
        }
        
        // 최대 개수 체크
        if (currentHealthPackCount >= maxHealthPacks)
        {
            Debug.Log("힐팩이 이미 최대 개수만큼 존재합니다.");
            return;
        }
        
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject healthPack = Instantiate(healthPackPrefab, spawnPosition, Quaternion.identity);
        
        // 힐팩이 제거될 때 카운트 감소
        HealthPackTracker tracker = healthPack.AddComponent<HealthPackTracker>();
        tracker.OnDestroyed += () => currentHealthPackCount--;
        
        currentHealthPackCount++;
        Debug.Log($"힐팩 스폰! (현재: {currentHealthPackCount}/{maxHealthPacks})");
    }
    
    /// <summary>
    /// 스폰 위치 계산
    /// </summary>
    private Vector3 GetSpawnPosition()
    {
        if (useRandomPosition)
        {
            // 랜덤 위치
            float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);
            
            return transform.position + new Vector3(randomX, spawnAreaSize.y, randomZ);
        }
        else
        {
            // 고정 위치
            return transform.position;
        }
    }
    
    /// <summary>
    /// 즉시 힐팩 스폰 (수동 호출용)
    /// </summary>
    public void SpawnNow()
    {
        SpawnHealthPack();
    }
    
    /// <summary>
    /// Scene 뷰에서 스폰 영역 표시
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}

/// <summary>
/// 힐팩이 파괴될 때 이벤트 발생 (내부 사용)
/// </summary>
public class HealthPackTracker : MonoBehaviour
{
    public event System.Action OnDestroyed;
    
    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}

