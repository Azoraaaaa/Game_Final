using UnityEngine;

public class BowTrigger : MonoBehaviour
{
    public GameObject BowUI;
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
        BowUI.SetActive(true);
    }
}
