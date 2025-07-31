using UnityEngine;

public class Magic : MonoBehaviour
{
    public float magicTimer = 3f;
    float countDown;

    bool hasExploded = false;

    //public float giveDamage = 120f;
    //public float radius = 10f;

    //public GameObject explosionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = magicTimer;
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
        //Instantiate(explosionEffect, transform.position, transform.rotation);

        //get nearby objects
        //Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        /*
        foreach (Collider nearbyObject in colliders)
        {
            //damage
            Object obj = nearbyObject.GetComponent<Object>();//find the nearby object that has Object scripts

            if (obj != null)
            {
                obj.objectHitDamage(giveDamage);
            }
        }
        */
        Debug.Log("Explore!");

        Destroy(gameObject);
    }
}
