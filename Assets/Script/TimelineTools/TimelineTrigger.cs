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
            // 显示交互提示UI
            NotificationCanvas.instance?.ShowInteractPrompt("按 E 交互");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // 隐藏交互提示UI
            NotificationCanvas.instance?.HideInteractPrompt();
        }
    }
}