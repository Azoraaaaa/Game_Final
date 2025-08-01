using UnityEngine;
using System.Collections;

public class Anim1 : MonoBehaviour
{
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip fallClip;

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
            Debug.Log("TriggerBarrier");
            anim.SetBool("isTriggered", true);
            StartCoroutine(DelayedSFX());
        }
    }
    IEnumerator DelayedSFX()
    {
        yield return new WaitForSeconds(0.9f);
        audioSource.PlayOneShot(fallClip);
    }
}
