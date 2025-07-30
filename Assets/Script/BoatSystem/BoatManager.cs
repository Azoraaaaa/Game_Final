using UnityEngine;
using System.Collections.Generic;

public class BoatManager : MonoBehaviour
{
    public static BoatManager instance;
    
    [Header("船只设置")]
    [SerializeField] private List<DockingPoint> dockingPoints;
    [SerializeField] private List<Boat> boats;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // 收集场景中的所有码头点
        if (dockingPoints.Count == 0)
        {
            dockingPoints.AddRange(FindObjectsOfType<DockingPoint>());
        }
        
        // 收集场景中的所有船只
        if (boats.Count == 0)
        {
            boats.AddRange(FindObjectsOfType<Boat>());
        }
    }
    
    public bool IsPlayerOnBoat(PlayerController player)
    {
        foreach (var boat in boats)
        {
            if (boat.IsOccupied())
            {
                return true;
            }
        }
        return false;
    }
    
    public Boat GetNearestBoat(Vector3 position)
    {
        Boat nearestBoat = null;
        float nearestDistance = float.MaxValue;
        
        foreach (var boat in boats)
        {
            if (!boat.IsOccupied())
            {
                float distance = Vector3.Distance(position, boat.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestBoat = boat;
                }
            }
        }
        
        return nearestBoat;
    }
    
    public DockingPoint GetNearestDockingPoint(Vector3 position)
    {
        DockingPoint nearestPoint = null;
        float nearestDistance = float.MaxValue;
        
        foreach (var point in dockingPoints)
        {
            float distance = Vector3.Distance(position, point.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPoint = point;
            }
        }
        
        return nearestPoint;
    }
}