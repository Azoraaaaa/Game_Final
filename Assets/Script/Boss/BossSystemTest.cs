using UnityEngine;

public class BossSystemTest : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private BossController bossController;
    [SerializeField] private PlayerHealthSystem playerHealth;
    [SerializeField] private bool enableDebugLogs = true;
    
    void Start()
    {
        // 自动查找组件
        if (bossController == null)
        {
            bossController = FindFirstObjectByType<BossController>();
        }
        
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealthSystem>();
        }
        
        // 验证组件
        ValidateComponents();
    }
    
    void Update()
    {
        // 测试按键
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestBossDamage();
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestPlayerDamage();
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            TestBossPhase();
        }
    }
    
    private void ValidateComponents()
    {
        if (enableDebugLogs)
        {
            if (bossController != null)
            {
                Debug.Log("✅ BossController 找到并正常工作");
                Debug.Log($"Boss HP: {bossController.CurrentHP}/{bossController.MaxHP}");
            }
            else
            {
                Debug.LogError("❌ BossController 未找到");
            }
            
            if (playerHealth != null)
            {
                Debug.Log("✅ PlayerHealthSystem 找到并正常工作");
                Debug.Log($"Player HP: {playerHealth.HealthPercentage * 100:F0}%");
            }
            else
            {
                Debug.LogError("❌ PlayerHealthSystem 未找到");
            }
            
            if (GameManager.Instance != null)
            {
                Debug.Log("✅ GameManager 找到并正常工作");
            }
            else
            {
                Debug.LogError("❌ GameManager 未找到");
            }
        }
    }
    
    private void TestBossDamage()
    {
        if (bossController != null)
        {
            bossController.TakeDamage(100f);
            Debug.Log($"对Boss造成100点伤害，当前HP: {bossController.CurrentHP}");
        }
    }
    
    private void TestPlayerDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(20f);
            Debug.Log($"对玩家造成20点伤害，当前HP百分比: {playerHealth.HealthPercentage * 100:F0}%");
        }
    }
    
    private void TestBossPhase()
    {
        if (bossController != null)
        {
            float healthPercentage = (bossController.CurrentHP / bossController.MaxHP) * 100f;
            Debug.Log($"Boss当前血量百分比: {healthPercentage:F0}%");
            
            if (healthPercentage > 75f)
            {
                Debug.Log("Boss处于第一阶段");
            }
            else if (healthPercentage > 30f)
            {
                Debug.Log("Boss处于第二阶段");
            }
            else
            {
                Debug.Log("Boss处于第三阶段");
            }
        }
    }
    
    // 用于调试的Gizmos
    private void OnDrawGizmos()
    {
        if (bossController != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(bossController.transform.position, 1f);
        }
        
        if (playerHealth != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(playerHealth.transform.position, 1f);
        }
    }
} 