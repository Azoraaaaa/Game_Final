using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [Header("弹体设置")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float damage = 30f;
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private ProjectileType projectileType = ProjectileType.Water;
    
    [Header("特效")]
    [SerializeField] private GameObject waterSplashPrefab;
    [SerializeField] private GameObject iceExplosionPrefab;
    [SerializeField] private GameObject slowEffectPrefab;
    
    [Header("冰团特殊效果")]
    [SerializeField] private float slowDuration = 3f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float explosionRadius = 3f;
    
    // 私有变量
    private Vector3 direction;
    private float currentSpeed;
    private float currentDamage;
    private bool hasHit = false;
    
    // 弹体类型枚举
    public enum ProjectileType
    {
        Water,
        Ice
    }
    
    void Start()
    {
        // 延迟销毁
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        if (!hasHit)
        {
            // 移动弹体
            transform.position += direction * currentSpeed * Time.deltaTime;
            
            // 旋转弹体（可选）
            transform.Rotate(Vector3.forward, 360f * Time.deltaTime);
        }
    }
    
    public void Initialize(Vector3 dir, float spd, float dmg, ProjectileType type)
    {
        direction = dir.normalized;
        currentSpeed = spd;
        currentDamage = dmg;
        projectileType = type;
        
        // 根据类型设置特殊属性
        switch (projectileType)
        {
            case ProjectileType.Water:
                // 水团：缓慢移动，命中时生成水花
                break;
            case ProjectileType.Ice:
                // 冰团：较慢移动，命中时爆炸并减速
                currentSpeed *= 0.8f;
                break;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        
        if (other.CompareTag("Player"))
        {
            PlayerHealthSystem.instance.TakeDamage(currentDamage);
            
            // 根据弹体类型产生不同效果
            switch (projectileType)
            {
                case ProjectileType.Water:
                    HandleWaterOrbHit(other.transform.position);
                    break;
                case ProjectileType.Ice:
                    HandleIceOrbHit(other.transform.position);
                    break;
            }
            
            hasHit = true;
            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment"))
        {
            // 击中环境物体
            switch (projectileType)
            {
                case ProjectileType.Water:
                    HandleWaterOrbHit(transform.position);
                    break;
                case ProjectileType.Ice:
                    HandleIceOrbHit(transform.position);
                    break;
            }
            
            hasHit = true;
            Destroy(gameObject);
        }
    }
    
    private void HandleWaterOrbHit(Vector3 hitPosition)
    {
        // 生成水花特效
        if (waterSplashPrefab != null)
        {
            GameObject splash = Instantiate(waterSplashPrefab, hitPosition, Quaternion.identity);
            
            // 延迟销毁特效
            Destroy(splash, 3f);
        }
        
        // TODO: 播放水花音效
        // AudioManager.Instance.PlaySound("water_splash", hitPosition);
        
        // TODO: 播放水花粒子特效
        // Instantiate(waterSplashParticles, hitPosition, Quaternion.identity);
    }
    
    private void HandleIceOrbHit(Vector3 hitPosition)
    {
        // 生成冰爆炸特效
        if (iceExplosionPrefab != null)
        {
            GameObject explosion = Instantiate(iceExplosionPrefab, hitPosition, Quaternion.identity);
            
            // 延迟销毁特效
            Destroy(explosion, 4f);
        }
        
        // TODO: 播放冰爆炸音效
        // AudioManager.Instance.PlaySound("ice_explosion", hitPosition);
        
        // TODO: 播放冰爆炸粒子特效
        // Instantiate(iceExplosionParticles, hitPosition, Quaternion.identity);
        
        // 范围减速效果
        ApplySlowEffect(hitPosition);
    }
    
    private void ApplySlowEffect(Vector3 center)
    {
        // 检测范围内的玩家
        Collider[] hitColliders = Physics.OverlapSphere(center, explosionRadius);
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // 应用减速效果
                PlayerMovement playerMovement = hitCollider.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.ApplySlowEffect(slowAmount, slowDuration);
                }
                
                // 生成减速特效
                if (slowEffectPrefab != null)
                {
                    GameObject slowEffect = Instantiate(slowEffectPrefab, hitCollider.transform.position, Quaternion.identity);
                    slowEffect.transform.SetParent(hitCollider.transform);
                    
                    // 延迟销毁特效
                    Destroy(slowEffect, slowDuration);
                }
                
                // TODO: 播放减速音效
                // AudioManager.Instance.PlaySound("slow_effect", hitCollider.transform.position);
            }
        }
    }
    
    // 用于调试的Gizmos
    private void OnDrawGizmosSelected()
    {
        if (projectileType == ProjectileType.Ice)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
} 