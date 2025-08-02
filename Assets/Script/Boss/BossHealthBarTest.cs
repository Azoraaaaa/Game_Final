using UnityEngine;

/// <summary>
/// Boss血条测试脚本 - 用于测试血条功能
/// </summary>
public class BossHealthBarTest : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private BossController bossController;
    [SerializeField] private BossHealthBarSimple healthBar;
    
    [Header("测试按钮")]
    [SerializeField] private bool testDamage = false;
    [SerializeField] private bool testHeal = false;
    [SerializeField] private bool testDeath = false;
    [SerializeField] private bool resetBoss = false;
    
    [Header("测试参数")]
    [SerializeField] private float damageAmount = 100f;
    [SerializeField] private float healAmount = 50f;
    
    void Start()
    {
        // 自动查找组件
        if (bossController == null)
        {
            bossController = FindFirstObjectByType<BossController>();
        }
        
        if (healthBar == null)
        {
            healthBar = FindFirstObjectByType<BossHealthBarSimple>();
        }
    }
    
    void Update()
    {
        // 测试按钮功能
        if (testDamage)
        {
            testDamage = false;
            TestDamage();
        }
        
        if (testHeal)
        {
            testHeal = false;
            TestHeal();
        }
        
        if (testDeath)
        {
            testDeath = false;
            TestDeath();
        }
        
        if (resetBoss)
        {
            resetBoss = false;
            ResetBoss();
        }
    }
    
    private void TestDamage()
    {
        if (bossController != null)
        {
            bossController.TakeDamage(damageAmount);
            Debug.Log($"对Boss造成 {damageAmount} 点伤害");
        }
        else
        {
            Debug.LogWarning("未找到BossController");
        }
    }
    
    private void TestHeal()
    {
        if (bossController != null)
        {
            // 注意：BossController没有治疗功能，这里只是示例
            Debug.Log("BossController没有治疗功能");
        }
        else
        {
            Debug.LogWarning("未找到BossController");
        }
    }
    
    private void TestDeath()
    {
        if (bossController != null)
        {
            // 直接造成致命伤害
            bossController.TakeDamage(bossController.CurrentHP);
            Debug.Log("Boss已死亡");
        }
        else
        {
            Debug.LogWarning("未找到BossController");
        }
    }
    
    private void ResetBoss()
    {
        if (bossController != null)
        {
            // 重新加载场景来重置Boss
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
        else
        {
            Debug.LogWarning("未找到BossController");
        }
    }
    
    // 在Inspector中显示当前状态
    void OnValidate()
    {
        if (bossController != null)
        {
            Debug.Log($"Boss状态 - HP: {bossController.CurrentHP}/{bossController.MaxHP}, 死亡: {bossController.IsDead}");
        }
    }
} 