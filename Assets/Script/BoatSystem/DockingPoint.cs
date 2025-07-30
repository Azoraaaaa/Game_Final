using UnityEngine;

public class DockingPoint : MonoBehaviour
{
    [Header("码头设置")]
    [SerializeField] private string dockName = "码头";
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private Transform boardingPoint; // 上船点
    
    [Header("任务检查")]
    [SerializeField] private string requiredQuestId = "L2-S03"; // 需要完成的任务ID
    [SerializeField] private bool requireQuestCompletion = true; // 是否需要完成任务才能使用
    
    private bool playerInRange = false;
    private PlayerController nearbyPlayer;
    private Boat nearbyBoat;
    
    private void OnTriggerEnter(Collider other)
    {
        // 检测玩家进入范围
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerInRange = true;
            nearbyPlayer = player;
        }
        
        // 检测船只进入范围
        Boat boat = other.GetComponent<Boat>();
        if (boat != null)
        {
            nearbyBoat = boat;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // 检测玩家离开范围
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerInRange = false;
            nearbyPlayer = null;
        }
        
        // 检测船只离开范围
        Boat boat = other.GetComponent<Boat>();
        if (boat != null)
        {
            nearbyBoat = null;
        }
    }
    
    private void Update()
    {
        // 检查是否可以使用船只
        if (!CanUseBoat())
        {
            return;
        }
        
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyBoat != null && nearbyBoat.CanPlayerBoard())
            {
                // 让玩家上船
                nearbyBoat.BoardBoat(nearbyPlayer);
            }
        }
    }
    
    private bool CanUseBoat()
    {
        // 如果不需要检查任务完成状态，直接返回true
        if (!requireQuestCompletion)
        {
            return true;
        }
        
        // 如果找不到任务系统，输出警告并返回true
        if (QuestManager.Instance == null)
        {
            Debug.LogWarning("未找到任务系统，默认允许使用船只", this);
            return true;
        }
        
        // 检查任务是否完成
        return QuestManager.Instance.GetQuestStatus(requiredQuestId) == QuestStatus.Completed;
    }
    
    private void OnDrawGizmosSelected()
    {
        // 在Scene视图中显示交互范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRange);
        
        // 显示上船点
        if (boardingPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(boardingPoint.position, 0.3f);
        }
    }
}