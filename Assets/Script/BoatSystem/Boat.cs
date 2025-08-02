using UnityEngine;

public class Boat : MonoBehaviour
{
    [Header("船只设置")]
    [SerializeField] private float boatSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Transform playerStandPoint;
    [SerializeField] private Animator boatAnimator;
    
    [Header("检测设置")]
    [SerializeField] private float dockingDistance = 2f;
    [SerializeField] private LayerMask dockLayer;
    
    [Header("调试")]
    [SerializeField] private bool showDebugLogs = true;
    
    private bool isOccupied = false;
    private PlayerController playerController;
    private bool canDock = false;
    private Camera mainCamera;
    
    // 保存玩家原始状态
    private Transform originalPlayerParent;
    private bool originalCanMove;
    private CharacterController playerCC;
    private Animator playerAnimator;
    
    // 保存船只原始状态
    private bool originalBoatAnimatorEnabled;
    
    private void Start()
    {
        ValidateSetup();
        mainCamera = Camera.main;
        
        // 如果没有手动设置船只Animator，尝试自动获取
        if (boatAnimator == null)
        {
            boatAnimator = GetComponent<Animator>();
        }
    }
    
    private void LateUpdate()
    {
        if (isOccupied)
        {
            HandleBoatMovement();
            CheckForDocking();
            
            // 确保玩家始终在正确位置和姿态
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        if (playerController != null && playerStandPoint != null)
        {
            // 确保CharacterController禁用
            if (playerCC != null && playerCC.enabled)
            {
                playerCC.enabled = false;
            }

            // 直接设置位置和旋转
            playerController.transform.position = playerStandPoint.position;
            playerController.transform.rotation = playerStandPoint.rotation;

            // 确保动画正确
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", 0);
                playerAnimator.SetBool("IsJumping", false);
                playerAnimator.SetBool("IsGrounded", true);
                // 强制更新动画器，确保姿势正确
                playerAnimator.Update(0f);
            }
        }
    }
    
    private void HandleBoatMovement()
    {
        if (!isOccupied || playerController == null) return;

        // 获取输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // 获取相机的前向和右向量（忽略Y轴）
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = mainCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // 计算移动方向
            Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

            // 如果有输入，转向移动方向
            if (moveDirection != Vector3.zero)
            {
                // 计算目标旋转
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                
                // 平滑旋转
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // 按照计算出的移动方向移动船只
            transform.Translate(moveDirection * boatSpeed * Time.deltaTime, Space.World);

            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] 船只移动中，方向: {moveDirection}", this);
            }
        }
    }
    
    public bool CanPlayerBoard()
    {
        return !isOccupied;
    }
    
    public void BoardBoat(PlayerController player)
    {
        if (!isOccupied && player != null)
        {
            isOccupied = true;
            playerController = player;
            
            // 缓存组件引用
            playerCC = player.GetComponent<CharacterController>();
            playerAnimator = player.GetComponent<Animator>();
            
            // 保存玩家原始状态
            originalPlayerParent = player.transform.parent;
            originalCanMove = player.canMove;
            
            // 保存船只Animator原始状态
            if (boatAnimator != null)
            {
                originalBoatAnimatorEnabled = boatAnimator.enabled;
                boatAnimator.enabled = false; // 禁用船只Animator
            }
            
            // 禁用玩家移动
            player.canMove = false;
            
            // 禁用CharacterController
            if (playerCC != null)
            {
                playerCC.enabled = false;
            }
            
            // 重置动画状态
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", 0);
                playerAnimator.SetBool("IsJumping", false);
                playerAnimator.SetBool("IsGrounded", true);
                playerAnimator.Update(0f); // 立即更新动画器
            }
            
            // 设置父子关系
            player.transform.SetParent(transform);
            
            // 设置玩家在船上的位置
            if (playerStandPoint != null)
            {
                player.transform.position = playerStandPoint.position;
                player.transform.rotation = playerStandPoint.rotation;
            }
            
            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] 玩家已上船: {player.name}", this);
            }
        }
    }
    
    public void ExitBoat(DockingPoint dockPoint)
    {
        if (isOccupied && playerController != null && dockPoint != null)
        {
            isOccupied = false;
            
            // 移除父子关系
            playerController.transform.SetParent(originalPlayerParent);
            
            // 获取码头的下船点
            Vector3 exitPoint = dockPoint.GetExitPosition();
            
            // 设置玩家位置
            playerController.transform.position = exitPoint;
            playerController.transform.rotation = dockPoint.transform.rotation;
            
            // 重新启用CharacterController
            if (playerCC != null)
            {
                playerCC.enabled = true;
            }
            
            // 重新启用玩家控制
            playerController.canMove = originalCanMove;
            
            // 重置动画
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", 0);
                playerAnimator.SetBool("IsJumping", false);
                playerAnimator.SetBool("IsGrounded", true);
            }
            
            // 重新启用船只Animator
            if (boatAnimator != null)
            {
                boatAnimator.enabled = originalBoatAnimatorEnabled;
            }
            
            if (showDebugLogs)
            {
                Debug.Log($"[{gameObject.name}] 玩家已在码头点下船: {dockPoint.name}", this);
            }
            
            // 清除所有引用
            playerController = null;
            playerCC = null;
            playerAnimator = null;
        }
    }
    
    public PlayerController GetPlayer()
    {
        return playerController;
    }
    
    private void CheckForDocking()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, dockingDistance, dockLayer);
        canDock = hitColliders.Length > 0;
    }
    
    public bool IsOccupied()
    {
        return isOccupied;
    }
    
    public bool CanDock()
    {
        return canDock;
    }
    
    private void ValidateSetup()
    {
        if (showDebugLogs)
        {
            if (playerStandPoint == null)
            {
                Debug.LogError($"[{gameObject.name}] 未设置玩家站立点！", this);
            }
            
            if (dockLayer == 0)
            {
                Debug.LogWarning($"[{gameObject.name}] 未设置码头层级！", this);
            }
            
            if (boatAnimator == null)
            {
                Debug.LogWarning($"[{gameObject.name}] 未设置船只Animator！将尝试自动获取。", this);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        // 显示靠岸检测范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, dockingDistance);
        
        // 显示玩家站立点
        if (playerStandPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(playerStandPoint.position, 0.3f);
            Gizmos.DrawLine(transform.position, playerStandPoint.position);
        }
        
        // 显示移动方向（仅在运行时）
        if (Application.isPlaying && isOccupied)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
        }
    }
}



