using UnityEngine;
using TMPro;

public class DockingPoint : MonoBehaviour
{
    [Header("码头设置")]
    [SerializeField] private string dockName = "码头";
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private Transform exitPoint; // 下船点
    
    [Header("任务检查")]
    [SerializeField] private string requiredQuestId = "L2-S03";
    [SerializeField] private bool requireQuestCompletion = true;
    
    [Header("UI设置")]
    [SerializeField] private TextMeshProUGUI promptText;
    
    [Header("调试")]
    [SerializeField] private bool showDebugLogs = true;
    
    private bool playerInRange = false;
    private bool boatInRange = false;
    private PlayerController nearbyPlayer;
    private Boat nearbyBoat;

    public Vector3 GetExitPosition()
    {
        return exitPoint != null ? exitPoint.position : transform.position;
    }
    
    private void Start()
    {
        ValidateSetup();
    }
    
    private void ValidateSetup()
    {
        if (showDebugLogs)
        {
            if (exitPoint == null)
            {
                Debug.LogWarning($"[{gameObject.name}] 未设置下船点，将使用码头点位置作为下船点", this);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (showDebugLogs)
        {
            Debug.Log($"[{gameObject.name}] 触发器检测到物体: {other.name}", this);
        }
        
        // 检测玩家进入范围
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerInRange = true;
            nearbyPlayer = player;
            
            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] 玩家进入范围", this);
            }
        }
        
        // 检测船只进入范围
        Boat boat = other.GetComponent<Boat>();
        if (boat != null)
        {
            boatInRange = true;
            nearbyBoat = boat;
            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] 船只进入范围: {boat.name}", this);
            }
        }
        
        UpdatePromptText();
    }
    
    private void OnTriggerExit(Collider other)
    {
        // 检测玩家离开范围
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            playerInRange = false;
            nearbyPlayer = null;
            
            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] 玩家离开范围", this);
            }
        }
        
        // 检测船只离开范围
        Boat boat = other.GetComponent<Boat>();
        if (boat != null)
        {
            boatInRange = false;
            nearbyBoat = null;
            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] 船只离开范围", this);
            }
        }
        
        UpdatePromptText();
    }
    
    private void Update()
    {
        // 如果船在范围内但玩家不在范围内，检查船上的玩家
        if (boatInRange && !playerInRange && nearbyBoat != null && nearbyBoat.IsOccupied())
        {
            playerInRange = true;
            nearbyPlayer = nearbyBoat.GetPlayer();
            UpdatePromptText();
        }
        
        if (!playerInRange || nearbyPlayer == null) return;
        
        // 当按下E键时
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 检查是否可以使用船只
            if (!CanUseBoat())
            {
                if (showDebugLogs)
                {
                    Debug.Log($"[{gameObject.name}] 任务未完成，无法使用船只", this);
                }
                return;
            }
            
            // 如果玩家在船上且船在范围内，尝试下船
            if (nearbyBoat != null && nearbyBoat.IsOccupied() && boatInRange)
            {
                nearbyBoat.ExitBoat(this);
                if (showDebugLogs)
                {
                    Debug.Log($"[{gameObject.name}] 玩家下船到码头点", this);
                }
            }
            // 如果玩家不在船上，尝试上船
            else if (nearbyBoat != null && nearbyBoat.CanPlayerBoard() && boatInRange)
            {
                nearbyBoat.BoardBoat(nearbyPlayer);
                if (showDebugLogs)
                {
                    Debug.Log($"[{gameObject.name}] 玩家上船", this);
                }
            }
            
            UpdatePromptText();
        }
    }
    
    private bool CanUseBoat()
    {
        if (!requireQuestCompletion) return true;
        
        if (QuestManager.Instance == null)
        {
            if (showDebugLogs)
            {
                Debug.LogWarning($"[{gameObject.name}] 未找到任务系统，默认允许使用船只", this);
            }
            return true;
        }
        
        bool questCompleted = QuestManager.Instance.GetQuestStatus(requiredQuestId) == QuestStatus.Completed;
        if (showDebugLogs && !questCompleted)
        {
            Debug.Log($"[{gameObject.name}] 任务 {requiredQuestId} 未完成", this);
        }
        return questCompleted;
    }
    
    private void UpdatePromptText()
    {
        if (promptText == null) return;
        
        // 如果船和玩家都不在范围内，隐藏提示
        if (!playerInRange || !boatInRange)
        {
            HidePromptText();
            return;
        }
        
        if (!CanUseBoat())
        {
            promptText.text = "需要完成指定任务才能使用船只";
            promptText.gameObject.SetActive(true);
            return;
        }
        
        if (nearbyBoat != null)
        {
            bool isOnBoat = nearbyBoat.IsOccupied();
            promptText.text = isOnBoat ? "按 E 下船" : "按 E 上船";
            promptText.gameObject.SetActive(true);
        }
        else
        {
            HidePromptText();
        }
    }
    
    private void HidePromptText()
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }
    
    private void OnDrawGizmos()
    {
        // 显示交互范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRange);
        
        // 显示下船点
        if (exitPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(exitPoint.position, 0.3f);
            Gizmos.DrawLine(transform.position, exitPoint.position);
        }
    }
}