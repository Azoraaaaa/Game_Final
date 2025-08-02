using UnityEngine;
using UnityEngine.Playables;

public class AreaManager : MonoBehaviour
{
    public AreaZone[] zones; // �ֶ������ĸ�����
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
                return; // ��һ��û��ɾͲ�����
        }

        // ����������ȷ������Timeline
        if (bossUnlockTimeline != null && bossUnlockTimeline.state != PlayState.Playing)
        {
            bossCanvas.SetActive(true); 
            bossUnlockTimeline.Play();
        }
    }
}
