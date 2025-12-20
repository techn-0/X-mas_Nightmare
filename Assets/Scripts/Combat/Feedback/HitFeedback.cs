using System.Collections;
using UnityEngine;

namespace Game.Combat
{
    /// <summary>
    /// 피격 시 시각적/청각적 피드백을 처리하는 클래스
    /// </summary>
    public class HitFeedback : MonoBehaviour
    {
        [Header("Visual Feedback")]
        [SerializeField] private float hitFlashDuration = 0.1f;
        [SerializeField] private Color hitFlashColor = Color.red;
        [SerializeField] private GameObject hitEffectPrefab;
        
        [Header("Audio Feedback")]
        [SerializeField] private AudioClip hitSound;
        
        private Renderer[] renderers;
        private AudioSource audioSource;
        private Coroutine flashCoroutine;
        
        private void Awake()
        {
            // 모든 렌더러 컴포넌트 찾기
            renderers = GetComponentsInChildren<Renderer>();
            
            // AudioSource 가져오기 또는 생성
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null && hitSound != null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }
        
        /// <summary>
        /// 피격 피드백 재생
        /// </summary>
        /// <param name="hitPoint">피격 위치</param>
        public void PlayHitFeedback(Vector3 hitPoint)
        {
            // 색상 플래시
            if (renderers.Length > 0 && flashCoroutine == null)
            {
                flashCoroutine = StartCoroutine(HitFlashCoroutine());
            }
            
            // 피격 이펙트 생성
            if (hitEffectPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
                Destroy(hitEffect, 2f); // 2초 후 자동 제거
            }
            
            // 피격 사운드 재생
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }
        
        /// <summary>
        /// 피격 색상 플래시 코루틴
        /// </summary>
        private IEnumerator HitFlashCoroutine()
        {
            // 원본 색상 저장
            Color[] originalColors = new Color[renderers.Length];
            Material[][] materials = new Material[renderers.Length][];
            
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    materials[i] = renderers[i].materials;
                    if (materials[i].Length > 0)
                    {
                        originalColors[i] = materials[i][0].color;
                    }
                }
            }
            
            // 플래시 색상으로 변경
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && materials[i] != null)
                {
                    foreach (Material mat in materials[i])
                    {
                        if (mat != null)
                        {
                            mat.color = hitFlashColor;
                        }
                    }
                }
            }
            
            yield return new WaitForSeconds(hitFlashDuration);
            
            // 원본 색상으로 복원
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && materials[i] != null)
                {
                    foreach (Material mat in materials[i])
                    {
                        if (mat != null)
                        {
                            mat.color = originalColors[i];
                        }
                    }
                }
            }
            
            flashCoroutine = null;
        }
        
        private void OnDestroy()
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
            }
        }
    }
}

