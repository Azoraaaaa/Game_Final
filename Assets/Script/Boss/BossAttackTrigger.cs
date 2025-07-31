using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
    [Header("攻击设置")]
    [SerializeField] private AttackType attackType = AttackType.Claw;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float knockbackForce = 5f;
    
    [Header("特效")]
    [SerializeField] private GameObject hitEffectPrefab;
    
    // 攻击类型枚举
    public enum AttackType
    {
        Claw,
        Stinger,
        TidalRush
    }
    
    // 私有变量
    private BossController bossController;
    private bool canDealDamage = false;
    private float damageCooldown = 0.5f;
    private float lastDamageTime = 0f;
    
    void Start()
    {
        // 获取Boss控制器
        bossController = GetComponentInParent<BossController>();
        
        // 确保有碰撞器
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage || Time.time - lastDamageTime < damageCooldown) return;
        
        if (other.CompareTag("Player"))
        {
            DealDamage(other);
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (!canDealDamage || Time.time - lastDamageTime < damageCooldown) return;
        
        if (other.CompareTag("Player"))
        {
            DealDamage(other);
        }
    }
    
    private void DealDamage(Collider playerCollider)
    {
        // 对玩家造成伤害
        PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        
        // 击退效果
        Rigidbody playerRb = playerCollider.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 knockbackDirection = (playerCollider.transform.position - transform.position).normalized;
            knockbackDirection.y = 0.5f; // 添加一些向上的力
            
            switch (attackType)
            {
                case AttackType.Claw:
                    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                    break;
                case AttackType.Stinger:
                    playerRb.AddForce(knockbackDirection * knockbackForce * 1.5f, ForceMode.Impulse);
                    break;
                case AttackType.TidalRush:
                    playerRb.AddForce(knockbackDirection * knockbackForce * 2f + Vector3.up * 3f, ForceMode.Impulse);
                    break;
            }
        }
        
        // 播放命中特效
        PlayHitEffect(playerCollider.transform.position);
        
        // 更新最后伤害时间
        lastDamageTime = Time.time;
    }
    
    private void PlayHitEffect(Vector3 position)
    {
        // 生成命中特效
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
            Destroy(effect, 2f);
        }
        
        // TODO: 播放命中音效
        // AudioManager.Instance.PlaySound("hit_sound", position);
        
        // TODO: 播放命中粒子特效
        // Instantiate(hitParticlePrefab, position, Quaternion.identity);
    }
    
    // 公共方法，由Boss控制器调用
    public void EnableDamage()
    {
        canDealDamage = true;
    }
    
    public void DisableDamage()
    {
        canDealDamage = false;
    }
    
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    
    public void SetKnockbackForce(float newForce)
    {
        knockbackForce = newForce;
    }
    
    // 用于调试的Gizmos
    private void OnDrawGizmosSelected()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider)
            {
                BoxCollider boxCol = (BoxCollider)col;
                Gizmos.DrawWireCube(boxCol.center, boxCol.size);
            }
            else if (col is SphereCollider)
            {
                SphereCollider sphereCol = (SphereCollider)col;
                Gizmos.DrawWireSphere(sphereCol.center, sphereCol.radius);
            }
        }
    }
} 