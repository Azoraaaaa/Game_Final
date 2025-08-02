using UnityEngine;

public class BombTrigger : MonoBehaviour
{
    public static BombTrigger instance;
    public GameObject BombUI;
    private void Awake()
    {
        instance = this;
    }

    public float val = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (val != 0)
        {
            BombUI.SetActive(true);
        }
    }
}
