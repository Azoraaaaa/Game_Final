using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    [Header("Boss 基础设置")]
    [SerializeField] private float maxHP = 1000f;
    [SerializeField] private float currentHP;
    [SerializeField] private int currentPhase = 1;
    
    [Header("动画控制")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationTransitionTime = 0.1f;
    
    [Header("移动设置")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private Transform player;
    
    [Header("攻击设置")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float comboAttackCooldown = 1.5f;
    [SerializeField] private float specialAttackCooldown = 5f;
    [SerializeField] private float meleeDamage = 50f;
    [SerializeField] private float stingerDamage = 80f;
    
    [Header("远程攻击")]
    [SerializeField] private GameObject waterOrbPrefab;
    [SerializeField] private GameObject iceOrbPrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float waterOrbDamage = 30f;
    [SerializeField] private float iceOrbDamage = 40f;
    
    [Header("碰撞检测")]
    [SerializeField] private Transform leftClaw;
    [SerializeField] private Transform rightClaw;
    [SerializeField] private Transform stinger;
    [SerializeField] private Vector3 meleeHitboxSize = new Vector3(1f, 1f, 1f);
    
    // 公共属性，供其他脚本访问
    public Transform ProjectileSpawnPoint => projectileSpawnPoint;
    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public bool IsDead => isDead;
    
    [Header("特效")]
    [SerializeField] private GameObject waterSplashPrefab;
    [SerializeField] private GameObject iceExplosionPrefab;
    [SerializeField] private GameObject tidalRushPrefab;
    [SerializeField] private GameObject deathEffectPrefab;
    
    // 私有变量
    private bool isDead = false;
    private bool isAttacking = false;
    private bool isMoving = false;
    private bool isInSpecialAttack = false;
    private float lastAttackTime = 0f;
    private float lastComboTime = 0f;
    private float lastSpecialTime = 0f;
    private Vector3 patrolTarget;
    private BossState currentState = BossState.Idle;
    
    // 动画参数名称
    private const string IS_MOVING = "IsMoving";
    private const string IS_ATTACKING = "IsAttacking";
    private const string ATTACK_INDEX = "AttackIndex";
    private const string PHASE = "Phase";
    private const string IS_DEAD = "IsDead";
    private const string GET_HIT = "GetHit";
    private const string DO_SPECIAL_ATTACK = "DoSpecialAttack";
    
    // Boss状态枚举
    private enum BossState
    {
        Idle,
        Patrolling,
        Attacking,
        SpecialAttacking,
        GettingHit,
        Dead
    }
    
    void Start()
    {
        InitializeBoss();
    }
    
    void Update()
    {
        if (isDead) 
        {
            // 死亡时只更新动画状态，确保死亡动画正确播放
            UpdateDeathAnimations();
            return;
        }
        
        UpdatePhase();
        UpdateState();
        UpdateAnimations();
    }
    
    private void InitializeBoss()
    {
        currentHP = maxHP;
        currentPhase = 1;
        isDead = false;
        isAttacking = false;
        isMoving = false;
        
        // 设置初始动画参数
        animator.SetBool(IS_DEAD, false);
        animator.SetInteger(PHASE, currentPhase);
        animator.SetBool(IS_MOVING, false);
        animator.SetBool(IS_ATTACKING, false);
        
        // 设置巡逻目标
        SetNewPatrolTarget();
        
        // 开始状态机
        StartCoroutine(BossStateMachine());
    }
    
    private void UpdatePhase()
    {
        float healthPercentage = (currentHP / maxHP) * 100f;
        
        if (healthPercentage > 75f && currentPhase != 1)
        {
            currentPhase = 1;
            animator.SetInteger(PHASE, currentPhase);
        }
        else if (healthPercentage <= 75f && healthPercentage > 30f && currentPhase != 2)
        {
            currentPhase = 2;
            animator.SetInteger(PHASE, currentPhase);
        }
        else if (healthPercentage <= 30f && currentPhase != 3)
        {
            currentPhase = 3;
            animator.SetInteger(PHASE, currentPhase);
        }
    }
    
    private void UpdateState()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        switch (currentState)
        {
            case BossState.Idle:
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = BossState.Attacking;
                }
                else
                {
                    currentState = BossState.Patrolling;
                }
                break;
                
            case BossState.Patrolling:
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = BossState.Attacking;
                }
                else
                {
                    Patrol();
                }
                break;
                
            case BossState.Attacking:
                if (distanceToPlayer > detectionRange)
                {
                    currentState = BossState.Patrolling;
                }
                else
                {
                    AttackPlayer();
                }
                break;
        }
    }
    
    private void UpdateAnimations()
    {
        animator.SetBool(IS_MOVING, isMoving);
        animator.SetBool(IS_ATTACKING, isAttacking);
    }
    
    private void UpdateDeathAnimations()
    {
        // 死亡时停止所有移动和攻击动画
        animator.SetBool(IS_MOVING, false);
        animator.SetBool(IS_ATTACKING, false);
        animator.SetBool(IS_DEAD, true);
    }
    
    private void Patrol()
    {
        isMoving = true;
        isAttacking = false;
        
        // 检查是否到达巡逻目标
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            SetNewPatrolTarget();
        }
        
        // 移动向巡逻目标
        Vector3 direction = (patrolTarget - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;
        transform.LookAt(patrolTarget);
    }
    
    private void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection.y = 0f;
        patrolTarget = transform.position + randomDirection;
    }
    
    private void AttackPlayer()
    {
        isMoving = false;
        
        if (Time.time - lastAttackTime < attackCooldown) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // 面向玩家
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0f;
        transform.LookAt(transform.position + directionToPlayer);
        
        // 根据阶段选择攻击方式
        switch (currentPhase)
        {
            case 1:
                Phase1Attack(distanceToPlayer);
                break;
            case 2:
                Phase2Attack(distanceToPlayer);
                break;
            case 3:
                Phase3Attack(distanceToPlayer);
                break;
        }
    }
    
    private void Phase1Attack(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackRange)
        {
            // 近战攻击
            int attackType = Random.Range(0, 2);
            animator.SetInteger(ATTACK_INDEX, attackType);
            animator.SetTrigger(IS_ATTACKING);
            
            StartCoroutine(PerformMeleeAttack(attackType == 0 ? "ClawAttackLeft" : "ClawAttackRight"));
        }
        else
        {
            // 移动到攻击范围
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * patrolSpeed * Time.deltaTime;
            isMoving = true;
        }
    }
    
    private void Phase2Attack(float distanceToPlayer)
    {
        if (Time.time - lastComboTime < comboAttackCooldown) return;
        
        int attackChoice = Random.Range(0, 4);
        
        switch (attackChoice)
        {
            case 0:
                // 连击攻击
                animator.SetInteger(ATTACK_INDEX, Random.Range(0, 2));
                animator.SetTrigger(IS_ATTACKING);
                StartCoroutine(PerformComboAttack());
                break;
            case 1:
                // 冰团攻击
                StartCoroutine(PerformIceOrbAttack());
                break;
            case 2:
                // 水团攻击
                StartCoroutine(PerformWaterOrbAttack());
                break;
            case 3:
                // 突刺攻击
                if (distanceToPlayer <= attackRange * 1.5f)
                {
                    animator.SetInteger(ATTACK_INDEX, 2);
                    animator.SetTrigger(IS_ATTACKING);
                    StartCoroutine(PerformStingerAttack());
                }
                break;
        }
        
        lastComboTime = Time.time;
    }
    
    private void Phase3Attack(float distanceToPlayer)
    {
        if (Time.time - lastSpecialTime < specialAttackCooldown) return;
        
        // 海浪冲击攻击
        StartCoroutine(PerformTidalRushAttack());
        lastSpecialTime = Time.time;
    }
    
    private IEnumerator PerformMeleeAttack(string attackName)
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        
        // 等待动画播放
        yield return new WaitForSeconds(0.5f);
        
        // 检测近战攻击
        CheckMeleeHit();
        
        // 等待动画完成
        yield return new WaitForSeconds(1f);
        
        isAttacking = false;
    }
    
    private IEnumerator PerformComboAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        
        // 播放连击动画
        yield return new WaitForSeconds(0.3f);
        
        // 检测连击攻击
        CheckMeleeHit();
        
        yield return new WaitForSeconds(0.5f);
        CheckMeleeHit();
        
        yield return new WaitForSeconds(1.2f);
        
        isAttacking = false;
    }
    
    private IEnumerator PerformStingerAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        
        yield return new WaitForSeconds(0.8f);
        
        // 检测突刺攻击
        CheckStingerHit();
        
        yield return new WaitForSeconds(1.2f);
        
        isAttacking = false;
    }
    
    private IEnumerator PerformWaterOrbAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        
        // 生成水团
        if (waterOrbPrefab != null && projectileSpawnPoint != null)
        {
            GameObject waterOrb = Instantiate(waterOrbPrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Projectile projectile = waterOrb.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector3 direction = (player.position - projectileSpawnPoint.position).normalized;
                projectile.Initialize(direction, projectileSpeed, waterOrbDamage, Projectile.ProjectileType.Water);
            }
        }
        
        yield return new WaitForSeconds(1f);
        
        isAttacking = false;
    }
    
    private IEnumerator PerformIceOrbAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        
        // 生成冰团
        if (iceOrbPrefab != null && projectileSpawnPoint != null)
        {
            GameObject iceOrb = Instantiate(iceOrbPrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Projectile projectile = iceOrb.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector3 direction = (player.position - projectileSpawnPoint.position).normalized;
                projectile.Initialize(direction, projectileSpeed * 0.8f, iceOrbDamage, Projectile.ProjectileType.Ice);
            }
        }
        
        yield return new WaitForSeconds(1f);
        
        isAttacking = false;
    }
    
    private IEnumerator PerformTidalRushAttack()
    {
        isAttacking = true;
        isInSpecialAttack = true;
        lastAttackTime = Time.time;
        
        // 撤退
        animator.SetBool(IS_MOVING, true);
        Vector3 retreatPosition = transform.position - transform.forward * 5f;
        
        while (Vector3.Distance(transform.position, retreatPosition) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, retreatPosition, patrolSpeed * 2f * Time.deltaTime);
            yield return null;
        }
        
        // 播放海浪冲击动画
        animator.SetTrigger(DO_SPECIAL_ATTACK);
        animator.SetBool(IS_MOVING, false);
        
        yield return new WaitForSeconds(0.5f);
        
        // 向前冲击
        Vector3 rushTarget = transform.position + transform.forward * 15f;
        float rushSpeed = 15f;
        
        while (Vector3.Distance(transform.position, rushTarget) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rushTarget, rushSpeed * Time.deltaTime);
            
            // 检测冲击攻击
            CheckTidalRushHit();
            
            yield return null;
        }
        
        // 生成海浪特效
        if (tidalRushPrefab != null)
        {
            Instantiate(tidalRushPrefab, transform.position, transform.rotation);
        }
        
        yield return new WaitForSeconds(1f);
        
        isAttacking = false;
        isInSpecialAttack = false;
    }
    
    private void CheckMeleeHit()
    {
        // 使用OverlapBox检测近战攻击
        Collider[] hitColliders = Physics.OverlapBox(
            transform.position + transform.forward * attackRange * 0.5f,
            meleeHitboxSize * 0.5f,
            transform.rotation
        );
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // 对玩家造成伤害
                PlayerHealthSystem.instance.TakeDamage(meleeDamage);
                
                // 播放命中特效
                PlayHitEffect(hitCollider.transform.position);
                break;
            }
        }
    }
    
    private void CheckStingerHit()
    {
        // 检测突刺攻击
        Collider[] hitColliders = Physics.OverlapBox(
            stinger.position,
            meleeHitboxSize * 0.3f,
            stinger.rotation
        );
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealthSystem.instance.TakeDamage(stingerDamage);
                
                PlayHitEffect(hitCollider.transform.position);
                break;
            }
        }
    }
    
    private void CheckTidalRushHit()
    {
        // 检测海浪冲击攻击
        Collider[] hitColliders = Physics.OverlapBox(
            transform.position + transform.forward * 2f,
            new Vector3(3f, 2f, 2f),
            transform.rotation
        );
        
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealthSystem.instance.TakeDamage(meleeDamage * 1.5f);
                
                // 击飞效果
                Rigidbody playerRb = hitCollider.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.AddForce(transform.forward * 10f + Vector3.up * 5f, ForceMode.Impulse);
                }
                
                PlayHitEffect(hitCollider.transform.position);
                break;
            }
        }
    }
    
    private void PlayHitEffect(Vector3 position)
    {
        // TODO: 播放命中音效
        // AudioManager.Instance.PlaySound("hit_sound", position);
        
        // TODO: 播放命中粒子特效
        // Instantiate(hitEffectPrefab, position, Quaternion.identity);
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        currentHP -= damage;
        
        // 播放受击动画
        animator.SetTrigger(GET_HIT);
        
        // TODO: 播放受击音效
        // AudioManager.Instance.PlaySound("boss_hit", transform.position);
        
        // TODO: 播放受击粒子特效
        // Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        
        if (currentHP <= 0f)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isDead = true;
        currentState = BossState.Dead;
        
        // 播放死亡动画
        animator.SetBool(IS_DEAD, true);
        animator.SetTrigger(GET_HIT);
        
        // 生成死亡特效
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        
        // TODO: 播放死亡音效
        // AudioManager.Instance.PlaySound("boss_death", transform.position);
        
        // 禁用碰撞器
        Collider bossCollider = GetComponent<Collider>();
        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }
        
        // 停止所有正在运行的协程
        StopAllCoroutines();
        
        // 停止所有移动和攻击状态
        isMoving = false;
        isAttacking = false;
        isInSpecialAttack = false;
        
        // 禁用所有攻击触发器
        DisableAllAttackTriggers();
        
        // 立即销毁Boss
        Destroy(gameObject, 5f);
        
        Debug.Log("Boss已死亡，将在2秒后销毁");
    }
    
    private IEnumerator BossStateMachine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.1f);
            StoryManager.instance.CheckEnd();
        }
    }
    
    private void DisableAllAttackTriggers()
    {
        // 禁用所有攻击触发器
        BossAttackTrigger[] attackTriggers = GetComponentsInChildren<BossAttackTrigger>();
        foreach (BossAttackTrigger trigger in attackTriggers)
        {
            if (trigger != null)
            {
                trigger.DisableDamage();
            }
        }
    }
    
    // 用于调试的Gizmos
    private void OnDrawGizmosSelected()
    {
        // 攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward * attackRange * 0.5f, meleeHitboxSize);
        
        // 检测范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // 巡逻目标
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(patrolTarget, 0.5f);
    }
} 