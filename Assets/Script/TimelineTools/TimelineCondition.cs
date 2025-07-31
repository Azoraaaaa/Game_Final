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
        Debug.Log($"TryPlayTimeline 被调用，当前状态索引: {currentStateIndex}");
        
        // 首先重新评估当前状态，如果当前状态不再满足条件，移动到下一个状态
        ReevaluateCurrentState();
        
        if (currentStateIndex >= timelineStates.Length)
        {
            Debug.LogWarning($"当前状态索引 {currentStateIndex} 超出范围，总状态数: {timelineStates.Length}");
            return false;
        }

        TimelineState currentState = timelineStates[currentStateIndex];
        Debug.Log($"检查状态 {currentStateIndex}: Timeline={currentState.timeline?.name}, CanRepeat={currentState.canRepeat}, HasPlayed={currentState.hasPlayed}");

        // 检查是否可以播放
        if (CanPlayCurrentTimeline())
        {
            Debug.Log($"状态 {currentStateIndex} 可以播放，开始播放 Timeline: {currentState.timeline?.name}");
            
            // 播放timeline
            TimelineManager.Instance.Play(currentState.timeline);
            
            // 如果不能重复播放，标记为已播放
            if (!currentState.canRepeat)
            {
                currentState.hasPlayed = true;
                // 如果当前状态不能重复，移动到下一个状态
                currentStateIndex++;
                Debug.Log($"状态 {currentStateIndex - 1} 已播放，移动到下一个状态: {currentStateIndex}");
            }
            return true;
        }
        else
        {
            Debug.LogWarning($"状态 {currentStateIndex} 不能播放，尝试移动到下一个状态");
            // 如果当前状态不能播放，尝试移动到下一个状态
            currentStateIndex++;
            if (currentStateIndex < timelineStates.Length)
            {
                Debug.Log($"移动到状态 {currentStateIndex}，递归调用 TryPlayTimeline");
                return TryPlayTimeline(); // 递归尝试下一个状态
            }
            else
            {
                Debug.LogWarning("没有更多状态可以尝试");
                return false;
            }
        }
    }
    
    /// <summary>
    /// 重新评估当前状态，如果当前状态不再满足条件，尝试移动到下一个状态
    /// </summary>
    private void ReevaluateCurrentState()
    {
        Debug.Log($"重新评估当前状态，索引: {currentStateIndex}");
        
        if (currentStateIndex >= timelineStates.Length)
        {
            Debug.LogWarning("当前状态索引超出范围，无法重新评估");
            return;
        }
        
        // 检查当前状态是否仍然可以播放
        if (!CanPlayCurrentTimeline())
        {
            Debug.Log("当前状态不再满足条件，尝试移动到下一个状态");
            currentStateIndex++;
            if (currentStateIndex < timelineStates.Length)
            {
                Debug.Log($"移动到状态 {currentStateIndex}，继续评估");
                ReevaluateCurrentState(); // 递归评估下一个状态
            }
            else
            {
                Debug.LogWarning("没有更多状态可以尝试");
            }
        }
        else
        {
            Debug.Log("当前状态仍然满足条件，继续播放");
        }
    }

    private bool CanPlayCurrentTimeline()
    {
        if (currentStateIndex >= timelineStates.Length)
            return false;

        TimelineState currentState = timelineStates[currentStateIndex];
        Debug.Log($"检查状态 {currentStateIndex} 是否可以播放:");

        // 如果已经播放过且不能重复，返回false
        if (currentState.hasPlayed && !currentState.canRepeat)
        {
            Debug.Log($"状态 {currentStateIndex} 已经播放过且不能重复");
            return false;
        }

        // 检查是否需要特定物品
        if (!string.IsNullOrEmpty(currentState.requiredItemId))
        {
            // 这里需要实现物品检查逻辑
            if (!HasRequiredItem(currentState.requiredItemId))
            {
                Debug.Log($"状态 {currentStateIndex} 缺少所需物品: {currentState.requiredItemId}");
                return false;
            }
        }

        // 检查任务状态
        if (!string.IsNullOrEmpty(currentState.requiredQuestId))
        {
            bool isCompleted = IsQuestCompleted(currentState.requiredQuestId);
            Debug.Log($"状态 {currentStateIndex} 任务 {currentState.requiredQuestId} 完成状态: {isCompleted}, 需要完成: {currentState.isQuestComplete}");
            
            // 如果需要任务完成但未完成，或需要任务未完成但已完成，返回false
            if (currentState.isQuestComplete && !isCompleted)
            {
                Debug.Log($"状态 {currentStateIndex} 需要任务完成但任务未完成");
                return false;
            }
            if (!currentState.isQuestComplete && isCompleted)
            {
                Debug.Log($"状态 {currentStateIndex} 需要任务未完成但任务已完成");
                return false;
            }
        }

        Debug.Log($"状态 {currentStateIndex} 可以播放");
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
        Debug.Log($"IsQuestCompleted 被调用，查询任务ID: {questId}");
        
        if (QuestManager.Instance != null)
        {
            QuestStatus status = QuestManager.Instance.GetQuestStatus(questId);
            bool isCompleted = (status == QuestStatus.Completed);
            Debug.Log($"从 QuestManager 获取任务 {questId} 状态: {status} (完成: {isCompleted})");
            return isCompleted;
        }
        else
        {
            Debug.LogError("QuestManager.Instance 为空！请确保场景中有 QuestManager 对象");
            return false;
        }
    }

    /// <summary>
    /// 重置所有状态，用于重新测试
    /// </summary>
    public void ResetAllStates()
    {
        currentStateIndex = 0;
        foreach (var state in timelineStates)
        {
            state.hasPlayed = false;
        }
        Debug.Log("所有Timeline状态已重置");
    }
    
    /// <summary>
    /// 获取当前状态索引
    /// </summary>
    public int GetCurrentStateIndex()
    {
        return currentStateIndex;
    }
    
    /// <summary>
    /// 设置当前状态索引（用于调试）
    /// </summary>
    public void SetCurrentStateIndex(int index)
    {
        if (index >= 0 && index < timelineStates.Length)
        {
            currentStateIndex = index;
            Debug.Log($"当前状态索引已设置为: {currentStateIndex}");
        }
        else
        {
            Debug.LogWarning($"无效的状态索引: {index}");
        }
    }
}