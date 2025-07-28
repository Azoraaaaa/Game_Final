using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Main,       // 主线任务
    Side        // 支线任务
}

public enum QuestStatus
{
    NotStarted,     // 未开始
    InProgress,     // 进行中
    Completed,      // 已完成
    Failed          // 失败
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/New Quest")]
public class QuestData : ScriptableObject
{
    [Header("Basic Info")]
    public string questID;
    public string questTitle;
    [TextArea(3, 5)]
    public string questDescription;
    public QuestType questType;
    
    [Header("Requirements")]
    public List<QuestData> prerequisiteQuests;  // 前置任务
    public int requiredLevel;
    
    [Header("Objectives")]
    public List<QuestObjective> objectives;
    
    [Header("Rewards")]
    public int experienceReward;
    public int goldReward;
    public List<ItemReward> itemRewards;
    
    [Header("Map Integration")]
    public Vector3 questLocation;  // 任务位置
    public bool showOnMap = true;  // 是否在地图上显示
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveID;
    public string description;
    public int requiredAmount;
    public int currentAmount;
    public bool isCompleted;
    
    public QuestObjective(string id, string desc, int required)
    {
        objectiveID = id;
        description = desc;
        requiredAmount = required;
        currentAmount = 0;
        isCompleted = false;
    }
}

[System.Serializable]
public class ItemReward
{
    public string itemID;
    public int amount;
} 