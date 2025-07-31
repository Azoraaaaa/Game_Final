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
    }

    public void ResetQuest(string questId)
    {
        completedQuests.Remove(questId);
    }
}