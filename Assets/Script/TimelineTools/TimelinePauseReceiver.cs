using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelinePauseReceiver : MonoBehaviour
{
    private PlayableDirector director;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    // 这个方法会被Timeline信号调用
    public void OnPauseSignal()
    {
        if (director != null)
        {
            director.Pause();
        }
    }
} 