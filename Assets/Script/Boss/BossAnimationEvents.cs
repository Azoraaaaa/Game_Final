using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{
    [Header("攻击触发器引用")]
    [SerializeField] private BossAttackTrigger leftClawTrigger;
    [SerializeField] private BossAttackTrigger rightClawTrigger;
    [SerializeField] private BossAttackTrigger stingerTrigger;
    [SerializeField] private BossAttackTrigger tidalRushTrigger;
    
    [Header("特效引用")]
    [SerializeField] private GameObject waterOrbSpawnEffect;
    [SerializeField] private GameObject iceOrbSpawnEffect;
    [SerializeField] private GameObject tidalRushEffect;
    
    private BossController bossController;
    
    void Start()
    {
        bossController = GetComponent<BossController>();
    }
    
    // 动画事件方法 - 左爪攻击开始
    public void OnLeftClawAttackStart()
    {
        if (leftClawTrigger != null)
        {
            leftClawTrigger.EnableDamage();
        }
        
        // TODO: 播放攻击音效
        // AudioManager.Instance.PlaySound("claw_attack_start", transform.position);
    }
    
    // 动画事件方法 - 左爪攻击结束
    public void OnLeftClawAttackEnd()
    {
        if (leftClawTrigger != null)
        {
            leftClawTrigger.DisableDamage();
        }
    }
    
    // 动画事件方法 - 右爪攻击开始
    public void OnRightClawAttackStart()
    {
        if (rightClawTrigger != null)
        {
            rightClawTrigger.EnableDamage();
        }
        
        // TODO: 播放攻击音效
        // AudioManager.Instance.PlaySound("claw_attack_start", transform.position);
    }
    
    // 动画事件方法 - 右爪攻击结束
    public void OnRightClawAttackEnd()
    {
        if (rightClawTrigger != null)
        {
            rightClawTrigger.DisableDamage();
        }
    }
    
    // 动画事件方法 - 连击攻击开始
    public void OnComboAttackStart()
    {
        if (leftClawTrigger != null)
        {
            leftClawTrigger.EnableDamage();
        }
        if (rightClawTrigger != null)
        {
            rightClawTrigger.EnableDamage();
        }
        
        // TODO: 播放连击音效
        // AudioManager.Instance.PlaySound("combo_attack_start", transform.position);
    }
    
    // 动画事件方法 - 连击攻击结束
    public void OnComboAttackEnd()
    {
        if (leftClawTrigger != null)
        {
            leftClawTrigger.DisableDamage();
        }
        if (rightClawTrigger != null)
        {
            rightClawTrigger.DisableDamage();
        }
    }
    
    // 动画事件方法 - 突刺攻击开始
    public void OnStingerAttackStart()
    {
        if (stingerTrigger != null)
        {
            stingerTrigger.EnableDamage();
        }
        
        // TODO: 播放突刺音效
        // AudioManager.Instance.PlaySound("stinger_attack_start", transform.position);
    }
    
    // 动画事件方法 - 突刺攻击结束
    public void OnStingerAttackEnd()
    {
        if (stingerTrigger != null)
        {
            stingerTrigger.DisableDamage();
        }
    }
    
    // 动画事件方法 - 海浪冲击开始
    public void OnTidalRushStart()
    {
        if (tidalRushTrigger != null)
        {
            tidalRushTrigger.EnableDamage();
        }
        
        // 生成海浪冲击特效
        if (tidalRushEffect != null)
        {
            Instantiate(tidalRushEffect, transform.position, transform.rotation);
        }
        
        // TODO: 播放海浪冲击音效
        // AudioManager.Instance.PlaySound("tidal_rush_start", transform.position);
    }
    
    // 动画事件方法 - 海浪冲击结束
    public void OnTidalRushEnd()
    {
        if (tidalRushTrigger != null)
        {
            tidalRushTrigger.DisableDamage();
        }
    }
    
    // 动画事件方法 - 水团生成
    public void OnWaterOrbSpawn()
    {
        // 生成水团生成特效
        if (waterOrbSpawnEffect != null)
        {
            Transform spawnPoint = bossController != null ? 
                bossController.ProjectileSpawnPoint : transform;
            
            if (spawnPoint != null)
            {
                Instantiate(waterOrbSpawnEffect, spawnPoint.position, spawnPoint.rotation);
            }
        }
        
        // TODO: 播放水团生成音效
        // AudioManager.Instance.PlaySound("water_orb_spawn", transform.position);
    }
    
    // 动画事件方法 - 冰团生成
    public void OnIceOrbSpawn()
    {
        // 生成冰团生成特效
        if (iceOrbSpawnEffect != null)
        {
            Transform spawnPoint = bossController != null ? 
                bossController.ProjectileSpawnPoint : transform;
            
            if (spawnPoint != null)
            {
                Instantiate(iceOrbSpawnEffect, spawnPoint.position, spawnPoint.rotation);
            }
        }
        
        // TODO: 播放冰团生成音效
        // AudioManager.Instance.PlaySound("ice_orb_spawn", transform.position);
    }
    
    // 动画事件方法 - 受击反应
    public void OnGetHit()
    {
        // TODO: 播放受击音效
        // AudioManager.Instance.PlaySound("boss_hit", transform.position);
        
        // TODO: 播放受击粒子特效
        // Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
    }
    
    // 动画事件方法 - 死亡开始
    public void OnDeathStart()
    {
        // TODO: 播放死亡音效
        // AudioManager.Instance.PlaySound("boss_death", transform.position);
        
        // TODO: 播放死亡粒子特效
        // Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
    }
    
    // 动画事件方法 - 脚步声
    public void OnFootstep()
    {
        // TODO: 播放脚步声
        // AudioManager.Instance.PlaySound("boss_footstep", transform.position);
    }
    
    // 动画事件方法 - 咆哮声
    public void OnRoar()
    {
        // TODO: 播放咆哮声
        // AudioManager.Instance.PlaySound("boss_roar", transform.position);
    }
} 