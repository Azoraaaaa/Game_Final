using UnityEngine;

public class PlayerHealthCheck : MonoBehaviour
{
    [Header("检查设置")]
    [SerializeField] private bool checkOnStart = true;
    [SerializeField] private bool checkOnUpdate = false;
    
    private void Start()
    {
        if (checkOnStart)
        {
            CheckPlayerHealthSystem();
        }
    }
    
    private void Update()
    {
        if (checkOnUpdate)
        {
            CheckPlayerHealthSystem();
        }
    }
    
    [ContextMenu("检查玩家生命值系统")]
    public void CheckPlayerHealthSystem()
    {
        Debug.Log("=== 开始检查玩家生命值系统 ===");
        
        // 检查 PlayerHealthSystem 实例
        if (PlayerHealthSystem.instance != null)
        {
            Debug.Log($"✓ PlayerHealthSystem 实例存在");
            Debug.Log($"  当前血量: {PlayerHealthSystem.instance.CurrentHealth}/{PlayerHealthSystem.instance.MaxHealth}");
            Debug.Log($"  当前技能值: {PlayerHealthSystem.instance.CurrentSkillPoints}/{PlayerHealthSystem.instance.MaxSkillPoints}");
            Debug.Log($"  是否死亡: {PlayerHealthSystem.instance.IsDead}");
        }
        else
        {
            Debug.LogError("✗ PlayerHealthSystem 实例不存在！");
        }
        
        // 检查玩家对象
        if (PlayerController.instance != null)
        {
            Debug.Log($"✓ PlayerController 实例存在");
            
            // 检查玩家对象上的组件
            PlayerHealthSystem healthSystem = PlayerController.instance.GetComponent<PlayerHealthSystem>();
            if (healthSystem != null)
            {
                Debug.Log($"✓ PlayerController 上有 PlayerHealthSystem 组件");
            }
            else
            {
                Debug.LogError("✗ PlayerController 上没有 PlayerHealthSystem 组件！");
            }
            
            // 检查玩家对象的 Tag
            if (PlayerController.instance.CompareTag("Player"))
            {
                Debug.Log($"✓ PlayerController 的 Tag 是 'Player'");
            }
            else
            {
                Debug.LogError($"✗ PlayerController 的 Tag 不是 'Player'，当前是: '{PlayerController.instance.tag}'");
            }
            
            // 检查玩家对象的碰撞器
            Collider playerCollider = PlayerController.instance.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Debug.Log($"✓ PlayerController 上有碰撞器: {playerCollider.GetType().Name}");
                Debug.Log($"  碰撞器是否启用: {playerCollider.enabled}");
                Debug.Log($"  是否为触发器: {playerCollider.isTrigger}");
            }
            else
            {
                Debug.LogError("✗ PlayerController 上没有碰撞器！");
            }
        }
        else
        {
            Debug.LogError("✗ PlayerController 实例不存在！");
        }
        
        Debug.Log("=== 检查完成 ===");
    }
} 