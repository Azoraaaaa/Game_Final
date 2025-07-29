using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string speakerName;
    [TextArea(3, 10)]
    public string sentence;
    public List<PlayerChoice> choices;
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