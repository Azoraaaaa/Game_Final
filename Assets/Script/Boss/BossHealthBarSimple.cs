using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Boss血条UI控制器（简化版）- 实时显示Boss血量
/// </summary>
public class BossHealthBarSimple : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI phaseText;
    [SerializeField] private Image fillImage;
    
    [Header("Boss引用")]
    [SerializeField] private BossController bossController;
    
    [Header("颜色设置")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private Color phase1Color = Color.green;
    [SerializeField] private Color phase2Color = Color.yellow;
    [SerializeField] private Color phase3Color = Color.red;
    
    // 私有变量
    private int lastPhase = 1;
    
    void Start()
    {
        InitializeHealthBar();
    }
    
    void Update()
    {
        if (bossController == null) return;
        
        UpdateHealthBar();
    }
    
    private void InitializeHealthBar()
    {
        // 自动查找Boss控制器
        if (bossController == null)
        {
            bossController = FindFirstObjectByType<BossController>();
        }
        
        // 初始化血条
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }
        
        // 更新阶段文本
        UpdatePhaseText(1);
    }
    
    private void UpdateHealthBar()
    {
        if (bossController == null) return;
        
        // 计算血量百分比
        float healthPercentage = bossController.CurrentHP / bossController.MaxHP;
        
        // 更新血条
        if (healthSlider != null)
        {
            healthSlider.value = healthPercentage;
        }
        
        // 更新血量文本
        if (healthText != null)
        {
            healthText.text = $"HP: {bossController.CurrentHP:F0}/{bossController.MaxHP:F0}";
        }
        
        // 更新血条颜色
        UpdateHealthBarColor(healthPercentage);
        
        // 检查阶段变化
        CheckPhaseChange();
        
        // 检查Boss是否死亡
        if (bossController.IsDead)
        {
            OnBossDeath();
        }
    }
    
    private void UpdateHealthBarColor(float healthPercentage)
    {
        if (fillImage == null) return;
        
        // 根据血量百分比插值颜色
        Color targetColor = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
        fillImage.color = targetColor;
    }
    
    private void CheckPhaseChange()
    {
        if (bossController == null) return;
        
        float healthPercentage = (bossController.CurrentHP / bossController.MaxHP) * 100f;
        int currentPhase = 1;
        
        if (healthPercentage > 75f)
        {
            currentPhase = 1;
        }
        else if (healthPercentage > 30f)
        {
            currentPhase = 2;
        }
        else
        {
            currentPhase = 3;
        }
        
        // 如果阶段发生变化
        if (currentPhase != lastPhase)
        {
            UpdatePhaseText(currentPhase);
            lastPhase = currentPhase;
        }
    }
    
    private void UpdatePhaseText(int phase)
    {
        if (phaseText == null) return;
        
        switch (phase)
        {
            case 1:
                phaseText.text = "第一阶段";
                phaseText.color = phase1Color;
                break;
            case 2:
                phaseText.text = "第二阶段";
                phaseText.color = phase2Color;
                break;
            case 3:
                phaseText.text = "第三阶段";
                phaseText.color = phase3Color;
                break;
        }
    }
    
    private void OnBossDeath()
    {
        // Boss死亡时的处理
        if (phaseText != null)
        {
            phaseText.text = "已击败";
            phaseText.color = Color.gray;
        }
        
        // 隐藏整个血条UI
        gameObject.SetActive(false);
    }
    
    // 公共方法，供外部调用
    public void SetBossController(BossController boss)
    {
        bossController = boss;
    }
} 