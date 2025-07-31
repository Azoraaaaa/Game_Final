using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class TimelineTrigger : MonoBehaviour
{
    [SerializeField] private TimelineCondition timelineCondition;
    private bool playerInRange = false;

    private void Start()
    {
        // 确保有触发器组件
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;

        // 获取TimelineCondition组件
        if (timelineCondition == null)
        {
            timelineCondition = GetComponent<TimelineCondition>();
        }
    }

    private void Update()
    {
        // 当玩家在范围内并按下交互键时
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryPlayTimeline();
        }
    }

    private void TryPlayTimeline()
    {
        if (timelineCondition != null)
        {
            timelineCondition.TryPlayTimeline();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("玩家进入触发器范围");
            // 显示交互提示UI - 添加空值检查
            if (NotificationCanvas.instance != null)
            {
                NotificationCanvas.instance.ShowInteractPrompt("Press E to Interact");
                Debug.Log("尝试显示交互提示");
            }
            else
            {
                Debug.LogWarning("NotificationCanvas.instance 为空！");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("玩家离开触发器范围");
            // 隐藏交互提示UI - 添加空值检查
            if (NotificationCanvas.instance != null)
            {
                NotificationCanvas.instance.HideInteractPrompt();
                Debug.Log("尝试隐藏交互提示");
            }
            else
            {
                Debug.LogWarning("NotificationCanvas.instance 为空！");
            }
        }
    }
}