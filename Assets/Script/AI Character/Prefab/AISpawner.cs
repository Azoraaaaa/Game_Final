using System.Collections;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject[] AiPrefabs;
    public int AiToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while(count < AiToSpawn)
        {
            int randomIndex = Random.Range(0, AiPrefabs.Length);

            GameObject obj = Instantiate(AiPrefabs[randomIndex]);

            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            //this will randmize from all traficAiWayPoint

            obj.GetComponent<WayPointNavigator>().currentWayPoint = child.GetComponent<WayPoint>();
            //set the AI currentWayPoint based on the random number

            obj.transform.position = child.position; //make it on child position

            yield return new WaitForSeconds(2f); //depends on yr preference

            count++;

        }
    }
}
