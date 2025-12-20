using UnityEngine;

/// <summary>
/// 보스 스폰 타이머 트리거
/// 플레이어가 특정 영역에 들어왔을 때 보스 스폰 타이머를 시작합니다.
/// </summary>
[RequireComponent(typeof(Collider))]
public class BossSpawnTrigger : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private BossSpawnTimer bossSpawnTimer;
    
    [Header("트리거 설정")]
    [SerializeField] private string playerTag = "Player"; // 플레이어 태그
    [SerializeField] private bool triggerOnce = true; // 한 번만 트리거
    [SerializeField] private bool showGizmo = true; // Scene 뷰에 트리거 영역 표시
    
    [Header("디버그")]
    [SerializeField] private bool debugMode; // 디버그 메시지 표시
    
    private bool hasTriggered;
    private Collider triggerCollider;
    
    private void Awake()
    {
        // Collider를 Trigger로 설정
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
        
        // BossSpawnTimer 자동 찾기
        if (bossSpawnTimer == null)
        {
            bossSpawnTimer = FindFirstObjectByType<BossSpawnTimer>();
            
            if (bossSpawnTimer == null && debugMode)
            {
                Debug.LogWarning("[BossSpawnTrigger] BossSpawnTimer를 찾을 수 없습니다!");
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 이미 트리거되었으면 무시
        if (triggerOnce && hasTriggered)
            return;
        
        // 플레이어가 아니면 무시
        if (!other.CompareTag(playerTag))
            return;
        
        // 보스 타이머가 설정되지 않았으면 무시
        if (bossSpawnTimer == null)
        {
            if (debugMode)
                Debug.LogError("[BossSpawnTrigger] BossSpawnTimer가 설정되지 않았습니다!");
            return;
        }
        
        // 타이머 시작
        if (debugMode)
            Debug.Log($"[BossSpawnTrigger] 플레이어가 트리거 영역에 진입! 보스 타이머 시작!");
        
        bossSpawnTimer.StartTimer();
        hasTriggered = true;
        
        // 한 번만 트리거하는 경우 트리거 비활성화
        if (triggerOnce)
        {
            if (triggerCollider != null)
                triggerCollider.enabled = false;
        }
    }
    
    /// <summary>
    /// 트리거 리셋 (다시 사용 가능하게)
    /// </summary>
    public void ResetTrigger()
    {
        hasTriggered = false;
        
        if (triggerCollider != null)
            triggerCollider.enabled = true;
        
        if (debugMode)
            Debug.Log("[BossSpawnTrigger] 트리거가 리셋되었습니다.");
    }
    
    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;
        
        // 트리거 영역 시각화
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = hasTriggered ? new Color(1f, 0f, 0f, 0.3f) : new Color(1f, 1f, 0f, 0.3f);
            
            if (col is BoxCollider boxCollider)
            {
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);
                Gizmos.matrix = oldMatrix;
            }
            else if (col is SphereCollider sphereCollider)
            {
                Gizmos.DrawSphere(transform.position + sphereCollider.center, sphereCollider.radius);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!showGizmo)
            return;
        
        // 선택했을 때 외곽선 표시
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.yellow;
            
            if (col is BoxCollider boxCollider)
            {
                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
                Gizmos.matrix = oldMatrix;
            }
            else if (col is SphereCollider sphereCollider)
            {
                Gizmos.DrawWireSphere(transform.position + sphereCollider.center, sphereCollider.radius);
            }
        }
    }
}

