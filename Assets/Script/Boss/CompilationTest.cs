using UnityEngine;

/// <summary>
/// 编译测试脚本 - 验证所有Boss系统组件的引用是否正确
/// </summary>
public class CompilationTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("🔧 Boss系统编译测试开始...");
        
        // 测试ProjectileType枚举
        TestProjectileType();
        
        // 测试BossController引用
        TestBossController();
        
        // 测试GameManager引用
        TestGameManager();
        
        Debug.Log("✅ Boss系统编译测试完成！");
    }
    
    private void TestProjectileType()
    {
        try
        {
            // 测试ProjectileType枚举访问
            Projectile.ProjectileType waterType = Projectile.ProjectileType.Water;
            Projectile.ProjectileType iceType = Projectile.ProjectileType.Ice;
            
            Debug.Log($"✅ ProjectileType枚举测试通过: {waterType}, {iceType}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ ProjectileType枚举测试失败: {e.Message}");
        }
    }
    
    private void TestBossController()
    {
        try
        {
            // 测试BossController组件查找
            BossController boss = FindFirstObjectByType<BossController>();
            if (boss != null)
            {
                Debug.Log($"✅ BossController查找成功: HP {boss.CurrentHP}/{boss.MaxHP}");
            }
            else
            {
                Debug.Log("⚠️ BossController未找到（这是正常的，如果没有在场景中）");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ BossController测试失败: {e.Message}");
        }
    }
    
    private void TestGameManager()
    {
        try
        {
            // 测试GameManager单例访问
            if (GameManager.Instance != null)
            {
                Debug.Log("✅ GameManager单例访问成功");
            }
            else
            {
                Debug.Log("⚠️ GameManager未找到（这是正常的，如果没有在场景中）");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ GameManager测试失败: {e.Message}");
        }
    }
} 