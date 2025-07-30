using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [Header("血量UI引用")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBackgroundFill; // 用于显示损失的血量
    
    [Header("技能值UI引用")]
    [SerializeField] private Slider skillSlider;
    [SerializeField] private Image skillFill;
    [SerializeField] private TextMeshProUGUI skillText;
    
    [Header("颜色设置")]
    [SerializeField] private Color healthColor = Color.red;
    [SerializeField] private Color skillColor = Color.blue;
    [SerializeField] private Color lowHealthColor = Color.yellow;
    [SerializeField] private Color criticalHealthColor = Color.red;
    [SerializeField] private Color backgroundHealthColor = new Color(1f, 1f, 1f, 0.3f);
    
    [Header("动画设置")]
    [SerializeField] private float smoothSpeed = 2f;
    [SerializeField] private float backgroundFadeSpeed = 1f;
    [SerializeField] private bool smoothTransition = true;
    
    [Header("阈值设置")]
    [SerializeField] private float lowHealthThreshold = 0.3f; // 30%以下显示黄色
    [SerializeField] private float criticalHealthThreshold = 0.15f; // 15%以下显示红色
    
    [Header("文本格式")]
    [SerializeField] private bool showExactNumbers = true;
    [SerializeField] private bool showPercentage = false;
    
    // 私有变量
    private float targetHealthValue;
    private float targetSkillValue;
    private float backgroundHealthValue;
    private Coroutine healthBackgroundCoroutine;

    private void Start()
    {
        // 订阅健康系统事件
        if (PlayerHealthSystem.instance != null)
        {
            PlayerHealthSystem.instance.OnHealthChanged += UpdateHealthDisplay;
            PlayerHealthSystem.instance.OnSkillChanged += UpdateSkillDisplay;
            PlayerHealthSystem.instance.OnPlayerDeath += OnPlayerDeath;
            
            // 初始化显示
            InitializeDisplay();
        }
        else
        {
            Debug.LogWarning("PlayerHealthSystem实例未找到！请确保PlayerHealthSystem脚本已添加到场景中。");
        }
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        if (PlayerHealthSystem.instance != null)
        {
            PlayerHealthSystem.instance.OnHealthChanged -= UpdateHealthDisplay;
            PlayerHealthSystem.instance.OnSkillChanged -= UpdateSkillDisplay;
            PlayerHealthSystem.instance.OnPlayerDeath -= OnPlayerDeath;
        }
    }

    private void Update()
    {
        // 平滑动画更新
        if (smoothTransition)
        {
            UpdateSmoothTransitions();
        }
    }

    /// <summary>
    /// 初始化显示
    /// </summary>
    private void InitializeDisplay()
    {
        var healthSystem = PlayerHealthSystem.instance;
        if (healthSystem != null)
        {
            UpdateHealthDisplay(healthSystem.CurrentHealth, healthSystem.MaxHealth);
            UpdateSkillDisplay(healthSystem.CurrentSkillPoints, healthSystem.MaxSkillPoints);
        }
    }

    /// <summary>
    /// 更新血量显示
    /// </summary>
    /// <param name="currentHealth">当前血量</param>
    /// <param name="maxHealth">最大血量</param>
    private void UpdateHealthDisplay(float currentHealth, float maxHealth)
    {
        float healthPercentage = maxHealth > 0 ? currentHealth / maxHealth : 0;
        
        if (smoothTransition)
        {
            targetHealthValue = healthPercentage;
            
            // 如果血量减少，启动背景血量条动画
            if (healthPercentage < (healthSlider ? healthSlider.value : 1f))
            {
                if (healthBackgroundCoroutine != null)
                {
                    StopCoroutine(healthBackgroundCoroutine);
                }
                healthBackgroundCoroutine = StartCoroutine(AnimateBackgroundHealth());
            }
        }
        else
        {
            SetHealthSliderValue(healthPercentage);
        }
        
        // 更新文本
        UpdateHealthText(currentHealth, maxHealth, healthPercentage);
        
        // 更新颜色
        UpdateHealthColor(healthPercentage);
    }

    /// <summary>
    /// 更新技能值显示
    /// </summary>
    /// <param name="currentSkill">当前技能值</param>
    /// <param name="maxSkill">最大技能值</param>
    private void UpdateSkillDisplay(float currentSkill, float maxSkill)
    {
        float skillPercentage = maxSkill > 0 ? currentSkill / maxSkill : 0;
        
        if (smoothTransition)
        {
            targetSkillValue = skillPercentage;
        }
        else
        {
            SetSkillSliderValue(skillPercentage);
        }
        
        // 更新文本
        UpdateSkillText(currentSkill, maxSkill, skillPercentage);
    }

    /// <summary>
    /// 平滑过渡更新
    /// </summary>
    private void UpdateSmoothTransitions()
    {
        // 血量平滑更新
        if (healthSlider != null && Mathf.Abs(healthSlider.value - targetHealthValue) > 0.01f)
        {
            float newValue = Mathf.Lerp(healthSlider.value, targetHealthValue, smoothSpeed * Time.deltaTime);
            SetHealthSliderValue(newValue);
        }
        
        // 技能值平滑更新
        if (skillSlider != null && Mathf.Abs(skillSlider.value - targetSkillValue) > 0.01f)
        {
            float newValue = Mathf.Lerp(skillSlider.value, targetSkillValue, smoothSpeed * Time.deltaTime);
            SetSkillSliderValue(newValue);
        }
    }

    /// <summary>
    /// 背景血量条动画
    /// </summary>
    private IEnumerator AnimateBackgroundHealth()
    {
        if (healthBackgroundFill != null && healthSlider != null)
        {
            backgroundHealthValue = healthSlider.value;
            yield return new WaitForSeconds(0.5f); // 延迟0.5秒开始消失
            
            while (backgroundHealthValue > targetHealthValue)
            {
                backgroundHealthValue = Mathf.Lerp(backgroundHealthValue, targetHealthValue, backgroundFadeSpeed * Time.deltaTime);
                healthBackgroundFill.fillAmount = backgroundHealthValue;
                yield return null;
            }
            
            healthBackgroundFill.fillAmount = targetHealthValue;
        }
    }

    /// <summary>
    /// 设置血量滑动条值
    /// </summary>
    private void SetHealthSliderValue(float value)
    {
        if (healthSlider != null)
        {
            healthSlider.value = value;
        }
    }

    /// <summary>
    /// 设置技能值滑动条值
    /// </summary>
    private void SetSkillSliderValue(float value)
    {
        if (skillSlider != null)
        {
            skillSlider.value = value;
        }
    }

    /// <summary>
    /// 更新血量文本
    /// </summary>
    private void UpdateHealthText(float current, float max, float percentage)
    {
        if (healthText != null)
        {
            if (showExactNumbers && showPercentage)
            {
                healthText.text = $"{current:F0}/{max:F0} ({percentage:P0})";
            }
            else if (showExactNumbers)
            {
                healthText.text = $"{current:F0}/{max:F0}";
            }
            else if (showPercentage)
            {
                healthText.text = $"{percentage:P0}";
            }
            else
            {
                healthText.text = "";
            }
        }
    }

    /// <summary>
    /// 更新技能值文本
    /// </summary>
    private void UpdateSkillText(float current, float max, float percentage)
    {
        if (skillText != null)
        {
            if (showExactNumbers && showPercentage)
            {
                skillText.text = $"{current:F0}/{max:F0} ({percentage:P0})";
            }
            else if (showExactNumbers)
            {
                skillText.text = $"{current:F0}/{max:F0}";
            }
            else if (showPercentage)
            {
                skillText.text = $"{percentage:P0}";
            }
            else
            {
                skillText.text = "";
            }
        }
    }

    /// <summary>
    /// 更新血量颜色
    /// </summary>
    private void UpdateHealthColor(float percentage)
    {
        if (healthFill != null)
        {
            Color targetColor;
            
            if (percentage <= criticalHealthThreshold)
            {
                targetColor = criticalHealthColor;
            }
            else if (percentage <= lowHealthThreshold)
            {
                targetColor = lowHealthColor;
            }
            else
            {
                targetColor = healthColor;
            }
            
            healthFill.color = targetColor;
        }
        
        // 设置背景血量条颜色
        if (healthBackgroundFill != null)
        {
            healthBackgroundFill.color = backgroundHealthColor;
        }
    }

    /// <summary>
    /// 玩家死亡事件处理
    /// </summary>
    private void OnPlayerDeath()
    {
        Debug.Log("玩家死亡！");
        // 这里可以添加死亡特效、音效等
        
        // 可以触发游戏结束UI或重生UI
        if (UIManager.instance != null)
        {
            // 假设UIManager有显示死亡通知的方法
            // UIManager.instance.ShowDeathNotification();
        }
    }

    #region 公共接口方法
    
    /// <summary>
    /// 设置血量条颜色
    /// </summary>
    /// <param name="normal">正常血量颜色</param>
    /// <param name="low">低血量颜色</param>
    /// <param name="critical">危险血量颜色</param>
    public void SetHealthColors(Color normal, Color low, Color critical)
    {
        healthColor = normal;
        lowHealthColor = low;
        criticalHealthColor = critical;
        
        // 立即更新当前颜色
        if (PlayerHealthSystem.instance != null)
        {
            UpdateHealthColor(PlayerHealthSystem.instance.HealthPercentage);
        }
    }
    
    /// <summary>
    /// 设置技能条颜色
    /// </summary>
    /// <param name="color">技能条颜色</param>
    public void SetSkillColor(Color color)
    {
        skillColor = color;
        if (skillFill != null)
        {
            skillFill.color = skillColor;
        }
    }
    
    /// <summary>
    /// 设置是否显示精确数字
    /// </summary>
    /// <param name="show">是否显示</param>
    public void SetShowExactNumbers(bool show)
    {
        showExactNumbers = show;
        // 立即更新显示
        if (PlayerHealthSystem.instance != null)
        {
            var healthSystem = PlayerHealthSystem.instance;
            UpdateHealthText(healthSystem.CurrentHealth, healthSystem.MaxHealth, healthSystem.HealthPercentage);
            UpdateSkillText(healthSystem.CurrentSkillPoints, healthSystem.MaxSkillPoints, healthSystem.SkillPercentage);
        }
    }
    
    /// <summary>
    /// 设置是否显示百分比
    /// </summary>
    /// <param name="show">是否显示</param>
    public void SetShowPercentage(bool show)
    {
        showPercentage = show;
        // 立即更新显示
        if (PlayerHealthSystem.instance != null)
        {
            var healthSystem = PlayerHealthSystem.instance;
            UpdateHealthText(healthSystem.CurrentHealth, healthSystem.MaxHealth, healthSystem.HealthPercentage);
            UpdateSkillText(healthSystem.CurrentSkillPoints, healthSystem.MaxSkillPoints, healthSystem.SkillPercentage);
        }
    }
    
    #endregion
}