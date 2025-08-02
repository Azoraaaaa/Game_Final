using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float bombTimer = 3f;
    float countDown;

    bool hasExploded = false;

    public float giveDamage;
    public float radius = 10f;

    public GameObject explosionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = bombTimer;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;

        if (countDown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }
    void Explode()
    {
        //showEffect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //get nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);


        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                EnemyHealthController enemy = nearbyObject.GetComponent<EnemyHealthController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(giveDamage);
                }
            }
        }

        Debug.Log("Explore!");

        Destroy(gameObject);
    }
}
