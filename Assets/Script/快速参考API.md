# 🎯 玩家健康系统 - 快速参考API

## 🚀 核心调用方法

### 血量管理
```csharp
// 造成伤害
PlayerHealthSystem.instance.TakeDamage(25f);

// 治疗血量
PlayerHealthSystem.instance.AddHealth(30f);

// 治疗到满血
PlayerHealthSystem.instance.FullHeal();

// 设置最大血量
PlayerHealthSystem.instance.SetMaxHealth(150f, true);
```

### 技能值管理
```csharp
// 检查技能值是否足够
if (PlayerHealthSystem.instance.HasEnoughSkillPoints(50f))
{
    // 消耗技能值
    PlayerHealthSystem.instance.ConsumeSkillPoints(50f);
}

// 恢复技能值
PlayerHealthSystem.instance.AddSkillPoints(25f);

// 技能值回满
PlayerHealthSystem.instance.RestoreFullSkill();

// 设置最大技能值
PlayerHealthSystem.instance.SetMaxSkillPoints(200f, true);
```

### 状态查询
```csharp
// 获取当前值
float currentHP = PlayerHealthSystem.instance.CurrentHealth;
float maxHP = PlayerHealthSystem.instance.MaxHealth;
float currentMP = PlayerHealthSystem.instance.CurrentSkillPoints;
float maxMP = PlayerHealthSystem.instance.MaxSkillPoints;

// 获取百分比
float hpPercent = PlayerHealthSystem.instance.HealthPercentage;
float mpPercent = PlayerHealthSystem.instance.SkillPercentage;

// 检查状态
bool isDead = PlayerHealthSystem.instance.IsDead;

// 获取状态信息字符串
string status = PlayerHealthSystem.instance.GetStatusInfo();
```

## 📱 事件订阅

```csharp
private void Start()
{
    var health = PlayerHealthSystem.instance;
    health.OnHealthChanged += OnHealthChanged;
    health.OnSkillChanged += OnSkillChanged;
    health.OnPlayerDeath += OnPlayerDeath;
}

private void OnHealthChanged(float current, float max) { }
private void OnSkillChanged(float current, float max) { }
private void OnPlayerDeath() { }

private void OnDestroy()
{
    // 记得取消订阅
    if (PlayerHealthSystem.instance != null)
    {
        PlayerHealthSystem.instance.OnHealthChanged -= OnHealthChanged;
        // ... 其他事件
    }
}
```

## 🎮 测试按键

在运行时按下这些键进行测试（需要添加HealthSystemExample脚本）：

- `H` - 受到伤害 (10点)
- `J` - 治疗 (15点)  
- `K` - 消耗技能值 (20点)
- `L` - 恢复技能值 (25点)
- `R` - 重置到满值
- `1` - 火球术 (消耗30点技能值)
- `2` - 治疗术 (消耗40点技能值)
- `3` - 护盾 (消耗50点技能值)

## 💡 实用代码片段

### 敌人攻击
```csharp
public void AttackPlayer(float damage)
{
    float actualDamage = PlayerHealthSystem.instance.TakeDamage(damage);
    Debug.Log($"造成 {actualDamage} 点伤害");
}
```

### 技能释放
```csharp
public bool CastSkill(float cost)
{
    if (PlayerHealthSystem.instance.ConsumeSkillPoints(cost))
    {
        // 技能释放成功
        return true;
    }
    Debug.Log("技能值不足！");
    return false;
}
```

### 物品使用
```csharp
public void UseHealthPotion(float healAmount)
{
    float actualHeal = PlayerHealthSystem.instance.AddHealth(healAmount);
    UIManager.instance?.ShowNotification($"血量 +{actualHeal:F0}", 2f);
}
```

### 升级系统
```csharp
public void LevelUp(float healthIncrease, float skillIncrease)
{
    var health = PlayerHealthSystem.instance;
    health.SetMaxHealth(health.MaxHealth + healthIncrease, true);
    health.SetMaxSkillPoints(health.MaxSkillPoints + skillIncrease, true);
}
```

## ⚙️ UI组件设置清单

### HealthBarUI脚本需要连接的组件：
- ✅ Health Slider (血量滑动条)
- ✅ Health Fill (血量填充图片)
- ✅ Health Text (血量文本)
- ✅ Health Background Fill (背景血量条)
- ✅ Skill Slider (技能滑动条)  
- ✅ Skill Fill (技能填充图片)
- ✅ Skill Text (技能文本)

### Inspector设置建议：
```
颜色设置:
├── Health Color: #FF0000 (红色)
├── Skill Color: #0080FF (蓝色)
├── Low Health Color: #FFFF00 (黄色)
└── Critical Health Color: #800000 (深红色)

动画设置:
├── Smooth Speed: 2-5
├── Background Fade Speed: 1-2
└── Smooth Transition: ✓

阈值设置:
├── Low Health Threshold: 0.3 (30%)
└── Critical Health Threshold: 0.15 (15%)
```

## 🔧 常见问题快速修复

### UI不更新
```csharp
// 检查PlayerHealthSystem是否存在
if (PlayerHealthSystem.instance == null)
{
    Debug.LogError("PlayerHealthSystem未找到！");
}

// 强制更新UI
PlayerHealthSystem.instance.OnHealthChanged?.Invoke(
    PlayerHealthSystem.instance.CurrentHealth, 
    PlayerHealthSystem.instance.MaxHealth
);
```

### 技能值不自动恢复
```csharp
// 在Inspector中确保：
// Auto Regen Skill: ✓
// Skill Regen Rate: > 0 (建议5-10)
```

### 事件不触发
```csharp
// 延迟订阅，确保PlayerHealthSystem已初始化
private IEnumerator Start()
{
    yield return null; // 等待一帧
    PlayerHealthSystem.instance.OnHealthChanged += OnHealthChanged;
}
```

## 📦 必需文件检查清单

确保以下文件都已正确添加到项目中：
- ✅ `PlayerHealthSystem.cs` (添加到玩家对象)
- ✅ `HealthBarUI.cs` (添加到UI Canvas)
- ✅ `HealthSystemExample.cs` (可选，用于测试)
- ✅ `HealthSystemSetup.cs` (可选，用于自动设置)

## 🎯 最佳实践

1. **总是检查实例是否存在**
   ```csharp
   if (PlayerHealthSystem.instance != null)
   {
       // 执行操作
   }
   ```

2. **使用事件而不是轮询**
   ```csharp
   // 好的做法
   PlayerHealthSystem.instance.OnHealthChanged += UpdateUI;
   
   // 避免在Update中轮询
   // if (currentHealth != lastHealth) UpdateUI();
   ```

3. **正确取消事件订阅**
   ```csharp
   void OnDestroy()
   {
       if (PlayerHealthSystem.instance != null)
       {
           PlayerHealthSystem.instance.OnHealthChanged -= UpdateUI;
       }
   }
   ```

4. **使用返回值进行反馈**
   ```csharp
   float actualDamage = PlayerHealthSystem.instance.TakeDamage(damage);
   if (actualDamage > 0)
   {
       // 显示伤害效果
   }
   ```

---

💡 **提示：** 将此文件保存为书签，方便随时查阅常用API！