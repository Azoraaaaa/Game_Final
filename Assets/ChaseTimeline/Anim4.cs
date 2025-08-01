using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Anim4 : MonoBehaviour
{
    public GameObject UIhint;
    public GameObject plants;
    public bool isPlayerNear = false;
    public AudioSource AS;
    public AudioClip pClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIhint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerNear)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayPlants();
                StartCoroutine(DelayedSFX());
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIhint.SetActive(true);
            isPlayerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIhint.SetActive(false);
        }
    }
    public void PlayPlants()
    {
        AS.PlayOneShot(pClip);
    }
    IEnumerator DelayedSFX()
    {
        yield return new WaitForSeconds(0.5f);
        plants.SetActive(false);
    }
}
