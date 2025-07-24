using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 offsetPosition = new Vector3(0, 2, -5);
    
    [Header("Position Settings")]
    [Range(0.0f, 15f)]
    public float smoothSpeed = 5.0f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;
    
    [Header("Rotation Settings")]
    public float rotationSensitivity = 2f;
    public bool invertY = false;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    
    [Header("Collision Settings")]
    public float collisionRadius = 0.2f;
    public LayerMask collisionLayers;
    
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private float currentDistance;
    private Vector3 smoothVelocity;
    private Vector3 desiredPosition;
    
    void Start()
    {
        currentDistance = -offsetPosition.z;
        
        // 初始化旋转角度
        Vector3 angles = transform.eulerAngles;
        currentRotationX = angles.y;
        currentRotationY = angles.x;
        
        // 确保有碰撞检测层
        if (collisionLayers.value == 0)
            collisionLayers = LayerMask.GetMask("Default");
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        HandleRotationInput();
        HandleZoomInput();
        UpdateCameraPosition();
    }
    
    void HandleRotationInput()
    {
        if (Input.GetMouseButton(1)) // 右键按下时才能旋转
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSensitivity * (invertY ? 1 : -1);
            
            currentRotationX += mouseX;
            currentRotationY = Mathf.Clamp(currentRotationY + mouseY, minVerticalAngle, maxVerticalAngle);
        }
    }
    
    void HandleZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance - scroll * zoomSpeed, minDistance, maxDistance);
    }
    
    void UpdateCameraPosition()
    {
        // 计算理想位置
        Vector3 targetPosition = target.position;
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        Vector3 directionFromTarget = rotation * Vector3.forward;
        desiredPosition = targetPosition - directionFromTarget * currentDistance + Vector3.up * offsetPosition.y;
        
        // 处理碰撞
        RaycastHit hit;
        Vector3 directionToTarget = (targetPosition + Vector3.up * offsetPosition.y - desiredPosition).normalized;
        float distanceToTarget = Vector3.Distance(targetPosition + Vector3.up * offsetPosition.y, desiredPosition);
        
        if (Physics.SphereCast(targetPosition + Vector3.up * offsetPosition.y, collisionRadius, -directionToTarget, out hit, distanceToTarget, collisionLayers))
        {
            desiredPosition = hit.point + directionToTarget * collisionRadius;
        }
        
        // 平滑移动
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, smoothSpeed * Time.deltaTime);
        transform.LookAt(targetPosition + Vector3.up * offsetPosition.y);
    }
} 