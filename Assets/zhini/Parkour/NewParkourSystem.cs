using UnityEngine;

[CreateAssetMenu(menuName = "Parkour Menu/Create New Parkour Action")]
public class NewParkourSystem : ScriptableObject
{
    [Header("Checking Obstacle Height")]
    public string animationName;
    [SerializeField] string barrierTag;
    [SerializeField] float minimumHeight;
    [SerializeField] float maximumHeight;

    [Header("Rotation to barrier")]
    public bool lookAtBarrier;
    public Quaternion RequiredRotation { get; set; }
    public float parkourActionDelay;

    [Header("Target Matching")]
    public bool allowTargetMatching = true;
    public AvatarTarget compareBodyPart;
    public float compareStartTime;
    public float compareEndTime;
    public Vector3 comparePosition { get; set; }

    public bool CheckBarrierHeight(BarrierChecker.BarrierInfo hitData, Transform player)
    {
        //Check Barrier Tag
        if(!string.IsNullOrEmpty(barrierTag) && hitData.hitInfo.transform.tag != barrierTag)
        {
            return false;
        }

        float checkHeight = hitData.heightInfo.point.y - player.position.y;
        // Debug.Log("Check Height = " + checkHeight);

        if (checkHeight < minimumHeight || checkHeight > maximumHeight)
        {
            return false;
        }
        else
        {
            if (lookAtBarrier)
            {
                Debug.Log("Looking at the barrier");
                RequiredRotation = Quaternion.LookRotation(-hitData.hitInfo.normal);
            }
            if (allowTargetMatching)
            {
                comparePosition = hitData.heightInfo.point;
            }
            return true;
        }
    }
}