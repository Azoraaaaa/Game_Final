using UnityEngine;
using System.Collections.Generic;

public class EnemyHealthController : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Damage Test Settings")]
    public Animator anim;
    public float testDamageAmount = 10f;
    public KeyCode testDamageKey = KeyCode.C;

    [Header("Death Settings")]
    public GameObject deathEffectPrefab;
    public float destroyDelay = 5f;

    [Header("Loot Drop Settings")]
    public List<GameObject> dropItemPrefabs; // 掉落物列表
    public float dropChance = 1f; // 0~1, 1=100%掉落

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(testDamageKey))
        {
            TakeDamage(testDamageAmount);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("isDead");
        Debug.Log($"{gameObject.name} has died.");

        Invoke(nameof(SpawnDeathEffect), destroyDelay - 0.2f); // 稍早一点播完再销毁（防止被删）

        TryDropItem();

        Destroy(gameObject, destroyDelay);
    }

    private void SpawnDeathEffect()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void TryDropItem()
    {
        float offset = 0f;
        foreach (GameObject item in dropItemPrefabs)
        {
            Vector3 dropPosition = transform.position + Vector3.up * 0.5f + Vector3.right * offset;
            Instantiate(item, dropPosition, Quaternion.identity);
            offset += 0.5f; // 横向间距
        }
    }

}
