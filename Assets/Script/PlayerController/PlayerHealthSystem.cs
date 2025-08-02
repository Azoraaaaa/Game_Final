using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealthSystem : MonoBehaviour
{
    public static PlayerHealthSystem instance;
    
    [Header("健康设置")]
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth;
    
    [Header("技能设置")]
    [SerializeField] public float maxSkillPoints = 100f;
    [SerializeField] public float currentSkillPoints;
    
    [Header("技能值恢复设置")]
    [SerializeField] private float skillRegenRate = 5f; // 每秒恢复的技能值
    [SerializeField] private bool autoRegenSkill = true;
    
    // 事件系统 - 供UI订阅
    public event Action<float, float> OnHealthChanged; // (当前血量, 最大血量)
    public event Action<float, float> OnSkillChanged;  // (当前技能值, 最大技能值)
    public event Action OnPlayerDeath;
    
    // 属性访问器
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentSkillPoints => currentSkillPoints;
    public float MaxSkillPoints => maxSkillPoints;
    public float HealthPercentage => maxHealth > 0 ? currentHealth / maxHealth : 0;
    public float SkillPercentage => maxSkillPoints > 0 ? currentSkillPoints / maxSkillPoints : 0;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        // 单例模式
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 确保不被销毁
        }
        else
        {
            // 只销毁这个组件，而不是整个游戏对象
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        // 初始化血量和技能值为最大值
        currentHealth = maxHealth;
        currentSkillPoints = maxSkillPoints;
        
        // 触发初始事件
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnSkillChanged?.Invoke(currentSkillPoints, maxSkillPoints);

        OnPlayerDeath += HandleDeath;
    }

    private void Update()
    {
        // 自动恢复技能值
        if (autoRegenSkill && currentSkillPoints < maxSkillPoints)
        {
            RegenerateSkill(skillRegenRate * Time.deltaTime);
        }
    }

    private void HandleDeath()
    {

        currentHealth = maxHealth;
        currentSkillPoints = maxSkillPoints;


        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnSkillChanged?.Invoke(currentSkillPoints, maxSkillPoints);


        string cpKey = SceneManager.GetActiveScene().name + "_cp";
        string cpName = PlayerPrefs.GetString(cpKey, "");

        CheckPoint[] checkpoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);
        foreach (var cp in checkpoints)
        {
            if (cp.cpName == cpName)
            {
                CharacterController controller = GetComponent<CharacterController>();
                if (controller) controller.enabled = false;

                transform.position = cp.transform.position;
                transform.rotation = cp.transform.rotation;

                if (controller) controller.enabled = true;

                break;
            }
        }
    }


    #region 血量管理方法

    /// <summary>
    /// 增加血量
    /// </summary>
    /// <param name="amount">增加的血量</param>
    /// <returns>实际增加的血量</returns>
    public float AddHealth(float amount)
    {
        if (amount <= 0) return 0;
        
        float oldHealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        float actualAdded = currentHealth - oldHealth;
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        return actualAdded;
    }
    
    /// <summary>
    /// 减少血量（受到伤害）
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <returns>实际造成的伤害</returns>
    public float TakeDamage(float damage)
    {
        if (damage <= 0 || IsDead) return 0;
        
        float oldHealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        float actualDamage = oldHealth - currentHealth;
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // 检查是否死亡
        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
        
        return actualDamage;
    }
    
    /// <summary>
    /// 治疗到满血
    /// </summary>
    public void FullHeal()
    {
        AddHealth(maxHealth);
    }
    
    /// <summary>
    /// 设置最大血量
    /// </summary>
    /// <param name="newMaxHealth">新的最大血量</param>
    /// <param name="adjustCurrentHealth">是否同时调整当前血量</param>
    public void SetMaxHealth(float newMaxHealth, bool adjustCurrentHealth = false)
    {
        if (newMaxHealth <= 0) return;
        
        float oldMaxHealth = maxHealth;
        maxHealth = newMaxHealth;
        
        if (adjustCurrentHealth)
        {
            float ratio = currentHealth / oldMaxHealth;
            currentHealth = maxHealth * ratio;
        }
        else
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    #endregion
    
    #region 技能值管理方法
    
    /// <summary>
    /// 增加技能值
    /// </summary>
    /// <param name="amount">增加的技能值</param>
    /// <returns>实际增加的技能值</returns>
    public float AddSkillPoints(float amount)
    {
        if (amount <= 0) return 0;
        
        float oldSkill = currentSkillPoints;
        currentSkillPoints = Mathf.Clamp(currentSkillPoints + amount, 0, maxSkillPoints);
        float actualAdded = currentSkillPoints - oldSkill;
        
        OnSkillChanged?.Invoke(currentSkillPoints, maxSkillPoints);
        
        return actualAdded;
    }
    
    /// <summary>
    /// 消耗技能值
    /// </summary>
    /// <param name="cost">消耗的技能值</param>
    /// <returns>是否成功消耗</returns>
    public bool ConsumeSkillPoints(float cost)
    {
        if (cost <= 0) return true;
        if (currentSkillPoints < cost) return false;
        
        currentSkillPoints = Mathf.Clamp(currentSkillPoints - cost, 0, maxSkillPoints);
        OnSkillChanged?.Invoke(currentSkillPoints, maxSkillPoints);
        
        return true;
    }
    
    /// <summary>
    /// 检查是否有足够的技能值
    /// </summary>
    /// <param name="cost">需要检查的技能值</param>
    /// <returns>是否有足够的技能值</returns>
    public bool HasEnoughSkillPoints(float cost)
    {
        return currentSkillPoints >= cost;
    }
    
    /// <summary>
    /// 恢复技能值
    /// </summary>
    /// <param name="amount">恢复的技能值</param>
    private void RegenerateSkill(float amount)
    {
        AddSkillPoints(amount);
    }
    
    /// <summary>
    /// 技能值回满
    /// </summary>
    public void RestoreFullSkill()
    {
        AddSkillPoints(maxSkillPoints);
    }
    
    /// <summary>
    /// 设置最大技能值
    /// </summary>
    /// <param name="newMaxSkill">新的最大技能值</param>
    /// <param name="adjustCurrentSkill">是否同时调整当前技能值</param>
    public void SetMaxSkillPoints(float newMaxSkill, bool adjustCurrentSkill = false)
    {
        if (newMaxSkill <= 0) return;
        
        float oldMaxSkill = maxSkillPoints;
        maxSkillPoints = newMaxSkill;
        
        if (adjustCurrentSkill)
        {
            float ratio = currentSkillPoints / oldMaxSkill;
            currentSkillPoints = maxSkillPoints * ratio;
        }
        else
        {
            currentSkillPoints = Mathf.Clamp(currentSkillPoints, 0, maxSkillPoints);
        }
        
        OnSkillChanged?.Invoke(currentSkillPoints, maxSkillPoints);
    }
    
    #endregion
    
    #region 调试方法
    
    /// <summary>
    /// 重置血量和技能值到最大值
    /// </summary>
    public void ResetToFull()
    {
        FullHeal();
        RestoreFullSkill();
    }
    
    /// <summary>
    /// 获取状态信息（用于调试）
    /// </summary>
    /// <returns>状态信息字符串</returns>
    public string GetStatusInfo()
    {
        return $"血量: {currentHealth:F1}/{maxHealth:F1} ({HealthPercentage:P1})\n" +
               $"技能: {currentSkillPoints:F1}/{maxSkillPoints:F1} ({SkillPercentage:P1})\n" +
               $"状态: {(IsDead ? "死亡" : "存活")}";
    }
    
    #endregion
}