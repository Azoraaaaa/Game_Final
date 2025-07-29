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
    
    // (因为我们不再显示详细进度，这个方法可以暂时留空或移除)
    public void UpdateQuestProgress(string questID)
    {
        // No visual progress update needed for the simplified version.
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