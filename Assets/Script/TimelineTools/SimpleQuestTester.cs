using UnityEngine;

/// <summary>
/// 简单的任务状态测试器
/// 直接在Inspector中修改任务状态
/// </summary>
public class SimpleQuestTester : MonoBehaviour
{
    [Header("任务设置")]
    [SerializeField] private string questId = "M01";
    
    [Header("任务状态")]
    [SerializeField] private bool isCompleted = false;
    
    private void Start()
    {
        UpdateQuestStatus();
    }
    
    private void OnValidate()
    {
        // 当在Inspector中修改isCompleted时，自动更新任务状态
        if (Application.isPlaying && QuestManager.Instance != null)
        {
            UpdateQuestStatus();
        }
    }
    
    private void UpdateQuestStatus()
    {
        if (QuestManager.Instance == null) 
        {
            Debug.LogWarning("QuestManager 未找到！");
            return;
        }
        
        QuestStatus currentStatus = QuestManager.Instance.GetQuestStatus(questId);
        bool currentIsCompleted = (currentStatus == QuestStatus.Completed);
        
        if (isCompleted && !currentIsCompleted)
        {
            // 完成任务
            QuestManager.Instance.CompleteQuest(questId);
            Debug.Log($"任务 {questId} 已完成");
        }
        else if (!isCompleted && currentIsCompleted)
        {
            // 重置任务（注意：QuestManager可能没有重置方法，这里只是演示）
            Debug.LogWarning($"任务 {questId} 已完成，无法重置。请手动在QuestManager中重置。");
        }
    }
    
    // 手动完成任务
    [ContextMenu("完成任务")]
    public void CompleteQuest()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteQuest(questId);
            isCompleted = true;
            Debug.Log($"任务 {questId} 已完成");
        }
    }
    
    // 手动重置任务
    [ContextMenu("重置任务")]
    public void ResetQuest()
    {
        Debug.LogWarning("QuestManager 没有重置任务的方法。请手动在QuestManager中重置任务状态。");
        isCompleted = false;
    }
    
    // 显示当前任务状态
    [ContextMenu("显示任务状态")]
    public void ShowQuestStatus()
    {
        if (QuestManager.Instance != null)
        {
            QuestStatus status = QuestManager.Instance.GetQuestStatus(questId);
            Debug.Log($"任务 {questId} 当前状态: {status}");
        }
    }
} 