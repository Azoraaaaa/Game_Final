using UnityEngine;

/// <summary>
/// 玩家健康系统使用示例和测试脚本
/// 展示如何调用PlayerHealthSystem的各种方法
/// </summary>
public class HealthSystemExample : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private float testDamageAmount = 10f;
    [SerializeField] private float testHealAmount = 15f;
    [SerializeField] private float testSkillCost = 20f;
    [SerializeField] private float testSkillRestore = 25f;
    
    [Header("技能使用示例")]
    [SerializeField] private float fireBallSkillCost = 30f;
    [SerializeField] private float healSkillCost = 40f;
    [SerializeField] private float shieldSkillCost = 50f;

    private void Update()
    {
        // 键盘测试控制
        HandleTestInputs();
    }

    /// <summary>
    /// 处理测试输入
    /// </summary>
    private void HandleTestInputs()
    {
        if (PlayerHealthSystem.instance == null) return;

        // 测试伤害 - 按H键
        if (Input.GetKeyDown(KeyCode.H))
        {
            TestTakeDamage();
        }
        
        // 测试治疗 - 按J键
        if (Input.GetKeyDown(KeyCode.J))
        {
            TestHeal();
        }
        
        // 测试技能消耗 - 按K键
        if (Input.GetKeyDown(KeyCode.K))
        {
            TestSkillConsumption();
        }
        
        // 测试技能恢复 - 按L键
        if (Input.GetKeyDown(KeyCode.L))
        {
            TestSkillRestore();
        }
        
        // 重置血量和技能 - 按R键
        if (Input.GetKeyDown(KeyCode.R))
        {
            TestReset();
        }
        
        // 模拟技能使用 - 数字键
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseFireBallSkill();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseHealSkill();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseShieldSkill();
        }
    }

    #region 基础测试方法

    /// <summary>
    /// 测试受到伤害
    /// </summary>
    public void TestTakeDamage()
    {
        float actualDamage = PlayerHealthSystem.instance.TakeDamage(testDamageAmount);
        Debug.Log($"受到伤害: {actualDamage:F1} 点，当前血量: {PlayerHealthSystem.instance.CurrentHealth:F1}");
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotification($"受到 {actualDamage:F1} 点伤害!", 2f);
        }
    }

    /// <summary>
    /// 测试治疗
    /// </summary>
    public void TestHeal()
    {
        float actualHeal = PlayerHealthSystem.instance.AddHealth(testHealAmount);
        Debug.Log($"治疗: {actualHeal:F1} 点，当前血量: {PlayerHealthSystem.instance.CurrentHealth:F1}");
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotification($"恢复 {actualHeal:F1} 点血量!", 2f);
        }
    }

    /// <summary>
    /// 测试技能消耗
    /// </summary>
    public void TestSkillConsumption()
    {
        bool success = PlayerHealthSystem.instance.ConsumeSkillPoints(testSkillCost);
        
        if (success)
        {
            Debug.Log($"消耗技能值: {testSkillCost:F1} 点，当前技能值: {PlayerHealthSystem.instance.CurrentSkillPoints:F1}");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification($"消耗 {testSkillCost:F1} 点技能值!", 2f);
            }
        }
        else
        {
            Debug.Log("技能值不足！");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification("技能值不足！", 2f);
            }
        }
    }

    /// <summary>
    /// 测试技能恢复
    /// </summary>
    public void TestSkillRestore()
    {
        float actualRestore = PlayerHealthSystem.instance.AddSkillPoints(testSkillRestore);
        Debug.Log($"恢复技能值: {actualRestore:F1} 点，当前技能值: {PlayerHealthSystem.instance.CurrentSkillPoints:F1}");
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotification($"恢复 {actualRestore:F1} 点技能值!", 2f);
        }
    }

    /// <summary>
    /// 测试重置
    /// </summary>
    public void TestReset()
    {
        PlayerHealthSystem.instance.ResetToFull();
        Debug.Log("血量和技能值已重置到满值");
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotification("血量和技能值已回满!", 2f);
        }
    }

    #endregion

    #region 模拟技能使用示例

    /// <summary>
    /// 火球术技能示例
    /// </summary>
    public void UseFireBallSkill()
    {
        if (PlayerHealthSystem.instance.HasEnoughSkillPoints(fireBallSkillCost))
        {
            if (PlayerHealthSystem.instance.ConsumeSkillPoints(fireBallSkillCost))
            {
                Debug.Log("火球术释放成功！");
                
                if (UIManager.instance != null)
                {
                    UIManager.instance.ShowNotification("火球术！", 2f);
                }
                
                // 这里可以添加实际的技能效果
                // 比如生成火球、造成伤害等
            }
        }
        else
        {
            Debug.Log("技能值不足，无法释放火球术！");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification("技能值不足！", 2f);
            }
        }
    }

    /// <summary>
    /// 治疗术技能示例
    /// </summary>
    public void UseHealSkill()
    {
        if (PlayerHealthSystem.instance.HasEnoughSkillPoints(healSkillCost))
        {
            if (PlayerHealthSystem.instance.ConsumeSkillPoints(healSkillCost))
            {
                // 治疗效果
                float healAmount = 50f;
                PlayerHealthSystem.instance.AddHealth(healAmount);
                
                Debug.Log("治疗术释放成功！");
                
                if (UIManager.instance != null)
                {
                    UIManager.instance.ShowNotification("治疗术！", 2f);
                }
            }
        }
        else
        {
            Debug.Log("技能值不足，无法释放治疗术！");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification("技能值不足！", 2f);
            }
        }
    }

    /// <summary>
    /// 护盾技能示例
    /// </summary>
    public void UseShieldSkill()
    {
        if (PlayerHealthSystem.instance.HasEnoughSkillPoints(shieldSkillCost))
        {
            if (PlayerHealthSystem.instance.ConsumeSkillPoints(shieldSkillCost))
            {
                Debug.Log("护盾技能释放成功！");
                
                if (UIManager.instance != null)
                {
                    UIManager.instance.ShowNotification("护盾激活！", 2f);
                }
                
                // 这里可以添加护盾效果
                // 比如临时增加防御力、免疫下一次伤害等
            }
        }
        else
        {
            Debug.Log("技能值不足，无法释放护盾！");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification("技能值不足！", 2f);
            }
        }
    }

    #endregion

    #region 其他脚本调用示例

    /// <summary>
    /// 敌人攻击示例（其他脚本可以这样调用）
    /// </summary>
    /// <param name="damage">伤害值</param>
    public static void DealDamageToPlayer(float damage)
    {
        if (PlayerHealthSystem.instance != null)
        {
            float actualDamage = PlayerHealthSystem.instance.TakeDamage(damage);
            Debug.Log($"玩家受到 {actualDamage:F1} 点伤害");
        }
    }

    /// <summary>
    /// 拾取治疗物品示例
    /// </summary>
    /// <param name="healAmount">治疗量</param>
    public static void UseHealthPotion(float healAmount)
    {
        if (PlayerHealthSystem.instance != null)
        {
            float actualHeal = PlayerHealthSystem.instance.AddHealth(healAmount);
            Debug.Log($"使用治疗药水，恢复 {actualHeal:F1} 点血量");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification($"使用治疗药水 +{actualHeal:F1}", 2f);
            }
        }
    }

    /// <summary>
    /// 拾取魔法药水示例
    /// </summary>
    /// <param name="skillAmount">技能值恢复量</param>
    public static void UseManaPotion(float skillAmount)
    {
        if (PlayerHealthSystem.instance != null)
        {
            float actualRestore = PlayerHealthSystem.instance.AddSkillPoints(skillAmount);
            Debug.Log($"使用魔法药水，恢复 {actualRestore:F1} 点技能值");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification($"使用魔法药水 +{actualRestore:F1}", 2f);
            }
        }
    }

    /// <summary>
    /// 升级增加最大血量示例
    /// </summary>
    /// <param name="additionalHealth">增加的最大血量</param>
    public static void LevelUpHealth(float additionalHealth)
    {
        if (PlayerHealthSystem.instance != null)
        {
            float newMaxHealth = PlayerHealthSystem.instance.MaxHealth + additionalHealth;
            PlayerHealthSystem.instance.SetMaxHealth(newMaxHealth, true); // 同时调整当前血量
            
            Debug.Log($"升级！最大血量增加 {additionalHealth:F1}，新的最大血量: {newMaxHealth:F1}");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification($"升级！最大血量 +{additionalHealth:F1}", 3f);
            }
        }
    }

    /// <summary>
    /// 升级增加最大技能值示例
    /// </summary>
    /// <param name="additionalSkill">增加的最大技能值</param>
    public static void LevelUpSkill(float additionalSkill)
    {
        if (PlayerHealthSystem.instance != null)
        {
            float newMaxSkill = PlayerHealthSystem.instance.MaxSkillPoints + additionalSkill;
            PlayerHealthSystem.instance.SetMaxSkillPoints(newMaxSkill, true); // 同时调整当前技能值
            
            Debug.Log($"升级！最大技能值增加 {additionalSkill:F1}，新的最大技能值: {newMaxSkill:F1}");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification($"升级！最大技能值 +{additionalSkill:F1}", 3f);
            }
        }
    }

    #endregion

    /// <summary>
    /// 在UI上显示使用说明
    /// </summary>
    private void OnGUI()
    {
        if (PlayerHealthSystem.instance == null) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 400));
        GUILayout.Label("=== 健康系统测试控制 ===");
        GUILayout.Label($"当前状态：");
        GUILayout.Label(PlayerHealthSystem.instance.GetStatusInfo());
        GUILayout.Space(10);
        
        GUILayout.Label("测试按键：");
        GUILayout.Label("H - 受到伤害");
        GUILayout.Label("J - 治疗");
        GUILayout.Label("K - 消耗技能值");
        GUILayout.Label("L - 恢复技能值");
        GUILayout.Label("R - 重置到满值");
        GUILayout.Space(10);
        
        GUILayout.Label("技能测试：");
        GUILayout.Label("1 - 火球术 (30点技能值)");
        GUILayout.Label("2 - 治疗术 (40点技能值)");
        GUILayout.Label("3 - 护盾 (50点技能值)");
        
        GUILayout.EndArea();
    }
}