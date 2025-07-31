using UnityEngine;

/// <summary>
/// Boss死亡测试脚本 - 专门测试Boss死亡功能
/// </summary>
public class BossDeathTest : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private BossController bossController;
    [SerializeField] private bool enableDebugLogs = true;
    
    void Start()
    {
        // 自动查找Boss控制器
        if (bossController == null)
        {
            bossController = FindFirstObjectByType<BossController>();
        }
        
        if (enableDebugLogs)
        {
            Debug.Log("🔧 Boss死亡测试脚本已启动");
            Debug.Log("按键说明：");
            Debug.Log("F1 - 对Boss造成100点伤害");
            Debug.Log("F2 - 对Boss造成500点伤害（快速死亡）");
            Debug.Log("F3 - 检查Boss状态");
            Debug.Log("F4 - 重置Boss（如果可能）");
        }
    }
    
    void Update()
    {
        if (bossController == null) return;
        
        // 测试按键
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestNormalDamage();
        }
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestInstantDeath();
        }
        
        if (Input.GetKeyDown(KeyCode.F3))
        {
            CheckBossStatus();
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            ResetBoss();
        }
    }
    
    private void TestNormalDamage()
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
    
    private void TestInstantDeath()
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
    
    private void CheckBossStatus()
    {
        if (bossController != null)
        {
            Debug.Log("=== Boss状态检查 ===");
            Debug.Log($"是否死亡: {bossController.IsDead}");
            Debug.Log($"当前HP: {bossController.CurrentHP}/{bossController.MaxHP}");
            Debug.Log($"HP百分比: {(bossController.CurrentHP / bossController.MaxHP) * 100:F1}%");
            
            if (bossController.IsDead)
            {
                Debug.Log("✅ Boss已正确死亡");
            }
            else
            {
                Debug.Log("⚠️ Boss仍然存活");
            }
        }
        else
        {
            Debug.LogError("❌ 未找到BossController");
        }
    }
    
    private void ResetBoss()
    {
        if (bossController != null)
        {
            // 注意：这个方法可能不会完全重置Boss，因为死亡后Boss会被销毁
            Debug.Log("尝试重置Boss...");
            Debug.Log("注意：如果Boss已死亡，可能需要重新加载场景");
        }
        else
        {
            Debug.Log("未找到BossController，无法重置");
        }
    }
    
    // 用于调试的Gizmos
    private void OnDrawGizmos()
    {
        if (bossController != null)
        {
            // 根据Boss状态改变颜色
            if (bossController.IsDead)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            
            Gizmos.DrawWireSphere(bossController.transform.position, 2f);
        }
    }
} 