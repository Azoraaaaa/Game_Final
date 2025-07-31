using UnityEngine;

/// <summary>
/// Timeline系统自动设置助手
/// 这个脚本帮助您快速设置Timeline系统所需的所有组件
/// </summary>
public class TimelineSystemSetup : MonoBehaviour
{
    [Header("自动设置")]
    [SerializeField] private bool autoSetupOnStart = true;
    
    [Header("管理器设置")]
    [SerializeField] private bool createTestQuestManager = true;
    [SerializeField] private bool createTimelineManager = true;
    [SerializeField] private bool createQuestTestManager = true;
    
    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupTimelineSystem();
        }
    }
    
    /// <summary>
    /// 自动设置Timeline系统
    /// </summary>
    public void SetupTimelineSystem()
    {
        Debug.Log("开始设置Timeline系统...");
        
        // 检查并创建TestQuestManager
        if (createTestQuestManager && TestQuestManager.Instance == null)
        {
            CreateTestQuestManager();
        }
        
        // 检查并创建TimelineManager
        if (createTimelineManager && TimelineManager.Instance == null)
        {
            CreateTimelineManager();
        }
        
        // 检查并创建QuestTestManager
        if (createQuestTestManager && FindObjectOfType<QuestTestManager>() == null)
        {
            CreateQuestTestManager();
        }
        
        Debug.Log("Timeline系统设置完成！");
    }
    
    /// <summary>
    /// 创建TestQuestManager
    /// </summary>
    private void CreateTestQuestManager()
    {
        GameObject questManagerObj = new GameObject("TestQuestManager");
        questManagerObj.AddComponent<TestQuestManager>();
        Debug.Log("已创建 TestQuestManager");
    }
    
    /// <summary>
    /// 创建TimelineManager
    /// </summary>
    private void CreateTimelineManager()
    {
        GameObject timelineManagerObj = new GameObject("TimelineManager");
        timelineManagerObj.AddComponent<TimelineManager>();
        Debug.Log("已创建 TimelineManager");
    }
    
    /// <summary>
    /// 创建QuestTestManager
    /// </summary>
    private void CreateQuestTestManager()
    {
        GameObject questTestManagerObj = new GameObject("QuestTestManager");
        questTestManagerObj.AddComponent<QuestTestManager>();
        Debug.Log("已创建 QuestTestManager");
    }
    
    /// <summary>
    /// 检查系统状态
    /// </summary>
    public void CheckSystemStatus()
    {
        Debug.Log("=== Timeline系统状态检查 ===");
        
        if (TestQuestManager.Instance != null)
        {
            Debug.Log("✓ TestQuestManager 已存在");
        }
        else
        {
            Debug.LogWarning("✗ TestQuestManager 未找到");
        }
        
        if (TimelineManager.Instance != null)
        {
            Debug.Log("✓ TimelineManager 已存在");
        }
        else
        {
            Debug.LogWarning("✗ TimelineManager 未找到");
        }
        
        if (FindObjectOfType<QuestTestManager>() != null)
        {
            Debug.Log("✓ QuestTestManager 已存在");
        }
        else
        {
            Debug.LogWarning("✗ QuestTestManager 未找到");
        }
        
        Debug.Log("=== 检查完成 ===");
    }
} 