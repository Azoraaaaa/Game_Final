using UnityEngine;

/// <summary>
/// Boss战斗测试脚本 - 测试Boss攻击和玩家掉血功能
/// </summary>
public class BossCombatTest : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private BossController bossController;
    [SerializeField] private PlayerHealth playerHealth;
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
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
        
        if (enableDebugLogs)
        {
            Debug.Log("🔧 Boss战斗测试脚本已启动");
            Debug.Log("按键说明：");
            Debug.Log("F1 - 对Boss造成100点伤害");
            Debug.Log("F2 - 对Boss造成500点伤害（快速死亡）");
            Debug.Log("F3 - 对玩家造成20点伤害");
            Debug.Log("F4 - 对玩家造成50点伤害");
            Debug.Log("F5 - 检查Boss和玩家状态");
            Debug.Log("F6 - 治疗玩家50点血量");
        }
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
            TestBossDeath();
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            TestPlayerDamage(20f);
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            TestPlayerDamage(50f);
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CheckCombatStatus();
        }
        
        if (Input.GetKeyDown(KeyCode.F6))
        {
            TestPlayerHeal();
        }
    }
    
    private void TestBossDamage()
    {
        if (bossController != null && !bossController.IsDead)
        {
            bossController.TakeDamage(100f);
            Debug.Log($"对Boss造成100点伤害，当前HP: {bossController.CurrentHP}");
        }
        else
        {
            Debug.Log("Boss已死亡或未找到");
        }
    }
    
    private void TestBossDeath()
    {
        if (bossController != null && !bossController.IsDead)
        {
            bossController.TakeDamage(500f);
            Debug.Log($"对Boss造成500点伤害，当前HP: {bossController.CurrentHP}");
        }
        else
        {
            Debug.Log("Boss已死亡或未找到");
        }
    }
    
    private void TestPlayerDamage(float damage)
    {
        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"对玩家造成{damage}点伤害");
        }
        else
        {
            Debug.Log("玩家已死亡或未找到PlayerHealth组件");
        }
    }
    
    private void TestPlayerHeal()
    {
        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.Heal(50f);
            Debug.Log("治疗玩家50点血量");
        }
        else
        {
            Debug.Log("玩家已死亡或未找到PlayerHealth组件");
        }
    }
    
    private void CheckCombatStatus()
    {
        Debug.Log("=== 战斗状态检查 ===");
        
        // 检查Boss状态
        if (bossController != null)
        {
            Debug.Log($"Boss状态: 存活={!bossController.IsDead}, HP={bossController.CurrentHP}/{bossController.MaxHP}");
        }
        else
        {
            Debug.Log("❌ 未找到BossController");
        }
        
        // 检查玩家状态
        if (playerHealth != null)
        {
            Debug.Log($"玩家状态: 存活={!playerHealth.IsDead()}, HP百分比={playerHealth.GetHealthPercentage():P1}");
        }
        else
        {
            Debug.Log("❌ 未找到PlayerHealth");
        }
        
        // 检查攻击触发器
        BossAttackTrigger[] attackTriggers = FindObjectsOfType<BossAttackTrigger>();
        Debug.Log($"找到{attackTriggers.Length}个攻击触发器");
        
        // 检查弹体
        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        Debug.Log($"找到{projectiles.Length}个弹体");
    }
    
    // 用于调试的Gizmos
    private void OnDrawGizmos()
    {
        // 绘制Boss位置
        if (bossController != null)
        {
            Gizmos.color = bossController.IsDead ? Color.red : Color.green;
            Gizmos.DrawWireSphere(bossController.transform.position, 1f);
        }
        
        // 绘制玩家位置
        if (playerHealth != null)
        {
            Gizmos.color = playerHealth.IsDead() ? Color.red : Color.blue;
            Gizmos.DrawWireSphere(playerHealth.transform.position, 0.5f);
        }
    }
} 