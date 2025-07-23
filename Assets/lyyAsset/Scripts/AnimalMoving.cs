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

        // ��ֵ XZ��Y ���ɵ��ξ���
        Vector3 posXZ = Vector3.Lerp(
            new Vector3(startPoint.position.x, 0, startPoint.position.z),
            new Vector3(endPoint.position.x, 0, endPoint.position.z),
            fracJourney
        );

        // ��ȡ���� Y ֵ
        float terrainY = Terrain.activeTerrain.SampleHeight(posXZ);
        transform.position = new Vector3(posXZ.x, terrainY, posXZ.z);

        if (fracJourney >= 1f)
        {
            isPaused = true;
            pauseStartTime = Time.time;

            // ת��
            transform.Rotate(0, 180, 0);

            // ����յ㻥��
            Transform temp = startPoint;
            startPoint = endPoint;
            endPoint = temp;
        }
    }

    /// <summary>
    /// ��ȡ���α����ϵ�λ�ã��Զ����أ�
    /// </summary>
    private Vector3 GetPositionOnTerrain(Vector3 originalPos)
    {
        float terrainY = Terrain.activeTerrain.SampleHeight(originalPos);
        return new Vector3(originalPos.x, terrainY, originalPos.z);
    }
}
