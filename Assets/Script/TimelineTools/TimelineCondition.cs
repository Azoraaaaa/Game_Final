using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TimelineState
{
    public PlayableDirector timeline;
    public bool canRepeat = false;
    public bool hasPlayed = false;
    public string requiredItemId = "";
    public string requiredQuestId = "";
    public bool isQuestComplete = false;
}

public class TimelineCondition : MonoBehaviour
{
    public TimelineState[] timelineStates;
    private int currentStateIndex = 0;

    public bool TryPlayTimeline()
    {
        Debug.Log(">>> TryPlayTimeline called");

        if (currentStateIndex >= timelineStates.Length)
            return false;

        // 尝试播放当前状态
        if (CanPlayCurrentTimeline())
        {
            PlayCurrentTimeline();
            return true;
        }
        else
        {
            // 如果当前状态无法播放，尝试自动推进到下一个可用状态
            Debug.Log($"Current state {currentStateIndex} cannot play, trying to advance...");
            return TryAdvanceToNextPlayableState();
        }
    }

    private bool TryAdvanceToNextPlayableState()
    {
        int originalIndex = currentStateIndex;
        
        // 尝试推进到下一个状态
        while (currentStateIndex < timelineStates.Length)
        {
            currentStateIndex++;
            Debug.Log($"Advancing to state {currentStateIndex}");
            
            if (CanPlayCurrentTimeline())
            {
                PlayCurrentTimeline();
                return true;
            }
        }
        
        // 如果无法找到可播放的状态，恢复到原始索引
        currentStateIndex = originalIndex;
        Debug.Log("No playable state found, staying at original index");
        return false;
    }

    private void PlayCurrentTimeline()
    {
        TimelineState currentState = timelineStates[currentStateIndex];
        
        // 播放timeline
        TimelineManager.Instance.Play(currentState.timeline);
        Debug.Log($"Playing timeline at index: {currentStateIndex}");

        // 如果不能重复播放，标记为已播放并移动到下一个状态
        if (!currentState.canRepeat)
        {
            currentState.hasPlayed = true;
            currentStateIndex++;
        }
    }

    private bool CanPlayCurrentTimeline()
    {
        if (currentStateIndex >= timelineStates.Length)
            return false;

        TimelineState currentState = timelineStates[currentStateIndex];

        // 如果已经播放过且不能重复，返回false
        if (currentState.hasPlayed && !currentState.canRepeat)
            return false;

        // 检查是否需要特定物品
        if (!string.IsNullOrEmpty(currentState.requiredItemId))
        {
            // 这里需要实现物品检查逻辑
            if (!HasRequiredItem(currentState.requiredItemId))
                return false;
        }

        // 检查任务状态
        if (!string.IsNullOrEmpty(currentState.requiredQuestId))
        {
            bool isCompleted = IsQuestCompleted(currentState.requiredQuestId);
            Debug.Log($"Quest {currentState.requiredQuestId} completed: {isCompleted}, required: {currentState.isQuestComplete}");
            
            // 如果需要任务完成但未完成，或需要任务未完成但已完成，返回false
            if (currentState.isQuestComplete && !isCompleted)
                return false;
            if (!currentState.isQuestComplete && isCompleted)
                return false;
        }

        return true;
    }

    // 这个方法需要根据你的物品系统来实现
    private bool HasRequiredItem(string itemId)
    {
        // TODO: 实现物品检查逻辑
        // 例如：return InventoryManager.Instance.HasItem(itemId);
        return false;
    }

    private bool IsQuestCompleted(string questId)
    {
        if (QuestManager.Instance != null)
        {
            Debug.Log("Checking if quest " + questId + " is completed: " + (QuestManager.Instance.GetQuestStatus(questId) == QuestStatus.Completed));
            return QuestManager.Instance.GetQuestStatus(questId) == QuestStatus.Completed;
        }
        Debug.LogWarning("QuestManager not found in scene!");
        return false;
    }

    // 添加一个公共方法来手动重置状态（用于调试）
    public void ResetState()
    {
        currentStateIndex = 0;
        foreach (var state in timelineStates)
        {
            state.hasPlayed = false;
        }
        Debug.Log("Timeline condition state reset");
    }

    // 添加一个公共方法来获取当前状态信息（用于调试）
    public string GetCurrentStateInfo()
    {
        if (currentStateIndex >= timelineStates.Length)
            return "No more states available";
            
        var state = timelineStates[currentStateIndex];
        return $"State {currentStateIndex}: Timeline={state.timeline?.name}, HasPlayed={state.hasPlayed}, CanRepeat={state.canRepeat}";
    }
}