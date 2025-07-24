using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class TimelineTrigger : MonoBehaviour
{
    [Tooltip("The PlayableDirector to trigger when the player enters.")]
    public PlayableDirector timelineToPlay;

    [Tooltip("The tag of the object that can trigger this timeline (e.g., 'Player').")]
    public string triggerTag = "Player";

    [Tooltip("Should this trigger only once?")]
    public bool playOnce = true;

    private bool hasPlayed = false;

    private void Awake()
    {
        // Ensure the collider is set to be a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timelineToPlay == null) return;
        
        if (other.CompareTag(triggerTag))
        {
            if (playOnce && hasPlayed)
            {
                return;
            }

            TimelineManager.Instance.Play(timelineToPlay);
            hasPlayed = true;
        }
    }
} 