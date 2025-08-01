using UnityEngine;

public class Anim2 : MonoBehaviour
{
    public GameObject originalMonster;
    public GameObject monster;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            originalMonster.SetActive(false);
            monster.SetActive(true);
        }
    }
}
