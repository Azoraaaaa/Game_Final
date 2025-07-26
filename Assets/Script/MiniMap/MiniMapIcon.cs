using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    public Transform target;
    
    private RectTransform myRectTransform;
    private RectTransform mapContainer;

    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        
        GameObject contObj = GameObject.FindWithTag("MiniMapIconsContainer");
        if(contObj != null)
        {
           mapContainer = contObj.GetComponent<RectTransform>();
        }
        else
        {
             Debug.LogError("--- MiniMap FATAL: Could not find a GameObject with tag 'MiniMapIconsContainer'. Please check your tags!", gameObject);
             enabled = false;
        }
    }

    void LateUpdate()
    {
        if (target == null || mapContainer == null || MiniMapController.Instance == null)
        {
            myRectTransform.anchoredPosition = new Vector2(float.MaxValue, float.MaxValue);
            return;
        }

        // Calculate the position difference in the world, relative to the player
        Vector3 positionDifference = target.position - MiniMapController.Instance.player.position;

        // Rotate the difference vector to match the minimap's rotation
        positionDifference = Quaternion.Euler(0, -MiniMapController.Instance.player.eulerAngles.y, 0) * positionDifference;
        
        // Scale the position based on the map's size and zoom level
        float mapSize = mapContainer.rect.width; // Assumes a square map
        float mapScale = mapSize / (MiniMapController.Instance.miniMapSize * 2);
        
        Vector2 finalPosition = new Vector2(positionDifference.x, positionDifference.z) * mapScale;

        myRectTransform.anchoredPosition = finalPosition;
    }
} 