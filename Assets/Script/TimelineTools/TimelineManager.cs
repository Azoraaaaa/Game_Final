using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [Header("Dependencies")]
    [Tooltip("Reference to the player's control script. It will be disabled during cutscenes.")]
    public MonoBehaviour playerController;
    
    [Tooltip("Reference to the main game UI canvas group. It will be hidden during cutscenes.")]
    public CanvasGroup mainUIGroup;

    private PlayableDirector activeDirector;
    private bool wasInGameMode;
    private bool isInDialogue = false;

    public bool IsInCutscene => activeDirector != null || isInDialogue;

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

    void OnEnable()
    {
        // 确保在启用时清理任何可能的残留状态
        CleanupTimelineResources();
    }

    void OnDisable()
    {
        // 确保在禁用时清理资源
        CleanupTimelineResources();
    }

    private void CleanupTimelineResources()
    {
        if (activeDirector != null)
        {
            activeDirector.stopped -= OnTimelineFinished;
            if (activeDirector.playableGraph.IsValid())
            {
                activeDirector.playableGraph.Destroy();
            }
            activeDirector = null;
        }

        // 强制进行垃圾回收以清理任何残留的Timeline资源
        System.GC.Collect();
    }

    public void Play(PlayableDirector director)
    {
        // 在播放新的Timeline之前清理旧的
        CleanupTimelineResources();

        // 记住之前的模式
        wasInGameMode = !InputManager.Instance.IsUIMode;
        
        // 切换到UI模式
        InputManager.Instance.SetUIMode();

        DisablePlayerAndUI();

        activeDirector = director;

        // 确保Timeline资源正确初始化
        if (!activeDirector.playableGraph.IsValid())
        {
            activeDirector.RebuildGraph();
        }

        activeDirector.stopped += OnTimelineFinished;
        activeDirector.Play();
    }

    public void StartDialogue()
    {
        isInDialogue = true;
        wasInGameMode = !InputManager.Instance.IsUIMode;
        InputManager.Instance.SetUIMode();
        DisablePlayerAndUI();
    }

    public void EndDialogue()
    {
        isInDialogue = false;
        RestorePlayerAndUI();
    }

    private void DisablePlayerAndUI()
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
    }

    private void RestorePlayerAndUI()
    {
        // 只有在不处于任何cutscene或对话时才恢复
        if (!IsInCutscene)
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
            
            // 如果之前是游戏模式，则恢复
            if (wasInGameMode)
            {
                InputManager.Instance.SetGameMode();
            }
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (director == activeDirector)
        {
            activeDirector.stopped -= OnTimelineFinished;
            
            // 确保清理Timeline资源
            if (activeDirector.playableGraph.IsValid())
            {
                activeDirector.playableGraph.Destroy();
            }
            
            activeDirector = null;
            
            // 只有在不在对话状态时才恢复
            if (!isInDialogue)
            {
                RestorePlayerAndUI();
            }
        }
    }

    void OnApplicationQuit()
    {
        CleanupTimelineResources();
    }
} 