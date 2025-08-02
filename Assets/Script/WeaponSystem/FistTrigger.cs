using UnityEngine;

public class FistTrigger : MonoBehaviour
{
    public GameObject FistUI;
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
        FistUI.SetActive(true);
    }
}
