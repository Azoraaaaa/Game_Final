using UnityEngine;

public class TidalWaveDamage : MonoBehaviour
{
    public float damage = 10f;
    public GameObject collisionEffectPrefab; // ���ŵ���Ч��Ԥ���壩

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

            Destroy(gameObject); // ���ٺ��˶���
        }
    }
    }