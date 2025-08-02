using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    public GameObject SwordUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void hasLearned()
    {
        SwordUI.SetActive(true);
    }
}
