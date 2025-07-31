using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("健康设置")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [Header("UI显示")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;
    
    [Header("受击效果")]
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material hitMaterial;
    
    // 私有变量
    private bool isInvincible = false;
    private Renderer playerRenderer;
    private bool isDead = false;
    
    void Start()
    {
        currentHealth = maxHealth;
        playerRenderer = GetComponent<Renderer>();
        
        UpdateHealthUI();
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(0f, currentHealth);
        
        UpdateHealthUI();
        
        // 播放受击效果
        StartCoroutine(HitEffect());
        
        // TODO: 播放受击音效
        // AudioManager.Instance.PlaySound("player_hit", transform.position);
        
        // TODO: 播放受击粒子特效
        // Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    private System.Collections.IEnumerator HitEffect()
    {
        isInvincible = true;
        
        // 闪烁效果
        if (playerRenderer != null && hitMaterial != null)
        {
            Material originalMaterial = playerRenderer.material;
            
            for (int i = 0; i < 5; i++)
            {
                playerRenderer.material = hitMaterial;
                yield return new WaitForSeconds(0.1f);
                playerRenderer.material = originalMaterial;
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        isInvincible = false;
    }
    
    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth:F0}/{maxHealth:F0}";
        }
    }
    
    private void Die()
    {
        isDead = true;
        
        // TODO: 播放死亡音效
        // AudioManager.Instance.PlaySound("player_death", transform.position);
        
        // TODO: 播放死亡粒子特效
        // Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        
        // 禁用玩家控制
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        
        // 显示游戏结束UI
        GameManager.Instance?.GameOver();
    }
    
    public void Heal(float healAmount)
    {
        if (isDead) return;
        
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        
        UpdateHealthUI();
        
        // TODO: 播放治疗音效
        // AudioManager.Instance.PlaySound("heal", transform.position);
    }
    
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    
    public bool IsDead()
    {
        return isDead;
    }
} 