using UnityEngine;

public class TidalWaveDamage : MonoBehaviour
{
    public float damage = 10f;
    public GameObject collisionEffectPrefab; // 播放的特效（预制体）

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Tidal wave hit the player!");
            PlayerHealthSystem.instance.TakeDamage(damage);
        }
        else if (other.CompareTag("Environment"))
        {
            Debug.Log("Tidal wave hit the environment!");

            if (collisionEffectPrefab != null)
            {
                Instantiate(collisionEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // 销毁海浪对象
        }
    }
    }