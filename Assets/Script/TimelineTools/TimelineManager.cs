using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [Header("Dependencies")]
    [Tooltip("Reference to the player's control script. It will be disabled during cutscenes.")]
    public MonoBehaviour[] playerControlScripts; // 改为数组以支持多个控制脚本
    
    [Tooltip("Reference to the main game UI canvas group. It will be hidden during cutscenes.")]
    public CanvasGroup mainUIGroup;

    private PlayableDirector activeDirector;
    private bool wasInGameMode;
    private bool isWaitingForInput;
    private double pausedTime;

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
        // 记住之前的模式
        wasInGameMode = !InputManager.Instance.IsUIMode;
        
        // 切换到UI模式
        InputManager.Instance.SetUIMode();

        // 禁用所有玩家控制脚本
        DisablePlayerControl();

        if (mainUIGroup != null)
        {
            mainUIGroup.alpha = 0;
            mainUIGroup.interactable = false;
            mainUIGroup.blocksRaycasts = false;
        }

        activeDirector = director;
        
        // 订阅Timeline事件
        activeDirector.played += OnTimelineStarted;
        activeDirector.stopped += OnTimelineFinished;
        activeDirector.paused += OnTimelinePaused;

        // 开始播放
        activeDirector.Play();
    }

    private void DisablePlayerControl()
    {
        if (playerControlScripts != null)
        {
            foreach (var script in playerControlScripts)
            {
                if (script != null)
                {
                    script.enabled = false;
                }
            }
        }
    }

    private void EnablePlayerControl()
    {
        if (playerControlScripts != null)
        {
            foreach (var script in playerControlScripts)
            {
                if (script != null)
                {
                    script.enabled = true;
                }
            }
        }
    }

    private void OnTimelineStarted(PlayableDirector director)
    {
        isWaitingForInput = false;
    }

    private void OnTimelinePaused(PlayableDirector director)
    {
        if (director == activeDirector)
        {
            isWaitingForInput = true;
            pausedTime = director.time;
        }
    }

    public void ContinueTimeline()
    {
        if (isWaitingForInput && activeDirector != null)
        {
            isWaitingForInput = false;
            activeDirector.time = pausedTime;
            activeDirector.Resume();
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        if (director == activeDirector)
        {
            // 恢复玩家控制
            EnablePlayerControl();

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
            
            // 取消订阅所有事件
            director.played -= OnTimelineStarted;
            director.stopped -= OnTimelineFinished;
            director.paused -= OnTimelinePaused;
            
            activeDirector = null;
            isWaitingForInput = false;
        }
    }

    // 用于检查Timeline是否正在等待玩家输入
    public bool IsWaitingForInput()
    {
        return isWaitingForInput;
    }
} 