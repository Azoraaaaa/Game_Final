using UnityEngine;

/// <summary>
/// QuestUI控制器，用于管理任务面板的显示和隐藏
/// </summary>
public class QuestUIController : MonoBehaviour
{
    [Header("UI控制")]
    [SerializeField] private QuestUI questUI;
    [SerializeField] private KeyCode toggleKey = KeyCode.J; // 默认按J键切换任务面板
    
    [Header("初始状态")]
    [SerializeField] private bool showOnStart = true; // 是否在游戏开始时显示
    
    private void Start()
    {
        // 如果没有指定QuestUI，尝试自动获取
        if (questUI == null)
        {
            questUI = FindObjectOfType<QuestUI>();
        }
        
        // 设置初始状态
        if (questUI != null)
        {
            if (showOnStart)
            {
                questUI.ShowQuestPanel();
            }
            else
            {
                questUI.HideQuestPanel();
            }
        }
    }
    
    private void Update()
    {
        // 检测按键输入
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleQuestPanel();
        }
    }
    
    /// <summary>
    /// 切换任务面板显示状态
    /// </summary>
    public void ToggleQuestPanel()
    {
        if (questUI != null)
        {
            questUI.ToggleQuestPanel();
        }
    }
    
    /// <summary>
    /// 显示任务面板
    /// </summary>
    public void ShowQuestPanel()
    {
        if (questUI != null)
        {
            questUI.ShowQuestPanel();
        }
    }
    
    /// <summary>
    /// 隐藏任务面板
    /// </summary>
    public void HideQuestPanel()
    {
        if (questUI != null)
        {
            questUI.HideQuestPanel();
        }
    }
} 