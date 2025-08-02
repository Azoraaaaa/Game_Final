using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public static SwordDamage instance;
    private void Awake()
    {
        instance = this;
    }

    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Hit enemy with fist!");
            }
        }
    }
}
