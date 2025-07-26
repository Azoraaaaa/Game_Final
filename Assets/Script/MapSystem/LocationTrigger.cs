
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationID; // 与MapLocation中的ID对应
    private bool hasBeenTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        // 确保是玩家触发的
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                // 标记玩家在传送点附近
                GameManager.instance.isNearTeleporter = true;

                if (!hasBeenTriggered && !string.IsNullOrEmpty(locationID))
                {
                    if (GameManager.instance.discoveredLocations.Add(locationID))
                    {
                        Debug.Log("Location discovered: " + locationID);
                        hasBeenTriggered = true;
                    }
                }
            }
            else
            {
                Debug.LogError("GameManager instance not found on " + gameObject.name);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 确保是玩家离开
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                // 标记玩家已离开传送点
                GameManager.instance.isNearTeleporter = false;
            }
        }
    }
} 