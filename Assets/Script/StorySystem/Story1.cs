using UnityEngine;

public class Story1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StoryManager.instance.isCollected1 = true;
            Destroy(gameObject);
        }
    }
}
