using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 게임 전체 상태를 관리하는 매니저
/// 싱글톤 패턴으로 구현
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    [Header("Game State")]
    [SerializeField] private bool isGameOver;

    [Header("Events")]
    public UnityEvent OnPlayerDeath = new UnityEvent();
    public UnityEvent OnBossDeath = new UnityEvent();
    public UnityEvent OnGameOver = new UnityEvent();

    public bool IsGameOver => isGameOver;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 플레이어 사망 처리
        /// </summary>
        public void PlayerDied()
        {
            if (isGameOver)
                return;

            Debug.Log("[GameManager] 플레이어 사망!");
            isGameOver = true;

            // 플레이어 사망 이벤트 발생
            OnPlayerDeath?.Invoke();

            // 모든 적 AI 정지
            StopAllEnemies();

            // 게임오버 이벤트 발생
            OnGameOver?.Invoke();
        }

        /// <summary>
        /// 보스 사망 처리
        /// </summary>
        public void BossDied()
        {
            if (isGameOver)
                return;

            Debug.Log("[GameManager] 보스 사망!");
            isGameOver = true;

            // 보스 사망 이벤트 발생
            OnBossDeath?.Invoke();

            // 모든 적 AI 정지
            StopAllEnemies();

            // 게임오버 이벤트 발생
            OnGameOver?.Invoke();
        }

    /// <summary>
    /// 모든 적 AI 동작 정지
    /// </summary>
    private void StopAllEnemies()
    {
        Debug.Log("[GameManager] 모든 적 AI 정지");

        // 모든 눈사람 AI 정지
        var snowmanAIs = Object.FindObjectsByType<Combat.Entities.SnowmanAI>(FindObjectsSortMode.None);
        foreach (var ai in snowmanAIs)
        {
            ai.enabled = false;
        }

        // 폭발 눈사람 AI 정지
        var explosiveAIs = Object.FindObjectsByType<Combat.Entities.ExplosiveSnowmanAI>(FindObjectsSortMode.None);
        foreach (var ai in explosiveAIs)
        {
            ai.enabled = false;
        }

        // 보스 AI 정지
        var bossAIs = Object.FindObjectsByType<Combat.Entities.BossSantaAI>(FindObjectsSortMode.None);
        foreach (var ai in bossAIs)
        {
            ai.enabled = false;
        }

        Debug.Log($"[GameManager] 정지된 AI: 눈사람 {snowmanAIs.Length}개, 폭발 눈사람 {explosiveAIs.Length}개, 보스 {bossAIs.Length}개");
    }

        /// <summary>
        /// 게임 재시작
        /// </summary>
        public void RestartGame()
        {
            isGameOver = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// 게임 종료
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }


