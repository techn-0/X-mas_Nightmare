using UnityEngine;

/// <summary>
/// 스포너 사용 예제 모음
/// </summary>
public class SpawnerExamples : MonoBehaviour
{
    // 예제 1: 기본 스포너 제어
    void Example1_BasicControl()
    {
        SnowmanSpawner spawner = FindObjectOfType<SnowmanSpawner>();

        // 스폰 시작
        spawner.StartSpawning();

        // 3초 후 중지
        Invoke(nameof(StopSpawner), 3f);
    }

    void StopSpawner()
    {
        FindObjectOfType<SnowmanSpawner>().StopSpawning();
    }

    // 예제 2: 플레이어가 특정 지역 진입시 스폰
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SnowmanSpawner spawner = GetComponent<SnowmanSpawner>();
            spawner.StartSpawning();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SnowmanSpawner spawner = GetComponent<SnowmanSpawner>();
            spawner.StopSpawning();
        }
    }

    // 예제 3: 보스 등장시 잡몹 제거
    void SpawnBoss()
    {
        SnowmanSpawner spawner = FindObjectOfType<SnowmanSpawner>();
        
        // 기존 몬스터 모두 제거
        spawner.ClearAllMonsters();
        
        // 스폰 중지
        spawner.StopSpawning();
        
        // 보스 소환
        // Instantiate(bossPrefab, ...);
    }

    // 예제 4: 웨이브 시스템 이벤트 활용
    void Start()
    {
        AdvancedSnowmanSpawner advancedSpawner = FindObjectOfType<AdvancedSnowmanSpawner>();
        
        if (advancedSpawner != null)
        {
            // 웨이브 시작시
            advancedSpawner.OnWaveStart += (wave) => {
                Debug.Log($"웨이브 {wave} 준비!");
                // UI 표시, 사운드 재생 등
            };

            // 웨이브 완료시
            advancedSpawner.OnWaveComplete += (wave) => {
                Debug.Log($"웨이브 {wave} 클리어!");
                // 보상 지급
                GiveReward(wave * 100); // 웨이브당 100골드
            };

            // 모든 웨이브 완료시
            advancedSpawner.OnAllWavesComplete += () => {
                Debug.Log("게임 클리어!");
                // 승리 화면 표시
            };
        }
    }

    void GiveReward(int amount)
    {
        Debug.Log($"보상 획득: {amount} 골드");
        // 실제 보상 지급 코드
    }

    // 예제 5: 난이도별 스포너 설정
    void SetDifficulty(string difficulty)
    {
        SnowmanSpawner spawner = FindFirstObjectByType<SnowmanSpawner>();

        switch (difficulty)
        {
            case "Easy":
                spawner.spawnInterval = 5f;
                spawner.maxActiveMonsters = 10;
                break;
            
            case "Normal":
                spawner.spawnInterval = 3f;
                spawner.maxActiveMonsters = 20;
                break;
            
            case "Hard":
                spawner.spawnInterval = 1.5f;
                spawner.maxActiveMonsters = 30;
                break;
        }

        spawner.StartSpawning();
    }

    // 예제 6: 시간대별 다른 스폰 설정
    void TimeBasedSpawning()
    {
        float gameTime = Time.timeSinceLevelLoad;
        SnowmanSpawner spawner = FindObjectOfType<SnowmanSpawner>();

        // 0-60초: 느린 스폰
        if (gameTime < 60f)
        {
            spawner.spawnInterval = 4f;
        }
        // 60-120초: 중간 스폰
        else if (gameTime < 120f)
        {
            spawner.spawnInterval = 2f;
        }
        // 120초 이후: 빠른 스폰
        else
        {
            spawner.spawnInterval = 1f;
        }
    }

    // 예제 7: 조건부 스폰 (몬스터가 모두 죽으면 다시 스폰)
    void ConditionalSpawning()
    {
        AdvancedSnowmanSpawner spawner = FindFirstObjectByType<AdvancedSnowmanSpawner>();

        if (spawner.GetActiveMonsterCount() == 0)
        {
            Debug.Log("몬스터가 없습니다. 다시 스폰합니다.");
            spawner.StartSpawning();
        }
    }
}

