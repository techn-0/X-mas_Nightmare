using UnityEngine;

/// <summary>
/// 힐팩 사용 예제 - 적이 죽을 때 힐팩 드롭
/// EnemyHealth 스크립트에서 사용할 수 있습니다
/// </summary>
public class HealthPackDropper : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private GameObject healthPackPrefab;
    [Tooltip("0.0 ~ 1.0 사이의 값 (0.3 = 30% 확률)")]
    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 0.3f;
    [SerializeField] private Vector3 dropOffset = Vector3.up * 0.5f;
    
    /// <summary>
    /// 적이 죽을 때 호출되는 메서드
    /// EnemyHealth의 OnDeath 이벤트에 연결하거나 직접 호출
    /// </summary>
    public void TryDropHealthPack()
    {
        if (healthPackPrefab == null)
        {
            Debug.LogWarning("HealthPack 프리팹이 할당되지 않았습니다!");
            return;
        }
        
        // 확률 체크
        if (Random.value <= dropChance)
        {
            Vector3 dropPosition = transform.position + dropOffset;
            Instantiate(healthPackPrefab, dropPosition, Quaternion.identity);
            Debug.Log($"힐팩 드롭! (위치: {dropPosition})");
        }
    }
    
    /// <summary>
    /// 힐팩을 무조건 드롭합니다 (보스나 특별한 적용)
    /// </summary>
    public void ForceDropHealthPack()
    {
        if (healthPackPrefab == null)
        {
            Debug.LogWarning("HealthPack 프리팹이 할당되지 않았습니다!");
            return;
        }
        
        Vector3 dropPosition = transform.position + dropOffset;
        Instantiate(healthPackPrefab, dropPosition, Quaternion.identity);
        Debug.Log($"힐팩 강제 드롭! (위치: {dropPosition})");
    }
    
    /// <summary>
    /// 여러 개의 힐팩을 드롭합니다
    /// </summary>
    public void DropMultipleHealthPacks(int count)
    {
        if (healthPackPrefab == null)
        {
            Debug.LogWarning("HealthPack 프리팹이 할당되지 않았습니다!");
            return;
        }
        
        for (int i = 0; i < count; i++)
        {
            // 랜덤한 위치에 드롭
            Vector3 randomOffset = new Vector3(
                Random.Range(-1f, 1f),
                0.5f,
                Random.Range(-1f, 1f)
            );
            
            Vector3 dropPosition = transform.position + randomOffset;
            Instantiate(healthPackPrefab, dropPosition, Quaternion.identity);
        }
        
        Debug.Log($"{count}개의 힐팩 드롭!");
    }
}

