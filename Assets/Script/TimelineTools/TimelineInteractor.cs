using UnityEngine;
using UnityEngine.Playables;

public class TimelineInteractor : MonoBehaviour, IInteractable
{
    [Tooltip("The PlayableDirector to play when interacted with.")]
    public PlayableDirector timelineToPlay;

    public void Interact()
    {
        if (timelineToPlay != null)
        {
            TimelineManager.Instance.Play(timelineToPlay);
        }
    }
} 