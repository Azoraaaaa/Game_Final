using UnityEngine;
using UnityEngine.Audio;

public class Anim3 : MonoBehaviour
{
    public GameObject UIhint;
    public AudioSource AS;
    public AudioClip mClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIhint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIhint.SetActive(true);
            PlayMonster();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIhint.SetActive(false);
        }
    }
    public void PlayMonster()
    {
        AS.PlayOneShot(mClip);
    }
}
