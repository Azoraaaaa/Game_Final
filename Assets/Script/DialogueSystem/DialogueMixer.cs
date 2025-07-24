using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (playerData is DialogueManager)
        {
            // Nothing to do here, the magic happens in DialogueBehaviour
        }
    }
} 