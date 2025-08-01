using UnityEngine;

public class TestDamage : MonoBehaviour
{
    [Header("测试设置")]
    [SerializeField] private float testDamage = 10f;
    [SerializeField] private KeyCode damageKey = KeyCode.T;
    [SerializeField] private KeyCode healKey = KeyCode.Y;
    [SerializeField] private KeyCode resetKey = KeyCode.R;
    
    private void Update()
    {
        // 按 T 键造成伤害
        if (Input.GetKeyDown(damageKey))
        {
            TestTakeDamage();
        }
        
        // 按 Y 键治疗
        if (Input.GetKeyDown(healKey))
        {
            TestHeal();
        }
        
        // 按 R 键重置
        if (Input.GetKeyDown(resetKey))
        {
            TestReset();
        }
    }
    
    [ContextMenu("测试造成伤害")]
    public void TestTakeDamage()
    {
        if (PlayerHealthSystem.instance != null)
        {
            float actualDamage = PlayerHealthSystem.instance.TakeDamage(testDamage);
            Debug.Log($"测试：对玩家造成 {actualDamage} 点伤害！当前血量: {PlayerHealthSystem.instance.CurrentHealth}");
        }
        else
        {
            Debug.LogError("PlayerHealthSystem 实例不存在！");
        }
    }
    
    [ContextMenu("测试治疗")]
    public void TestHeal()
    {
        if (PlayerHealthSystem.instance != null)
        {
            float actualHeal = PlayerHealthSystem.instance.AddHealth(testDamage);
            Debug.Log($"测试：治疗玩家 {actualHeal} 点血量！当前血量: {PlayerHealthSystem.instance.CurrentHealth}");
        }
        else
        {
            Debug.LogError("PlayerHealthSystem 实例不存在！");
        }
    }
    
    [ContextMenu("测试重置")]
    public void TestReset()
    {
        if (PlayerHealthSystem.instance != null)
        {
            PlayerHealthSystem.instance.ResetToFull();
            Debug.Log($"测试：重置玩家血量和技能值！当前血量: {PlayerHealthSystem.instance.CurrentHealth}");
        }
        else
        {
            Debug.LogError("PlayerHealthSystem 实例不存在！");
        }
    }
} 