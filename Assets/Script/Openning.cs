using UnityEngine;
using System.Collections;

public class Openning : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DelayedDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
