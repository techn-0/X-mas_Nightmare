using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 눈사람 몬스터를 스폰하는 스포너 시스템
/// </summary>
public class SnowmanSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableMonster
    {
        [Tooltip("스폰할 몬스터 프리팹")]
        public GameObject prefab;
        
        [Tooltip("스폰 확률 (높을수록 자주 나옴)")]
        [Range(1, 100)]
        public int spawnWeight = 50;
    }

    [Header("스폰할 몬스터 설정")]
    [Tooltip("스폰 가능한 몬스터 리스트")]
    public List<SpawnableMonster> monsters = new List<SpawnableMonster>();

    [Header("스폰 설정")]
    [Tooltip("스폰 간격 (초)")]
    [Range(0.5f, 30f)]
    public float spawnInterval = 3f;

    [Tooltip("스폰 포인트 개수")]
    [Range(1, 10)]
    public int spawnPointCount = 4;

    [Tooltip("스폰 범위 반경")]
    [Range(5f, 50f)]
    public float spawnRadius = 20f;

    [Header("제한 설정")]
    [Tooltip("최대 동시 스폰 수 (0 = 무제한)")]
    [Range(0, 100)]
    public int maxActiveMonsters = 20;

    [Tooltip("게임 시작시 자동 스폰")]
    public bool autoStart = true;

    [Header("디버그")]
    [Tooltip("스폰 포인트 기즈모 표시")]
    public bool showSpawnPoints = true;

    private List<GameObject> activeMonsters = new List<GameObject>();
    private bool isSpawning;
    private Vector3[] spawnPoints;

    void Start()
    {
        GenerateSpawnPoints();
        
        if (autoStart)
        {
            StartSpawning();
        }
    }

    /// <summary>
    /// 스폰 포인트 생성
    /// </summary>
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

    /// <summary>
    /// 스폰 시작
    /// </summary>
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    /// <summary>
    /// 스폰 중지
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    /// <summary>
    /// 스폰 루틴
    /// </summary>
    IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);

            // 최대 스폰 수 체크
            CleanupDeadMonsters();
            if (maxActiveMonsters > 0 && activeMonsters.Count >= maxActiveMonsters)
            {
                continue;
            }

            SpawnMonster();
        }
    }

    /// <summary>
    /// 몬스터 스폰
    /// </summary>
    void SpawnMonster()
    {
        if (monsters.Count == 0)
        {
            Debug.LogWarning("스폰할 몬스터가 설정되지 않았습니다!");
            return;
        }

        // 확률 기반 몬스터 선택
        GameObject selectedPrefab = SelectRandomMonster();
        if (selectedPrefab == null)
        {
            Debug.LogWarning("유효한 몬스터 프리팹이 없습니다!");
            return;
        }

        // 랜덤 스폰 포인트 선택
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 약간의 랜덤 오프셋 추가
        spawnPosition += new Vector3(
            Random.Range(-2f, 2f),
            0,
            Random.Range(-2f, 2f)
        );

        // 몬스터 생성
        GameObject monster = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        activeMonsters.Add(monster);
    }

    /// <summary>
    /// 확률 기반 몬스터 선택
    /// </summary>
    GameObject SelectRandomMonster()
    {
        // 전체 가중치 계산
        int totalWeight = 0;
        foreach (var monster in monsters)
        {
            if (monster.prefab != null)
            {
                totalWeight += monster.spawnWeight;
            }
        }

        if (totalWeight == 0) return null;

        // 랜덤 값 선택
        int randomValue = Random.Range(0, totalWeight);
        int currentWeight = 0;

        // 가중치 기반 선택
        foreach (var monster in monsters)
        {
            if (monster.prefab != null)
            {
                currentWeight += monster.spawnWeight;
                if (randomValue < currentWeight)
                {
                    return monster.prefab;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 죽은 몬스터 정리
    /// </summary>
    void CleanupDeadMonsters()
    {
        activeMonsters.RemoveAll(monster => monster == null);
    }

    /// <summary>
    /// 모든 몬스터 제거
    /// </summary>
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

    /// <summary>
    /// 스폰 포인트 재생성
    /// </summary>
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            GenerateSpawnPoints();
        }
    }

    /// <summary>
    /// 기즈모 그리기
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showSpawnPoints) return;

        // 스폰 범위 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        // 스폰 포인트 생성 (에디터 전용)
        if (spawnPoints == null || spawnPoints.Length != spawnPointCount)
        {
            Vector3[] tempPoints = new Vector3[spawnPointCount];
            for (int i = 0; i < spawnPointCount; i++)
            {
                float angle = i * (360f / spawnPointCount);
                float radian = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(
                    Mathf.Cos(radian) * spawnRadius,
                    0,
                    Mathf.Sin(radian) * spawnRadius
                );
                tempPoints[i] = transform.position + offset;
            }
            
            // 스폰 포인트 표시
            Gizmos.color = Color.green;
            foreach (var point in tempPoints)
            {
                Gizmos.DrawSphere(point, 0.5f);
                Gizmos.DrawLine(transform.position, point);
            }
        }
        else
        {
            // 런타임 스폰 포인트 표시
            Gizmos.color = Color.green;
            foreach (var point in spawnPoints)
            {
                Gizmos.DrawSphere(point, 0.5f);
                Gizmos.DrawLine(transform.position, point);
            }
        }
    }
}

