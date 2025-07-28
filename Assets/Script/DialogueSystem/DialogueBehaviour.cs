using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    public DialogueNode startingNode;
    private bool isClipPlayed = false;

    public override void OnPlayableCreate(Playable playable)
    {
        isClipPlayed = false;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (startingNode != null && !isClipPlayed && info.weight > 0)
        {
            // Use the new Singleton property 'Instance' and start dialogue with a node
            if(DialogueManager.Instance != null)
            {
                DialogueManager.Instance.StartDialogue(startingNode);
            }
            isClipPlayed = true;
        }
    }
} 