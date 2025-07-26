using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DialogueClip : PlayableAsset
{
    public DialogueNode startingNode;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);
        DialogueBehaviour dialogueBehaviour = playable.GetBehaviour();
        dialogueBehaviour.startingNode = startingNode;
        return playable;
    }
} 