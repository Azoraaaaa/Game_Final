using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI引用")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Text bossHealthText;
    [SerializeField] private Slider bossHealthBar;

    [Header("游戏设置")]
    [SerializeField] private BossController bossController;
    [SerializeField] private PlayerHealthSystem playerHealth;

    [Header("场景设置")]
    [SerializeField] private string[] sceneSequence; // 场景名称列表
    private int currentSceneIndex = 0;

    // 单例模式
    public static GameManager Instance { get; private set; }

    // 公共属性
    public GameObject player;
    public bool isNearTeleporter = false;
    public HashSet<string> discoveredLocations = new HashSet<string>();

    // 私有变量
    private bool isGameOver = false;
    private bool isVictory = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGame();

        // 初始化当前场景索引
        string currentSceneName = SceneManager.GetActiveScene().name;
        for (int i = 0; i < sceneSequence.Length; i++)
        {
            if (sceneSequence[i] == currentSceneName)
            {
                currentSceneIndex = i;
                break;
            }
        }
    }

    void Update()
    {
        UpdateBossHealthUI();
        CheckGameState();
    }

    private void InitializeGame()
    {
        isGameOver = false;
        isVictory = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        if (bossController == null)
            bossController = FindFirstObjectByType<BossController>();

        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealthSystem>();

        UpdateBossHealthUI();
    }

    private void UpdateBossHealthUI()
    {
        if (bossController != null && bossHealthBar != null)
        {
            float currentHP = bossController.CurrentHP;
            float maxHP = bossController.MaxHP;

            bossHealthBar.value = currentHP / maxHP;

            if (bossHealthText != null)
                bossHealthText.text = $"Boss HP: {currentHP:F0}/{maxHP:F0}";
        }
    }

    private void CheckGameState()
    {
        if (playerHealth != null && playerHealth.IsDead && !isGameOver)
        {
            GameOver();
        }

        if (bossController != null && !isVictory && bossController.IsDead)
        {
            Victory();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        // AudioManager.Instance.PlaySound("game_over");
    }

    public void Victory()
    {
        if (isVictory) return;

        isVictory = true;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f;

        // AudioManager.Instance.PlaySound("victory");

        // 延迟3秒加载下一关
        Invoke("LoadNextScene", 3f);
    }

    public void LoadNextScene()
    {
        if (currentSceneIndex + 1 < sceneSequence.Length)
        {
            string nextSceneName = sceneSequence[currentSceneIndex + 1];
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("已到达最后一个场景，游戏结束或返回主菜单。");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsVictory()
    {
        return isVictory;
    }
}
