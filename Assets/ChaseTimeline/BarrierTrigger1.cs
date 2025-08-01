using Unity.Cinemachine;
using UnityEngine;

public class BarrierTrigger1 : MonoBehaviour
{
    public bool isPlayerNear = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isPlayerNear)
        {
            Barrier1.instance.set();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }
}
