using UnityEngine;

public class ChaseController : MonoBehaviour
{
    public GameObject originalMonster;
    public GameObject monster;
    public AudioSource audioSource;
    public AudioClip bgmClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        monster.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void timelineEnd()
    {
        originalMonster.SetActive(false);
        monster.SetActive(true);
    }
    public void PlayBgm()
    {
        audioSource.PlayOneShot(bgmClip);
    }

}
