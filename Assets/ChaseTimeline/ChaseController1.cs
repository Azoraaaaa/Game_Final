using UnityEngine;

public class ChaseController1 : MonoBehaviour
{
    public GameObject originalMonster;
    public GameObject monster;

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
}
