using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class QuestUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject questPanel;
    public Transform mainQuestContainer;
    public Transform sideQuestContainer;
    public GameObject questItemPrefab;
    
    [Header("Quest Item References")]
    public TMP_Text questTitleText;
    public TMP_Text questDescriptionText;
    public TMP_Text questProgressText;
    public Image questTypeIcon;
    public Image questStatusIcon;
    
    private List<GameObject> activeQuestItems = new List<GameObject>();
    
    void Start()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }
    
    // 更新任务列表
    public void UpdateQuestList()
    {
        ClearQuestItems();
        
        if (QuestManager.Instance == null) return;
        
        // 显示主线任务
        var mainQuests = QuestManager.Instance.GetActiveQuests().FindAll(q => q.questType == QuestType.Main);
        foreach (var quest in mainQuests)
        {
            CreateQuestItem(quest, mainQuestContainer);
        }
        
        // 显示支线任务
        var sideQuests = QuestManager.Instance.GetActiveQuests().FindAll(q => q.questType == QuestType.Side);
        foreach (var quest in sideQuests)
        {
            CreateQuestItem(quest, sideQuestContainer);
        }
    }
    
    // 创建任务项
    private void CreateQuestItem(QuestData quest, Transform container)
    {
        GameObject questItem = Instantiate(questItemPrefab, container);
        activeQuestItems.Add(questItem);
        
        // 设置任务信息
        var questItemUI = questItem.GetComponent<QuestItemUI>();
        if (questItemUI != null)
        {
            questItemUI.SetupQuest(quest);
        }
    }
    
    // 更新特定任务的进度
    public void UpdateQuestProgress(string questID)
    {
        foreach (var questItem in activeQuestItems)
        {
            var questItemUI = questItem.GetComponent<QuestItemUI>();
            if (questItemUI != null && questItemUI.QuestID == questID)
            {
                questItemUI.UpdateProgress();
                break;
            }
        }
    }
    
    // 清除任务项
    private void ClearQuestItems()
    {
        foreach (var item in activeQuestItems)
        {
            Destroy(item);
        }
        activeQuestItems.Clear();
    }
    
    // 显示/隐藏任务面板
    public void ToggleQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(!questPanel.activeSelf);
            if (questPanel.activeSelf)
            {
                UpdateQuestList();
            }
        }
    }
    
    // 显示任务面板
    public void ShowQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(true);
            UpdateQuestList();
        }
    }
    
    // 隐藏任务面板
    public void HideQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }
}

// 任务项UI组件
public class QuestItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text progressText;
    public Image typeIcon;
    public Image statusIcon;
    
    [Header("Icons")]
    public Sprite mainQuestIcon;
    public Sprite sideQuestIcon;
    public Sprite inProgressIcon;
    public Sprite completedIcon;
    
    private QuestData questData;
    public string QuestID => questData?.questID;
    
    public void SetupQuest(QuestData quest)
    {
        questData = quest;
        UpdateDisplay();
    }
    
    public void UpdateProgress()
    {
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (questData == null) return;
        
        // 设置标题和描述
        if (titleText != null)
            titleText.text = questData.questTitle;
        
        if (descriptionText != null)
            descriptionText.text = questData.questDescription;
        
        // 设置进度
        if (progressText != null)
        {
            int completedObjectives = questData.objectives.Count(o => o.isCompleted);
            int totalObjectives = questData.objectives.Count;
            progressText.text = $"{completedObjectives}/{totalObjectives}";
        }
        
        // 设置图标
        if (typeIcon != null)
        {
            typeIcon.sprite = questData.questType == QuestType.Main ? mainQuestIcon : sideQuestIcon;
        }
        
        if (statusIcon != null)
        {
            QuestStatus status = QuestManager.Instance.GetQuestStatus(questData.questID);
            statusIcon.sprite = status == QuestStatus.Completed ? completedIcon : inProgressIcon;
        }
    }
} 