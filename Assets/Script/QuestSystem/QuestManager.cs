using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    [Header("Quest Data")]
    public List<QuestData> allQuests;
    
    [Header("UI References")]
    public QuestUI questUI;
    
    private Dictionary<string, QuestStatus> questStatuses = new Dictionary<string, QuestStatus>();
    private Dictionary<string, QuestData> questDataMap = new Dictionary<string, QuestData>();
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            InitializeQuestSystem();
        }
    }
    
    void InitializeQuestSystem()
    {
        // 初始化任务数据映射
        foreach (var quest in allQuests)
        {
            questDataMap[quest.questID] = quest;
            if (!questStatuses.ContainsKey(quest.questID))
            {
                questStatuses[quest.questID] = QuestStatus.NotStarted;
            }
        }
    }
    
    // 开始任务
    public bool StartQuest(string questID)
    {
        if (!questDataMap.ContainsKey(questID))
        {
            Debug.LogWarning($"Quest {questID} not found!");
            return false;
        }
        
        QuestData quest = questDataMap[questID];
        
        // 检查前置条件
        if (!CheckPrerequisites(quest))
        {
            Debug.LogWarning($"Prerequisites not met for quest {questID}");
            return false;
        }
        
        questStatuses[questID] = QuestStatus.InProgress;
        
        // 通知UI更新
        if (questUI != null)
        {
            questUI.UpdateQuestList();
        }
        
        // 通知地图系统
        if (quest.showOnMap)
        {
            MapSystem.Instance?.AddQuestMarker(quest);
        }
        
        Debug.Log($"Quest {questID} started!");
        return true;
    }
    
    // 更新任务目标进度
    public void UpdateQuestObjective(string questID, string objectiveID, int progress)
    {
        if (!questDataMap.ContainsKey(questID) || questStatuses[questID] != QuestStatus.InProgress)
            return;
            
        QuestData quest = questDataMap[questID];
        var objective = quest.objectives.Find(o => o.objectiveID == objectiveID);
        
        if (objective != null)
        {
            objective.currentAmount = Mathf.Min(objective.currentAmount + progress, objective.requiredAmount);
            objective.isCompleted = objective.currentAmount >= objective.requiredAmount;
            
            // 检查任务是否完成
            if (quest.objectives.All(o => o.isCompleted))
            {
                CompleteQuest(questID);
            }
            else
            {
                // 更新UI
                questUI?.UpdateQuestProgress(questID);
            }
        }
    }
    
    // 完成任务
    public void CompleteQuest(string questID)
    {
        if (!questDataMap.ContainsKey(questID))
            return;
            
        questStatuses[questID] = QuestStatus.Completed;
        
        QuestData quest = questDataMap[questID];
        
        // 给予奖励
        GiveQuestRewards(quest);
        
        // 更新地图标记
        if (quest.showOnMap)
        {
            MapSystem.Instance?.UpdateQuestMarker(quest, QuestStatus.Completed);
        }
        
        // 更新UI
        questUI?.UpdateQuestList();
        
        Debug.Log($"Quest {questID} completed!");
    }
    
    // 检查前置条件
    private bool CheckPrerequisites(QuestData quest)
    {
        foreach (var prereq in quest.prerequisiteQuests)
        {
            if (GetQuestStatus(prereq.questID) != QuestStatus.Completed)
            {
                return false;
            }
        }
        return true;
    }
    
    // 给予任务奖励
    private void GiveQuestRewards(QuestData quest)
    {
        // 这里可以集成您的物品系统
        Debug.Log($"Giving rewards for quest {quest.questID}: {quest.experienceReward} XP, {quest.goldReward} Gold");
    }
    
    // 获取任务状态
    public QuestStatus GetQuestStatus(string questID)
    {
        return questStatuses.ContainsKey(questID) ? questStatuses[questID] : QuestStatus.NotStarted;
    }
    
    // 获取可用的任务
    public List<QuestData> GetAvailableQuests()
    {
        return allQuests.Where(q => 
            GetQuestStatus(q.questID) == QuestStatus.NotStarted && 
            CheckPrerequisites(q)
        ).ToList();
    }
    
    // 获取进行中的任务
    public List<QuestData> GetActiveQuests()
    {
        return allQuests.Where(q => GetQuestStatus(q.questID) == QuestStatus.InProgress).ToList();
    }
    
    // 获取已完成的任务
    public List<QuestData> GetCompletedQuests()
    {
        return allQuests.Where(q => GetQuestStatus(q.questID) == QuestStatus.Completed).ToList();
    }
} 