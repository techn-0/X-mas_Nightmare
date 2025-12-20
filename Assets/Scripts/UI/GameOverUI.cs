using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    /// <summary>
    /// 게임 종료 UI 관리
    /// YOU DIE (플레이어 사망) / YOU WIN (보스 사망) 화면 표시
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject gameOverPanel; // 게임오버 패널
        [SerializeField] private TextMeshProUGUI gameOverText; // 게임오버 텍스트

        [Header("Text Settings")]
        [SerializeField] private string playerDeathText = "YOU DIE";
        [SerializeField] private string bossDeathText = "YOU WIN";
        [SerializeField] private Color playerDeathColor = Color.red;
        [SerializeField] private Color bossDeathColor = new Color(1f, 0.84f, 0f); // 금색

        [Header("Animation")]
        [SerializeField] private float fadeInDuration = 1f;
        [SerializeField] private bool useAnimation = true;

        [Header("Audio")]
        [SerializeField] private AudioClip victorySound; // 승리 사운드
        [SerializeField] [Range(0f, 1f)] private float victorySoundVolume = 1f; // 승리 사운드 볼륨

        private CanvasGroup canvasGroup;
        private AudioSource audioSource;

        private void Awake()
        {
            // CanvasGroup 컴포넌트 가져오기 또는 생성
            if (gameOverPanel != null)
            {
                canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
                }
            }

            // AudioSource 초기화
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }

            // 초기에는 패널 숨김
            HidePanel();

            // GameManager 이벤트 구독
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPlayerDeath.AddListener(ShowPlayerDeath);
                GameManager.Instance.OnBossDeath.AddListener(ShowBossVictory);
            }
        }

        /// <summary>
        /// 플레이어 사망 화면 표시
        /// </summary>
        private void ShowPlayerDeath()
        {
            ShowGameOver(playerDeathText, playerDeathColor, null);
        }

        /// <summary>
        /// 승리 화면 표시
        /// </summary>
        private void ShowBossVictory()
        {
            Debug.Log("[GameOverUI] ShowBossVictory 호출됨!");
            
            // 승리 사운드를 ShowGameOver에 전달
            ShowGameOver(bossDeathText, bossDeathColor, victorySound);
        }

        /// <summary>
        /// 게임오버 화면 표시
        /// </summary>
        private void ShowGameOver(string text, Color color, AudioClip soundToPlay)
        {
            if (gameOverPanel == null || gameOverText == null)
            {
                Debug.LogWarning("[GameOverUI] UI 레퍼런스가 설정되지 않았습니다!");
                return;
            }

            // 텍스트 설정
            gameOverText.text = text;
            gameOverText.color = color;

            // 패널 활성화 (AudioSource 활성화를 위해 필수!)
            gameOverPanel.SetActive(true);

            // 패널 활성화 후 사운드 재생
            if (soundToPlay != null && audioSource != null)
            {
                audioSource.PlayOneShot(soundToPlay, victorySoundVolume);
                Debug.Log($"[GameOverUI] 사운드 재생! (볼륨: {victorySoundVolume})");
            }
            else if (soundToPlay != null && audioSource == null)
            {
                Debug.LogError("[GameOverUI] AudioSource가 null입니다!");
            }

            // 애니메이션 재생
            if (useAnimation && canvasGroup != null)
            {
                StartCoroutine(FadeIn());
            }
            else if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }

            Debug.Log($"[GameOverUI] 게임오버 화면 표시: {text}");
        }

        /// <summary>
        /// 패널 숨기기
        /// </summary>
        private void HidePanel()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }

        /// <summary>
        /// 페이드 인 애니메이션
        /// </summary>
        private System.Collections.IEnumerator FadeIn()
        {
            float elapsed = 0f;
            canvasGroup.alpha = 0f;

            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// 재시작 버튼 (UI 버튼에서 호출)
        /// </summary>
        public void OnRestartButton()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartGame();
            }
        }

        /// <summary>
        /// 종료 버튼 (UI 버튼에서 호출)
        /// </summary>
        public void OnQuitButton()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.QuitGame();
            }
        }

        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnPlayerDeath.RemoveListener(ShowPlayerDeath);
                GameManager.Instance.OnBossDeath.RemoveListener(ShowBossVictory);
            }
        }
    }
}

