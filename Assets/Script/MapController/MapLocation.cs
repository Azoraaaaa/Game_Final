
using UnityEngine;
using UnityEngine.UI;

public class MapLocation : MonoBehaviour
{
    public string locationID; // 每个地点的唯一标识符
    public Transform teleportDestination; // 为该地点设置一个传送目标点
    public GameObject undiscoveredVisual; // 地点未被发现时的视觉效果
    public GameObject discoveredVisual; // 地点被发现后的视觉效果
    public Button teleportButton; // 传送按钮

    private bool isDiscovered = false;

    void Start()
    {
        if (string.IsNullOrEmpty(locationID))
        {
            Debug.LogError("Location ID is not set for " + gameObject.name);
        }

        // 初始化时，根据GameManager的数据设置状态
        if (GameManager.Instance != null)
        {
            SetDiscovered(GameManager.Instance.discoveredLocations.Contains(locationID));
        }

        if (teleportButton != null)
        {
            teleportButton.onClick.AddListener(TeleportPlayer);
        }
    }

    public void SetDiscovered(bool discovered)
    {
        isDiscovered = discovered;
        undiscoveredVisual.SetActive(!isDiscovered);
        discoveredVisual.SetActive(isDiscovered);

        // 传送按钮的可用性现在取决于地点是否被发现以及玩家是否在传送点附近
        bool canTeleport = isDiscovered && GameManager.Instance.isNearTeleporter;
        teleportButton.interactable = canTeleport;
    }

    void TeleportPlayer()
    {
        if (!isDiscovered) return;

        // 检查玩家是否在传送点附近
        if (GameManager.Instance.isNearTeleporter)
        {
            if (teleportDestination == null)
            {
                Debug.LogError("Teleport destination not set for location: " + locationID);
                return;
            }

            if (GameManager.Instance == null || GameManager.Instance.player == null)
            {
                Debug.LogError("GameManager or Player not found!");
                return;
            }

            Debug.Log("Teleporting player to " + locationID);

            // 如果玩家对象上有CharacterController，传送前需要先禁用它
            CharacterController controller = GameManager.Instance.player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            GameManager.Instance.player.transform.position = teleportDestination.position;

            // 传送后再启用CharacterController
            if (controller != null)
            {
                controller.enabled = true;
            }
        }
        else
        {
            // 如果玩家不在传送点附近，则显示提示信息
            UIManager.instance.ShowNotification("需要先靠近一个传送点才能传送", 2f);
        }
    }
} 