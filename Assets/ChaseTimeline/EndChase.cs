using UnityEngine;

public class EndChase : MonoBehaviour
{
    public AudioSource chaseAudio;
    public GameObject originalAudio;

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
            chaseAudio.enabled = false;
            originalAudio.SetActive(true);
        }
    }
}
