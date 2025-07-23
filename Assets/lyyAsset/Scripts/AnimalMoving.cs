using UnityEngine;

public class AnimalMoving : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;
    public float pauseDuration = 1f;

    private float startTime;
    private bool isPaused = false;
    private float pauseStartTime;

    void Start()
    {
        transform.position = GetPositionOnTerrain(startPoint.position);
        startTime = Time.time;
    }

    void Update()
    {
        if (isPaused)
        {
            if (Time.time - pauseStartTime >= pauseDuration)
            {
                isPaused = false;
                startTime = Time.time;
            }
            else
                return;
        }

        float journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;

        // 插值 XZ，Y 轴由地形决定
        Vector3 posXZ = Vector3.Lerp(
            new Vector3(startPoint.position.x, 0, startPoint.position.z),
            new Vector3(endPoint.position.x, 0, endPoint.position.z),
            fracJourney
        );

        // 获取贴地 Y 值
        float terrainY = Terrain.activeTerrain.SampleHeight(posXZ);
        transform.position = new Vector3(posXZ.x, terrainY, posXZ.z);

        if (fracJourney >= 1f)
        {
            isPaused = true;
            pauseStartTime = Time.time;

            // 转身
            transform.Rotate(0, 180, 0);

            // 起点终点互换
            Transform temp = startPoint;
            startPoint = endPoint;
            endPoint = temp;
        }
    }

    /// <summary>
    /// 获取地形表面上的位置（自动贴地）
    /// </summary>
    private Vector3 GetPositionOnTerrain(Vector3 originalPos)
    {
        float terrainY = Terrain.activeTerrain.SampleHeight(originalPos);
        return new Vector3(originalPos.x, terrainY, originalPos.z);
    }
}
