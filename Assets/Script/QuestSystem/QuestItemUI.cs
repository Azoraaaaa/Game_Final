using UnityEngine;
using TMPro;

// 任务项UI组件，只负责显示标题
public class QuestItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text titleText;
    
    private QuestData questData;
    public string QuestID => questData?.questID;
    
    public void SetupQuest(QuestData quest)
    {
        questData = quest;
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (questData == null) return;
        
        // 只设置标题
        if (titleText != null)
            titleText.text = questData.questTitle;
    }
} 