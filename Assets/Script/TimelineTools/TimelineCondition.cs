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
        if (currentStateIndex >= timelineStates.Length)
            return false;

        TimelineState currentState = timelineStates[currentStateIndex];

        // 检查是否可以播放
        if (CanPlayCurrentTimeline())
        {
            // 播放timeline
            TimelineManager.Instance.Play(currentState.timeline);
            
            // 如果不能重复播放，标记为已播放
            if (!currentState.canRepeat)
            {
                currentState.hasPlayed = true;
                // 如果当前状态不能重复，移动到下一个状态
                currentStateIndex++;
            }
            return true;
        }

        return false;
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
        if (TestQuestManager.Instance != null)
        {
            return TestQuestManager.Instance.IsQuestCompleted(questId);
        }
        Debug.LogWarning("TestQuestManager not found in scene!");
        return false;
    }
}