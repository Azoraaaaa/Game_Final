using UnityEngine;

public class ChaseStart : MonoBehaviour
{

    public GameObject bgmAS;
    public GameObject monster1;
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
        if(other.CompareTag("Player"))
        {
            bgmAS.SetActive(false);
            monster1.SetActive(true);
        }
    }
}
