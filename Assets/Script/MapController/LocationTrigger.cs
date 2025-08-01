
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public static LocationTrigger instance;
    private void Awake()
    {
        instance = this;
    }

    public string locationID; // 与MapLocation中的ID对应
    private bool playerIsInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = true;
            GameManager.Instance.isNearTeleporter = true;

            // 检查地点是否已被发现
            if (!GameManager.Instance.discoveredLocations.Contains(locationID))
            {
                UIManager.instance.ShowPersistentNotification("Press <b>E</b> Activate the Teleportation Point");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = false;
            GameManager.Instance.isNearTeleporter = false;
            UIManager.instance.HideNotification();

            ChaseTrigger.instance.isTriggered = true;
        }
    }

    void Update()
    {
        // 如果玩家在区域内并且按下了E键
        if (playerIsInZone && Input.GetKeyDown(KeyCode.E))
        {
            // 检查地点是否是第一次被激活
            if (GameManager.Instance.discoveredLocations.Add(locationID))
            {
                Debug.Log("Location discovered: " + locationID);
                UIManager.instance.ShowNotification("Teleportation Point Activated", 2f); // 显示一个短暂的成功提示
            }
        }
    }
} 