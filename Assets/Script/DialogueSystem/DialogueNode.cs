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
    // 可扩展更多事件
}

[System.Serializable]
public class PlayerChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public DialogueChoiceEventType eventType;
} 