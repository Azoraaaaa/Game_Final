using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("攻击设置")]
    [SerializeField] private float attackDamage = 10f; // 攻击伤害
    [SerializeField] private string playerTag = "Player"; // 玩家标签
    
    // 引用对应的EnemyController
    private EnemyController enemyController;
    
    void Start()
    {
        // 获取同一个GameObject上的EnemyController组件
        enemyController = GetComponent<EnemyController>();
        
        // 如果当前对象没有EnemyController，尝试查找父对象
        if (enemyController == null)
        {
            enemyController = GetComponentInParent<EnemyController>();
        }
        
        // 如果还是找不到，显示警告
        if (enemyController == null)
        {
            Debug.LogWarning($"EnemyAttack: 在 {gameObject.name} 上未找到EnemyController组件");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞对象是否是玩家
        if (other.CompareTag(playerTag) && enemyController != null && enemyController.isAttacking)
        {
            if (PlayerHealthSystem.instance != null)
            {
                PlayerHealthSystem.instance.TakeDamage(attackDamage);
                Debug.Log($"对玩家造成 {attackDamage} 点伤害！");
            }
            else
            {
                Debug.LogWarning("PlayerHealthSystem.instance 为 null");
            }
        }
    }
}