using UnityEngine;

public class BarrierChecker : MonoBehaviour
{
    public Vector3 rayOffset = new Vector3(0, 0.2f, 0);
    public float rayLength = 0.9f;
    public LayerMask BarrierLayer;
    public float HeightRayLength = 6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BarrierInfo CheckBarrier()
    {
        var hitData = new BarrierInfo();

        var rayOrigin = transform.position + rayOffset; //start from 0.2f above the player feet
        //ray facing forward
        hitData.hitFound = Physics.Raycast(rayOrigin, transform.forward, out hitData.hitInfo, rayLength, BarrierLayer);

        Debug.DrawRay(rayOrigin, transform.forward, (hitData.hitFound) ? Color.red : Color.green);

        if(hitData.hitFound)
        {
            //create ray from hit point. It will appear from 6f above
            var heightOrigin = hitData.hitInfo.point + Vector3.up * HeightRayLength;
            hitData.HeightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightInfo, HeightRayLength, BarrierLayer);

            Debug.DrawRay(heightOrigin, Vector3.down * HeightRayLength, (hitData.HeightHitFound) ? Color.blue : Color.green);
        }

        return hitData;
    }

    public struct BarrierInfo
    {
        public bool hitFound;
        public RaycastHit hitInfo;
        public RaycastHit heightInfo;
        public bool HeightHitFound;
    }
}
