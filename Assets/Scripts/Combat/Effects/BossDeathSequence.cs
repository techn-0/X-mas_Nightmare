using System.Collections;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 보스 사망 시퀀스
    /// 여러 부위에서 순차적으로 폭발 이펙트를 재생
    /// </summary>
    public class BossDeathSequence : MonoBehaviour
    {
        [Header("Explosion Settings")]
        [SerializeField] private GameObject explosionEffectPrefab; // 폭발 이펙트 프리팹
        [SerializeField] private Transform[] explosionPoints; // 폭발 위치들
        [SerializeField] private float delayBetweenExplosions = 0.3f; // 폭발 간 딜레이
        [SerializeField] private float explosionEffectDuration = 3f; // 이펙트 지속 시간
        
        [Header("Final Destruction")]
        [SerializeField] private float destroyDelay = 2f; // 마지막 폭발 후 오브젝트 파괴 시간
        
        private bool isPlayingSequence;
        
        /// <summary>
        /// 사망 시퀀스 재생
        /// </summary>
        public void PlayDeathSequence()
        {
            if (isPlayingSequence)
                return;
            
            StartCoroutine(DeathSequenceCoroutine());
        }
        
        /// <summary>
        /// 사망 시퀀스 코루틴
        /// </summary>
        private IEnumerator DeathSequenceCoroutine()
        {
            isPlayingSequence = true;
            
            Debug.Log("[Boss Death] 보스 사망 시퀀스 시작!");
            
            // 폭발 위치가 설정되지 않았으면 기본 위치 사용
            if (explosionPoints == null || explosionPoints.Length == 0)
            {
                Debug.LogWarning("[Boss Death] 폭발 위치가 설정되지 않음! 기본 위치 사용");
                
                if (explosionEffectPrefab != null)
                {
                    GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, explosionEffectDuration);
                }
                
                yield return new WaitForSeconds(destroyDelay);
                Destroy(gameObject);
                yield break;
            }
            
            // 각 폭발 위치에서 순차적으로 폭발
            foreach (Transform explosionPoint in explosionPoints)
            {
                if (explosionPoint != null)
                {
                    CreateExplosion(explosionPoint.position);
                    yield return new WaitForSeconds(delayBetweenExplosions);
                }
            }
            
            Debug.Log("[Boss Death] 보스 사망 시퀀스 완료!");
            
            // 마지막 폭발 후 오브젝트 파괴
            yield return new WaitForSeconds(destroyDelay);
            Destroy(gameObject);
        }
        
        /// <summary>
        /// 폭발 생성
        /// </summary>
        private void CreateExplosion(Vector3 position)
        {
            if (explosionEffectPrefab == null)
            {
                Debug.LogWarning("[Boss Death] 폭발 이펙트 프리팹이 설정되지 않았습니다!");
                return;
            }
            
            // 폭발 이펙트 생성
            GameObject explosion = Instantiate(explosionEffectPrefab, position, Quaternion.identity);
            
            // 이펙트 자동 파괴
            Destroy(explosion, explosionEffectDuration);
            
            Debug.Log($"[Boss Death] 폭발 생성 at {position}");
        }
        
        /// <summary>
        /// 디버그용 Gizmos
        /// </summary>
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (explosionPoints == null)
                return;
            
            Gizmos.color = Color.red;
            
            // 폭발 위치들 표시
            for (int i = 0; i < explosionPoints.Length; i++)
            {
                Transform point = explosionPoints[i];
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.3f);
                    
                    // 번호 표시용 (에디터에서 순서 확인)
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.red;
                    UnityEditor.Handles.Label(point.position, (i + 1).ToString(), style);
                }
            }
            
            // 폭발 순서 연결선
            if (explosionPoints.Length > 1)
            {
                Gizmos.color = new Color(1f, 0.5f, 0f);
                for (int i = 0; i < explosionPoints.Length - 1; i++)
                {
                    if (explosionPoints[i] != null && explosionPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(explosionPoints[i].position, explosionPoints[i + 1].position);
                    }
                }
            }
#endif
        }
    }
}

