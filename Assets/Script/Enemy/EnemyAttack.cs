using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("攻击设置")]
    [SerializeField] private float attackDamage = 10f; // 攻击伤害
    [SerializeField] private string playerTag = "Player"; // 玩家标签

    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞对象是否是玩家
        if (other.CompareTag(playerTag))
        {
            PlayerHealthSystem.instance.TakeDamage(attackDamage);
            Debug.Log($"对玩家造成 {attackDamage} 点伤害！");
        }
    }
}