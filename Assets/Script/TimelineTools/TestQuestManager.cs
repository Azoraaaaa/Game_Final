using UnityEngine;
using System.Collections.Generic;

public class TestQuestManager : MonoBehaviour
{
    public static TestQuestManager Instance { get; private set; }

    private HashSet<string> completedQuests = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsQuestCompleted(string questId)
    {
        return completedQuests.Contains(questId);
    }

    public void CompleteQuest(string questId)
    {
        completedQuests.Add(questId);
        Debug.Log($"任务 {questId} 已标记为完成");
    }

    public void ResetQuest(string questId)
    {
        completedQuests.Remove(questId);
        Debug.Log($"任务 {questId} 已重置");
    }
    
    /// <summary>
    /// 清除所有任务状态
    /// </summary>
    public void ClearAllQuests()
    {
        completedQuests.Clear();
        Debug.Log("所有任务状态已清除");
    }
    
    /// <summary>
    /// 获取所有已完成的任务
    /// </summary>
    public List<string> GetCompletedQuests()
    {
        return new List<string>(completedQuests);
    }
    
    /// <summary>
    /// 获取任务完成数量
    /// </summary>
    public int GetCompletedQuestCount()
    {
        return completedQuests.Count;
    }
    
    /// <summary>
    /// 检查任务是否存在（无论是否完成）
    /// </summary>
    public bool HasQuest(string questId)
    {
        // 在测试版本中，我们假设所有被查询的任务都存在
        return true;
    }
}