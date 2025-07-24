using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public static MiniMapController Instance { get; private set; }

    public Transform player;
    public float height = 70f;

    [Tooltip("The orthographic size of the MiniMap camera. This defines the zoom level.")]
    public float miniMapSize = 25f;
    
    private Camera miniMapCamera;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        miniMapCamera = GetComponent<Camera>();
    }

    void Start()
    {
        if(miniMapCamera != null)
        {
            miniMapCamera.orthographicSize = miniMapSize;
        }
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector3 newPosition = player.position;
        newPosition.y = height;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
} 