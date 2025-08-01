using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    public static ChaseTrigger instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject triggerArea;
    public GameObject bgmAS;

    public bool isTriggered = true;//!!!


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triggerArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isTriggered)
        {
            Debug.Log("Trigger!");
            triggerArea.SetActive(true);
            bgmAS.SetActive(false);
        }
    }
}
