using UnityEngine;
using UnityEngine.Playables;

public class QuestTrigger : MonoBehaviour
{
    [Header("Quest Settings")]
    public string questID;
    public bool triggerOnStart = false;
    public bool triggerOnTimelineEnd = false;
    
    [Header("Timeline Integration")]
    public PlayableDirector timelineDirector;
    
    private bool hasTriggered = false;
    
    void Start()
    {
        if (triggerOnStart)
        {
            TriggerQuest();
        }
        
        if (timelineDirector != null && triggerOnTimelineEnd)
        {
            timelineDirector.stopped += OnTimelineEnded;
        }
    }
    
    void OnDestroy()
    {
        if (timelineDirector != null)
        {
            timelineDirector.stopped -= OnTimelineEnded;
        }
    }
    
    // Timeline结束时触发任务
    private void OnTimelineEnded(PlayableDirector director)
    {
        if (director == timelineDirector && !hasTriggered)
        {
            TriggerQuest();
        }
    }
    
    // 触发任务
    public void TriggerQuest()
    {
        if (hasTriggered || QuestManager.Instance == null) return;
        
        if (QuestManager.Instance.StartQuest(questID))
        {
            hasTriggered = true;
            Debug.Log($"Quest {questID} triggered successfully!");
        }
        else
        {
            Debug.LogWarning($"Failed to trigger quest {questID}");
        }
    }
    
    // 更新任务进度
    public void UpdateQuestProgress(string objectiveID, int progress = 1)
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.UpdateQuestObjective(questID, objectiveID, progress);
        }
    }
    
    // 完成任务
    public void CompleteQuest()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteQuest(questID);
        }
    }
    
    // 检查任务状态
    public QuestStatus GetQuestStatus()
    {
        if (QuestManager.Instance != null)
        {
            return QuestManager.Instance.GetQuestStatus(questID);
        }
        return QuestStatus.NotStarted;
    }
} 