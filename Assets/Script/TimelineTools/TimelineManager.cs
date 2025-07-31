using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    private bool isInDialogue = false;
    private PlayableDirector currentTimeline;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(PlayableDirector timeline)
    {
        if (timeline != null)
        {
            currentTimeline = timeline;
            timeline.Play();
        }
        else
        {
            Debug.LogWarning("Attempted to play a null timeline!");
        }
    }

    public void StartDialogue()
    {
        isInDialogue = true;
        if (currentTimeline != null)
        {
            currentTimeline.Pause();
        }
    }

    public void EndDialogue()
    {
        isInDialogue = false;
        if (currentTimeline != null)
        {
            currentTimeline.Resume();
        }
    }

    public bool IsInDialogue()
    {
        return isInDialogue;
    }
}