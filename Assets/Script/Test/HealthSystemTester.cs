using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 健康系统测试工具
/// 提供UI界面来测试各种功能
/// </summary>
public class HealthSystemTester : MonoBehaviour
{
    [Header("伤害测试")]
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private Button damageButton;
    [SerializeField] private TMP_InputField damageInput;
    
    [Header("治疗测试")]
    [SerializeField] private float healAmount = 15f;
    [SerializeField] private Button healButton;
    [SerializeField] private TMP_InputField healInput;
    
    [Header("技能测试")]
    [SerializeField] private float skillCost = 20f;
    [SerializeField] private Button consumeSkillButton;
    [SerializeField] private TMP_InputField skillInput;
    
    [Header("状态显示")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private float updateInterval = 0.1f;
    
    private float nextUpdateTime;
    
    private void Start()
    {
        // 设置按钮监听
        if (damageButton) damageButton.onClick.AddListener(TestDamage);
        if (healButton) healButton.onClick.AddListener(TestHeal);
        if (consumeSkillButton) consumeSkillButton.onClick.AddListener(TestConsumeSkill);
        
        // 设置输入框监听
        if (damageInput) damageInput.onValueChanged.AddListener(UpdateDamageAmount);
        if (healInput) healInput.onValueChanged.AddListener(UpdateHealAmount);
        if (skillInput) skillInput.onValueChanged.AddListener(UpdateSkillAmount);
        
        // 初始化输入框显示
        if (damageInput) damageInput.text = damageAmount.ToString();
        if (healInput) healInput.text = healAmount.ToString();
        if (skillInput) skillInput.text = skillCost.ToString();
        
        // 订阅健康系统事件
        if (PlayerHealthSystem.instance != null)
        {
            PlayerHealthSystem.instance.OnHealthChanged += OnHealthChanged;
            PlayerHealthSystem.instance.OnSkillChanged += OnSkillChanged;
            PlayerHealthSystem.instance.OnPlayerDeath += OnPlayerDeath;
        }
    }
    
    private void Update()
    {
        // 定期更新状态显示
        if (Time.time >= nextUpdateTime)
        {
            UpdateStatusDisplay();
            nextUpdateTime = Time.time + updateInterval;
        }
    }
    
    private void OnDestroy()
    {
        // 取消事件订阅
        if (PlayerHealthSystem.instance != null)
        {
            PlayerHealthSystem.instance.OnHealthChanged -= OnHealthChanged;
            PlayerHealthSystem.instance.OnSkillChanged -= OnSkillChanged;
            PlayerHealthSystem.instance.OnPlayerDeath -= OnPlayerDeath;
        }
    }
    
    #region 测试方法
    
    public void TestDamage()
    {
        if (PlayerHealthSystem.instance != null)
        {
            float actualDamage = PlayerHealthSystem.instance.TakeDamage(damageAmount);
            Debug.Log($"造成 {actualDamage:F1} 点伤害！");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification($"受到 {actualDamage:F1} 点伤害!", 2f);
            }
        }
    }
    
    public void TestHeal()
    {
        if (PlayerHealthSystem.instance != null)
        {
            float actualHeal = PlayerHealthSystem.instance.AddHealth(healAmount);
            Debug.Log($"恢复 {actualHeal:F1} 点血量！");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification($"恢复 {actualHeal:F1} 点血量!", 2f);
            }
        }
    }
    
    public void TestConsumeSkill()
    {
        if (PlayerHealthSystem.instance != null)
        {
            bool success = PlayerHealthSystem.instance.ConsumeSkillPoints(skillCost);
            
            if (success)
            {
                Debug.Log($"消耗 {skillCost:F1} 点技能值！");
                
                if (UIManager.instance != null)
                {
                    UIManager.instance.ShowNotification($"消耗 {skillCost:F1} 点技能值!", 2f);
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
    }
    
    public void TestResetAll()
    {
        if (PlayerHealthSystem.instance != null)
        {
            PlayerHealthSystem.instance.ResetToFull();
            Debug.Log("重置所有数值到满值！");
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification("重置到满值！", 2f);
            }
        }
    }
    
    #endregion
    
    #region UI更新方法
    
    private void UpdateDamageAmount(string value)
    {
        if (float.TryParse(value, out float amount))
        {
            damageAmount = amount;
        }
    }
    
    private void UpdateHealAmount(string value)
    {
        if (float.TryParse(value, out float amount))
        {
            healAmount = amount;
        }
    }
    
    private void UpdateSkillAmount(string value)
    {
        if (float.TryParse(value, out float amount))
        {
            skillCost = amount;
        }
    }
    
    private void UpdateStatusDisplay()
    {
        if (statusText != null && PlayerHealthSystem.instance != null)
        {
            var health = PlayerHealthSystem.instance;
            string status = $"血量: {health.CurrentHealth:F1}/{health.MaxHealth:F1} ({health.HealthPercentage:P1})\n" +
                          $"技能: {health.CurrentSkillPoints:F1}/{health.MaxSkillPoints:F1} ({health.SkillPercentage:P1})\n" +
                          $"状态: {(health.IsDead ? "死亡" : "存活")}";
            
            statusText.text = status;
        }
    }
    
    #endregion
    
    #region 事件处理
    
    private void OnHealthChanged(float current, float max)
    {
        UpdateStatusDisplay();
    }
    
    private void OnSkillChanged(float current, float max)
    {
        UpdateStatusDisplay();
    }
    
    private void OnPlayerDeath()
    {
        Debug.Log("玩家死亡！");
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowNotification("玩家死亡！", 3f);
        }
    }
    
    #endregion
    
    #region 调试GUI
    
    private void OnGUI()
    {
        // 如果没有UI组件，使用IMGUI显示测试界面
        if (damageButton == null || healButton == null || consumeSkillButton == null)
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 300));
            
            GUILayout.Label("=== 健康系统测试 ===");
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("伤害:", GUILayout.Width(50));
            if (float.TryParse(GUILayout.TextField(damageAmount.ToString(), GUILayout.Width(50)), out float newDamage))
            {
                damageAmount = newDamage;
            }
            if (GUILayout.Button("造成伤害", GUILayout.Width(80)))
            {
                TestDamage();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("治疗:", GUILayout.Width(50));
            if (float.TryParse(GUILayout.TextField(healAmount.ToString(), GUILayout.Width(50)), out float newHeal))
            {
                healAmount = newHeal;
            }
            if (GUILayout.Button("治疗", GUILayout.Width(80)))
            {
                TestHeal();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("技能:", GUILayout.Width(50));
            if (float.TryParse(GUILayout.TextField(skillCost.ToString(), GUILayout.Width(50)), out float newSkill))
            {
                skillCost = newSkill;
            }
            if (GUILayout.Button("消耗技能", GUILayout.Width(80)))
            {
                TestConsumeSkill();
            }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("重置到满值"))
            {
                TestResetAll();
            }
            
            GUILayout.Space(20);
            
            if (PlayerHealthSystem.instance != null)
            {
                GUILayout.Label(PlayerHealthSystem.instance.GetStatusInfo());
            }
            
            GUILayout.EndArea();
        }
    }
    
    #endregion
}