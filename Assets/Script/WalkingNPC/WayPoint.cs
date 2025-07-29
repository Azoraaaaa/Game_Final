using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [Header("Waypoint Status")]
    public WayPoint previousWayPoint;
    public WayPoint nextWayPoint;

    [Range(0f, 5f)] //range for waypointwidth
    public float waypointWidth = 1f;

    public Vector3 GetPosition() //create space from 1 point to another
    {
        Vector3 minBound = transform.position + transform.right * waypointWidth / 2f;
        Vector3 maxBound = transform.position - transform.right * waypointWidth / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
    
}
