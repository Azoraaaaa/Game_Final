using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    public DialogueData dialogueData;
    private bool isClipPlayed = false;

    public override void OnPlayableCreate(Playable playable)
    {
        isClipPlayed = false;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (dialogueData != null && !isClipPlayed && info.weight > 0)
        {
            DialogueManager.instance.StartDialogue(dialogueData);
            isClipPlayed = true;
        }
    }
} 