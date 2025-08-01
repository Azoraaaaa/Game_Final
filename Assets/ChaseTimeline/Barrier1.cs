using UnityEngine;

public class Barrier1 : MonoBehaviour
{
    public static Barrier1 instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void set()
    {
        MeshCollider col = GetComponent<MeshCollider>();
        col.enabled = false;
    }
}
