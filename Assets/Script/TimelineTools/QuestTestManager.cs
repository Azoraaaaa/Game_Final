using UnityEngine;
using System.Collections.Generic;

public class QuestTestManager : MonoBehaviour
{
    [Header("任务测试设置")]
    [SerializeField] private string questIdToTest = "quest1";
    [SerializeField] private bool setQuestCompleted = false;
    [SerializeField] private bool setQuestIncomplete = false;
    
    [Header("当前任务状态")]
    [SerializeField] private bool isQuestCompleted = false;
    
    [Header("所有任务状态")]
    [SerializeField] private List<QuestStatus> allQuests = new List<QuestStatus>();
    
    private bool hasWarnedAboutMissingManager = false;
    
    [System.Serializable]
    public class QuestStatus
    {
        public string questId;
        public bool isCompleted;
    }
    
    private void Start()
    {
        // 初始化测试任务
        if (!allQuests.Exists(q => q.questId == questIdToTest))
        {
            allQuests.Add(new QuestStatus { questId = questIdToTest, isCompleted = false });
        }
        
        UpdateQuestStatus();
    }
    
    private void Update()
    {
        // 检查按钮输入
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQuestStatus();
        }
    }
    
    private void OnValidate()
    {
        // 当在Inspector中修改值时自动更新
        if (setQuestCompleted)
        {
            CompleteQuest(questIdToTest);
            setQuestCompleted = false;
        }
        
        if (setQuestIncomplete)
        {
            ResetQuest(questIdToTest);
            setQuestIncomplete = false;
        }
        
        UpdateQuestStatus();
    }
    
    /// <summary>
    /// 完成任务
    /// </summary>
    public void CompleteQuest(string questId)
    {
        if (TestQuestManager.Instance != null)
        {
            TestQuestManager.Instance.CompleteQuest(questId);
            Debug.Log($"任务 {questId} 已完成");
            hasWarnedAboutMissingManager = false; // 重置警告标志
        }
        else
        {
            if (!hasWarnedAboutMissingManager)
            {
                Debug.LogWarning("TestQuestManager 未找到！请确保场景中有 TestQuestManager 对象。");
                hasWarnedAboutMissingManager = true;
            }
        }
        
        UpdateQuestStatus();
    }
    
    /// <summary>
    /// 重置任务
    /// </summary>
    public void ResetQuest(string questId)
    {
        if (TestQuestManager.Instance != null)
        {
            TestQuestManager.Instance.ResetQuest(questId);
            Debug.Log($"任务 {questId} 已重置");
            hasWarnedAboutMissingManager = false; // 重置警告标志
        }
        else
        {
            if (!hasWarnedAboutMissingManager)
            {
                Debug.LogWarning("TestQuestManager 未找到！请确保场景中有 TestQuestManager 对象。");
                hasWarnedAboutMissingManager = true;
            }
        }
        
        UpdateQuestStatus();
    }
    
    /// <summary>
    /// 切换任务状态
    /// </summary>
    public void ToggleQuestStatus()
    {
        if (TestQuestManager.Instance != null)
        {
            bool isCompleted = TestQuestManager.Instance.IsQuestCompleted(questIdToTest);
            if (isCompleted)
            {
                ResetQuest(questIdToTest);
            }
            else
            {
                CompleteQuest(questIdToTest);
            }
        }
        else
        {
            if (!hasWarnedAboutMissingManager)
            {
                Debug.LogWarning("TestQuestManager 未找到！请确保场景中有 TestQuestManager 对象。");
                hasWarnedAboutMissingManager = true;
            }
        }
    }
    
    /// <summary>
    /// 更新任务状态显示
    /// </summary>
    private void UpdateQuestStatus()
    {
        if (TestQuestManager.Instance != null)
        {
            isQuestCompleted = TestQuestManager.Instance.IsQuestCompleted(questIdToTest);
            
            // 更新所有任务状态
            foreach (var quest in allQuests)
            {
                quest.isCompleted = TestQuestManager.Instance.IsQuestCompleted(quest.questId);
            }
        }
        else
        {
            // 如果TestQuestManager不存在，显示默认状态
            isQuestCompleted = false;
            foreach (var quest in allQuests)
            {
                quest.isCompleted = false;
            }
        }
    }
    
    /// <summary>
    /// 添加新任务到测试列表
    /// </summary>
    public void AddQuestToTest(string questId)
    {
        if (!allQuests.Exists(q => q.questId == questId))
        {
            allQuests.Add(new QuestStatus { questId = questId, isCompleted = false });
            Debug.Log($"已添加任务 {questId} 到测试列表");
        }
    }
    
    /// <summary>
    /// 清除所有任务
    /// </summary>
    public void ClearAllQuests()
    {
        if (TestQuestManager.Instance != null)
        {
            TestQuestManager.Instance.ClearAllQuests();
        }
        
        allQuests.Clear();
    }
    
    /// <summary>
    /// 显示当前所有任务状态
    /// </summary>
    public void ShowAllQuestStatus()
    {
        if (TestQuestManager.Instance != null)
        {
            Debug.Log("=== 当前任务状态 ===");
            foreach (var quest in allQuests)
            {
                bool isCompleted = TestQuestManager.Instance.IsQuestCompleted(quest.questId);
                Debug.Log($"任务 {quest.questId}: {(isCompleted ? "已完成" : "未完成")}");
            }
        }
    }
} 