using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [Header("Dialogue Content")]
    public string speakerName;
    [TextArea(3, 10)]
    public string sentence;

    [Header("Player Choices")]
    public List<PlayerChoice> choices;
}

[System.Serializable]
public class PlayerChoice
{
    [Tooltip("The text that will appear on the button for this choice.")]
    public string choiceText;

    [Tooltip("The next dialogue node to go to if this choice is selected.")]
    public DialogueNode nextNode;

    [Tooltip("Event(s) to trigger when this choice is selected. Can be used to open shops, give items, etc.")]
    public UnityEvent onChoiceSelected;
} 