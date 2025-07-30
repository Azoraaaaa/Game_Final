using UnityEngine;

public class WayPointNavigator : MonoBehaviour
{
    [Header("AI Character")]
    CharacterNavigating character;
    public WayPoint currentWayPoint;
    int direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        character = GetComponent<CharacterNavigating>();
        character.LocateDestination(currentWayPoint.GetPosition()); //get nearest waypoint for him to start the walk
        
    }

    // Update is called once per frame
    void Update()
    {
        //this code will make nestWayPoint as next destination

        if(character.destinationReached)
        {
            if(direction == 0)
            {
                currentWayPoint = currentWayPoint.nextWayPoint;
            }
            else
            {
                currentWayPoint = currentWayPoint.previousWayPoint;
            }

            character.LocateDestination(currentWayPoint.GetPosition());
        }
        
    }
}
