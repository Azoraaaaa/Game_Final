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
    [SerializeField] private PlayerHealth playerHealth;
    
    // 单例模式
    public static GameManager Instance { get; private set; }
    
    // 公共属性，供其他脚本访问
    public GameObject player;
    public bool isNearTeleporter = false;
    public HashSet<string> discoveredLocations = new HashSet<string>();
    
    // 私有变量
    private bool isGameOver = false;
    private bool isVictory = false;
    
    void Awake()
    {
        // 单例模式设置
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
        
        // 隐藏UI面板
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        
        // 查找Boss和玩家
        if (bossController == null)
        {
            bossController = FindFirstObjectByType<BossController>();
        }
        
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
        
        // 初始化Boss血条
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
            {
                bossHealthText.text = $"Boss HP: {currentHP:F0}/{maxHP:F0}";
            }
        }
    }
    
    private void CheckGameState()
    {
        // 检查玩家是否死亡
        if (playerHealth != null && playerHealth.IsDead() && !isGameOver)
        {
            GameOver();
        }
        
        // 检查Boss是否死亡
        if (bossController != null && !isVictory && bossController.IsDead)
        {
            Victory();
        }
    }
    
    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        
        // 显示游戏结束UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // 暂停游戏
        Time.timeScale = 0f;
        
        // TODO: 播放游戏结束音效
        // AudioManager.Instance.PlaySound("game_over");
    }
    
    public void Victory()
    {
        if (isVictory) return;
        
        isVictory = true;
        
        // 显示胜利UI
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        // 暂停游戏
        Time.timeScale = 0f;
        
        // TODO: 播放胜利音效
        // AudioManager.Instance.PlaySound("victory");
    }
    
    public void RestartGame()
    {
        // 恢复时间缩放
        Time.timeScale = 1f;
        
        // 重新加载当前场景
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
