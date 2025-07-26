using UnityEngine;

public class DialogueInteractor : MonoBehaviour, IInteractable
{
    [Tooltip("The first dialogue node to start the conversation with.")]
    public DialogueNode startingNode;

    public void Interact()
    {
        if (startingNode != null)
        {
            DialogueManager.Instance.StartDialogue(startingNode);
        }
    }
} 