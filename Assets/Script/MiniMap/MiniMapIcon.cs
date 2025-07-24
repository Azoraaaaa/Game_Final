using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    [HideInInspector]
    public Transform target;

    // Static variables to avoid finding them for every icon instance
    private static Camera miniMapCamera;
    private static RectTransform mapContainer;

    private RectTransform myRectTransform;

    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();

        if (miniMapCamera == null)
        {
            GameObject camObj = GameObject.FindWithTag("MiniMapCamera");
            if (camObj != null)
            {
                miniMapCamera = camObj.GetComponent<Camera>();
            }
        }
        
        if (mapContainer == null)
        {
             GameObject contObj = GameObject.FindWithTag("MiniMapIconsContainer");
             if(contObj != null)
             {
                mapContainer = contObj.GetComponent<RectTransform>();
             }
        }
        
        if (miniMapCamera == null || mapContainer == null)
        {
            Debug.LogError("MiniMap setup error: Could not find a Camera with 'MiniMapCamera' tag, or a UI object with 'MiniMapIconsContainer' tag. Please check your setup.", this);
            enabled = false;
        }
    }

    void LateUpdate()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        // Convert the target's world position to a screen position relative to the minimap camera
        Vector2 screenPos = miniMapCamera.WorldToScreenPoint(target.position);

        // Convert the screen position to a local position within the map's UI container
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mapContainer, screenPos, null, out localPosition);

        myRectTransform.localPosition = localPosition;
    }
} 