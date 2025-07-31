
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject mapUI; // 指向你的大地图UI面板
    public Transform locationsContainer; // 指向地图上所有地点的父物体
    private bool isMapOpen = false;

    void Start()
    {
        if (mapUI == null)
        {
            Debug.LogError("Map UI is not assigned in the MapController.");
            return;
        }
        mapUI.SetActive(false); // 游戏开始时默认关闭地图
    }

    void Update()
    {
        // 按下M键打开/关闭大地图
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMapOpen = !isMapOpen;
            mapUI.SetActive(isMapOpen);

            if (isMapOpen)
            {
                UpdateMapLocations();
            }
        }
    }

    public void UpdateMapLocations()
    {
        if (locationsContainer == null)
        {
            Debug.LogError("Locations container is not assigned in the MapController.");
            return;
        }

        foreach (Transform location in locationsContainer)
        {
            MapLocation mapLocation = location.GetComponent<MapLocation>();
            if (mapLocation != null)
            {
                bool isDiscovered = GameManager.Instance.discoveredLocations.Contains(mapLocation.locationID);
                mapLocation.SetDiscovered(isDiscovered);
            }
        }
    }
} 