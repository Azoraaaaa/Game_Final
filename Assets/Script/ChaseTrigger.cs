using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    public GameObject triggerArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(LocationTrigger.instance.isTriggered)
        {
            triggerArea.SetActive(true);
            Debug.Log("Trigger!");
            //stop bgm
        }
    }
}
