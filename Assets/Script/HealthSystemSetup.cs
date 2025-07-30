using UnityEngine;

/// <summary>
/// 健康系统快速设置助手
/// 这个脚本帮助您快速将健康系统集成到现有的PlayerController中
/// </summary>
public class HealthSystemSetup : MonoBehaviour
{
    [Header("自动设置")]
    [SerializeField] private bool autoSetupOnStart = true;
    
    [Header("健康系统设置")]
    [SerializeField] private float initialMaxHealth = 100f;
    [SerializeField] private float initialMaxSkillPoints = 100f;
    [SerializeField] private float skillRegenRate = 5f;
    
    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupHealthSystem();
        }
    }
    
    /// <summary>
    /// 自动设置健康系统
    /// </summary>
    public void SetupHealthSystem()
    {
        // 检查PlayerController是否已有PlayerHealthSystem组件
        PlayerController playerController = PlayerController.instance;
        if (playerController == null)
        {
            Debug.LogError("找不到PlayerController实例！请确保场景中有PlayerController。");
            return;
        }
        
        // 检查是否已有PlayerHealthSystem组件
        PlayerHealthSystem healthSystem = playerController.GetComponent<PlayerHealthSystem>();
        if (healthSystem == null)
        {
            // 添加PlayerHealthSystem组件
            healthSystem = playerController.gameObject.AddComponent<PlayerHealthSystem>();
            Debug.Log("已自动添加PlayerHealthSystem组件到PlayerController。");
        }
        
        // 设置初始值（通过反射或者公共方法）
        Debug.Log($"健康系统设置完成！最大血量: {initialMaxHealth}, 最大技能值: {initialMaxSkillPoints}");
        
        // 添加测试组件（可选）
        HealthSystemExample example = playerController.GetComponent<HealthSystemExample>();
        if (example == null)
        {
            playerController.gameObject.AddComponent<HealthSystemExample>();
            Debug.Log("已添加HealthSystemExample测试组件。");
        }
    }
}

/* 
=== 健康系统使用说明 ===

1. 基础设置：
   - 将 PlayerHealthSystem.cs 脚本添加到您的玩家GameObject上
   - 将 HealthBarUI.cs 脚本添加到UI Canvas上的某个GameObject上
   - 或者使用 HealthSystemSetup.cs 进行自动设置

2. UI设置步骤：
   a) 创建UI Canvas（如果还没有）
   b) 在Canvas下创建血量条UI：
      - 创建空GameObject命名为"HealthBar"
      - 添加Slider组件作为血量条
      - 在Slider下创建子对象"Background"、"Fill Area"、"Fill"
      - 可选：添加TextMeshPro组件显示血量数字
   
   c) 创建技能条UI（类似血量条）：
      - 创建空GameObject命名为"SkillBar"
      - 添加Slider组件作为技能条
      - 设置相应的子对象
   
   d) 将HealthBarUI脚本添加到Canvas上的某个GameObject
   e) 在HealthBarUI脚本中拖拽对应的UI组件到相应字段

3. 脚本调用示例：
   // 造成伤害
   PlayerHealthSystem.instance.TakeDamage(damageAmount);
   
   // 治疗
   PlayerHealthSystem.instance.AddHealth(healAmount);
   
   // 消耗技能值
   if (PlayerHealthSystem.instance.ConsumeSkillPoints(skillCost))
   {
       // 技能释放成功
   }
   
   // 恢复技能值
   PlayerHealthSystem.instance.AddSkillPoints(restoreAmount);
   
   // 检查技能值是否足够
   if (PlayerHealthSystem.instance.HasEnoughSkillPoints(requiredSkill))
   {
       // 可以使用技能
   }

4. 事件订阅示例：
   void Start()
   {
       PlayerHealthSystem.instance.OnHealthChanged += OnHealthChanged;
       PlayerHealthSystem.instance.OnSkillChanged += OnSkillChanged;
       PlayerHealthSystem.instance.OnPlayerDeath += OnPlayerDeath;
   }
   
   void OnHealthChanged(float current, float max)
   {
       // 血量变化时的处理
   }

5. 测试功能：
   - 添加 HealthSystemExample.cs 到玩家对象上进行测试
   - 使用键盘按键H/J/K/L/R进行各种测试
   - 使用数字键1/2/3测试技能使用

6. 自定义设置：
   - 在PlayerHealthSystem脚本中调整最大血量、最大技能值
   - 调整技能值自动恢复速度
   - 在HealthBarUI脚本中自定义颜色、动画效果
   - 设置血量危险阈值和颜色变化

注意事项：
- 确保场景中只有一个PlayerHealthSystem实例
- UI组件需要正确连接到HealthBarUI脚本
- 如果使用TextMeshPro，需要导入TextMeshPro包
- 建议在游戏开始时就初始化健康系统

常见问题解决：
Q: UI不显示或不更新？
A: 检查HealthBarUI脚本是否正确连接了UI组件，确保PlayerHealthSystem存在于场景中

Q: 事件不触发？
A: 确保在适当的时机订阅事件，通常在Start()方法中

Q: 技能值不自动恢复？
A: 检查PlayerHealthSystem中的autoRegenSkill和skillRegenRate设置
*/