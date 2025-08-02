using UnityEngine;
using UnityEngine.Playables;

public class AreaManager : MonoBehaviour
{
    public AreaZone[] zones; // 手动拖入四个区域
    public PlayableDirector bossUnlockTimeline;
    public GameObject bossCanvas;

    void Start()
    {
        //bossCanvas.SetActive(false);
    }
    public void CheckAllZones()
    {
        foreach (var zone in zones)
        {
            if (!zone.isCorrect)
                return; // 有一个没完成就不触发
        }

        // 所有区域都正确，播放Timeline
        if (bossUnlockTimeline != null && bossUnlockTimeline.state != PlayState.Playing)
        {
            bossCanvas.SetActive(true); 
            bossUnlockTimeline.Play();
        }
    }
}
