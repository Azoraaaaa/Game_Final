using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [Header("Dependencies")]
    [Tooltip("Reference to the player's control script. It will be disabled during cutscenes.")]
    public MonoBehaviour playerController; 
    
    [Tooltip("Reference to the main game UI canvas group. It will be hidden during cutscenes.")]
    public CanvasGroup mainUIGroup;

    private PlayableDirector activeDirector;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Play(PlayableDirector director)
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (mainUIGroup != null)
        {
            mainUIGroup.alpha = 0;
            mainUIGroup.interactable = false;
            mainUIGroup.blocksRaycasts = false;
        }

        activeDirector = director;
        activeDirector.Play();
        
        activeDirector.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (director == activeDirector)
        {
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            if (mainUIGroup != null)
            {
                mainUIGroup.alpha = 1;
                mainUIGroup.interactable = true;
                mainUIGroup.blocksRaycasts = true;
            }
            
            director.stopped -= OnTimelineFinished;
            activeDirector = null;
        }
    }
} 