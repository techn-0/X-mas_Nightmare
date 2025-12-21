using UnityEngine;
using Game.Combat;

/// <summary>
/// 힐팩 아이템 - 플레이어가 닿으면 체력을 회복하고 사라집니다
/// </summary>
public class HealthPack : MonoBehaviour
{
    [Header("Heal Settings")]
    [SerializeField] private float healAmount = 30f;
    [Tooltip("true면 회복량이 절대값, false면 최대 체력의 비율(%)")]
    [SerializeField] private bool useAbsoluteValue = true;
    
    [Header("Effects")]
    [SerializeField] private GameObject pickupEffectPrefab;
    [SerializeField] private AudioClip pickupSound;
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 1f;
    
    [Header("Rotation (Optional)")]
    [SerializeField] private bool rotateItem = true;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    
    [Header("Floating (Optional)")]
    [SerializeField] private bool floatItem = true;
    [SerializeField] private float floatAmplitude = 0.3f;
    [SerializeField] private float floatSpeed = 2f;
    
    private Vector3 startPosition;
    private AudioSource audioSource;
    
    private void Start()
    {
        // 시작 위치 저장 (떠다니는 효과용)
        startPosition = transform.position;
        
        // AudioSource 컴포넌트 추가
        if (pickupSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0.5f; // 3D 사운드
        }
    }
    
    private void Update()
    {
        // 회전 효과
        if (rotateItem)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
        
        // 떠다니는 효과
        if (floatItem)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 확인
        if (other.CompareTag("Player"))
        {
            // PlayerHealth 컴포넌트 찾기
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                // 이미 최대 체력이면 아이템을 먹지 않음
                if (playerHealth.CurrentHP >= playerHealth.MaxHP)
                {
                    Debug.Log("체력이 이미 최대입니다. 힐팩을 사용할 수 없습니다.");
                    return;
                }
                
                // 회복량 계산
                float actualHealAmount = healAmount;
                if (!useAbsoluteValue)
                {
                    // 비율로 계산 (예: 50%면 최대 체력의 50%)
                    actualHealAmount = playerHealth.MaxHP * (healAmount / 100f);
                }
                
                // 체력 회복
                playerHealth.Heal(actualHealAmount);
                
                Debug.Log($"힐팩 획득! {actualHealAmount} 체력 회복 ({playerHealth.CurrentHP}/{playerHealth.MaxHP})");
                
                // 이펙트 생성
                if (pickupEffectPrefab != null)
                {
                    GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, 2f); // 2초 후 이펙트 제거
                }
                
                // 사운드 재생
                if (pickupSound != null && audioSource != null)
                {
                    // 오디오가 재생되도록 잠시 오브젝트 유지
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
                }
                
                // 오브젝트 제거
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("플레이어에게 PlayerHealth 컴포넌트가 없습니다!");
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        // Scene 뷰에서 아이템 위치 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}


