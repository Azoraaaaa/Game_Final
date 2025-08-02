using UnityEngine;

/// <summary>
/// Enemy系统测试脚本 - 用于验证Enemy功能
/// </summary>
public class EnemyTest : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private EnemyController[] enemies;
    [SerializeField] private EnemyAttack[] enemyAttacks;
    
    [Header("测试按钮")]
    [SerializeField] private bool findEnemies = false;
    [SerializeField] private bool testEnemyHealth = false;
    [SerializeField] private bool testEnemyAttack = false;
    
    void Start()
    {
        FindEnemies();
    }
    
    void Update()
    {
        if (findEnemies)
        {
            findEnemies = false;
            FindEnemies();
        }
        
        if (testEnemyHealth)
        {
            testEnemyHealth = false;
            TestEnemyHealth();
        }
        
        if (testEnemyAttack)
        {
            testEnemyAttack = false;
            TestEnemyAttack();
        }
    }
    
    private void FindEnemies()
    {
        // 查找场景中的所有Enemy
        enemies = FindObjectsOfType<EnemyController>();
        enemyAttacks = FindObjectsOfType<EnemyAttack>();
        
        Debug.Log($"找到 {enemies.Length} 个EnemyController");
        Debug.Log($"找到 {enemyAttacks.Length} 个EnemyAttack");
        
        // 检查每个Enemy的组件设置
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];
            var attack = enemy.GetComponent<EnemyAttack>();
            
            if (attack != null)
            {
                Debug.Log($"Enemy {i}: {enemy.name} - 攻击组件正常");
            }
            else
            {
                Debug.LogWarning($"Enemy {i}: {enemy.name} - 缺少EnemyAttack组件");
            }
        }
    }
    
    private void TestEnemyHealth()
    {
        if (enemies.Length > 0)
        {
            // 测试第一个Enemy的死亡功能
            enemies[0].Die();
            Debug.Log($"测试Enemy死亡: {enemies[0].name}");
        }
        else
        {
            Debug.LogWarning("没有找到Enemy进行测试");
        }
    }
    
    private void TestEnemyAttack()
    {
        if (enemyAttacks.Length > 0)
        {
            Debug.Log($"EnemyAttack组件状态检查完成，共 {enemyAttacks.Length} 个");
        }
        else
        {
            Debug.LogWarning("没有找到EnemyAttack组件");
        }
    }
    
    // 在Inspector中显示当前状态
    void OnValidate()
    {
        if (enemies != null && enemies.Length > 0)
        {
            Debug.Log($"当前场景中有 {enemies.Length} 个Enemy");
        }
    }
} 