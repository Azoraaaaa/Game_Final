using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [Header("Dialogue Content")]
    [Tooltip("Multiple consecutive dialogue sentences. Each sentence will be displayed one by one. If there's only one sentence, you can just fill in the first line.")]
    [TextArea(3, 10)]
    public string[] sentences = new string[1];
    
    [Header("Speaker Settings")]
    [Tooltip("Speaker name for each sentence. Array length should match the sentences array. If empty, the default speaker will be used.")]
    public string[] speakerNames = new string[1];
    
    [Header("Default Speaker")]
    [Tooltip("Default speaker name used when speakerNames array is empty or the corresponding position is empty")]
    public string defaultSpeakerName = "";
    
    [Header("Player Choices")]
    [Tooltip("Player choice options, displayed after all sentences are played")]
    public List<PlayerChoice> choices;
    
    // 为了向后兼容，保留原有的speakerName字段作为属性
    public string speakerName
    {
        get { return defaultSpeakerName; }
        set { defaultSpeakerName = value; }
    }
    
    // 为了向后兼容，保留原有的sentence字段作为属性
    public string sentence
    {
        get { return sentences != null && sentences.Length > 0 ? sentences[0] : ""; }
        set 
        { 
            if (sentences == null || sentences.Length == 0)
                sentences = new string[1];
            sentences[0] = value; 
        }
    }
    
    /// <summary>
    /// Get speaker name for the specified index
    /// </summary>
    /// <param name="index">Sentence index</param>
    /// <returns>Speaker name</returns>
    public string GetSpeakerName(int index)
    {
        if (speakerNames != null && index >= 0 && index < speakerNames.Length && !string.IsNullOrEmpty(speakerNames[index]))
        {
            return speakerNames[index];
        }
        return defaultSpeakerName;
    }
    
    /// <summary>
    /// Validate data integrity
    /// </summary>
    /// <returns>Whether the data is valid</returns>
    public bool IsValid()
    {
        if (sentences == null || sentences.Length == 0)
            return false;
            
        // Check if there are valid sentences
        bool hasValidSentence = false;
        foreach (string sentence in sentences)
        {
            if (!string.IsNullOrEmpty(sentence))
            {
                hasValidSentence = true;
                break;
            }
        }
        
        return hasValidSentence;
    }
}

public enum DialogueChoiceEventType
{
    None,
    OpenShop,
    OpenBag,
    CloseShop,
    CloseBag,
    StartQuest // 添加一个新的事件类型
}

[System.Serializable]
public class PlayerChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public DialogueChoiceEventType eventType;
    
    [Tooltip("A string parameter used by the event. For StartQuest, this is the Quest ID.")]
    public string stringParameter; // 新增的字符串参数字段
} 