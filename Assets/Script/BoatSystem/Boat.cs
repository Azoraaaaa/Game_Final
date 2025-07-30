using UnityEngine;

public class Boat : MonoBehaviour
{
    [Header("船只设置")]
    [SerializeField] private float boatSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Transform playerStandPoint; // 玩家站立点
    
    [Header("检测设置")]
    [SerializeField] private float dockingDistance = 2f; // 靠岸检测距离
    [SerializeField] private LayerMask dockLayer; // 码头层级
    
    private bool isOccupied = false; // 是否有玩家在船上
    private PlayerController playerController; // 玩家控制器引用
    private Vector3 originalPlayerPosition; // 玩家原始位置
    private bool canDock = false; // 是否可以靠岸
    
    private void Update()
    {
        if (isOccupied)
        {
            HandleBoatMovement();
            CheckForDocking();
        }
    }
    
    private void HandleBoatMovement()
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // 移动船只
        Vector3 movement = transform.forward * vertical * boatSpeed * Time.deltaTime;
        transform.position += movement;
        
        // 旋转船只
        float rotation = horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
        
        // 更新玩家位置到站立点
        if (playerController != null)
        {
            playerController.transform.position = playerStandPoint.position;
            playerController.transform.rotation = transform.rotation;
        }
    }
    
    private void CheckForDocking()
    {
        // 检测是否靠近码头
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, dockingDistance, dockLayer);
        canDock = hitColliders.Length > 0;
        
        // 如果靠近码头并按下交互键，玩家可以下船
        if (canDock && Input.GetKeyDown(KeyCode.E))
        {
            ExitBoat();
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
            originalPlayerPosition = player.transform.position;
            
            // 禁用玩家控制器的移动
            player.SetControl(false);
            
            // 将玩家移动到船上的站立点
            player.transform.position = playerStandPoint.position;
            player.transform.rotation = transform.rotation;
        }
    }
    
    public void ExitBoat()
    {
        if (isOccupied && playerController != null)
        {
            isOccupied = false;
            
            // 找到最近的可下船点
            Vector3 exitPoint = FindNearestDockingPoint();
            
            // 重新启用玩家控制器
            playerController.SetControl(true);
            
            // 将玩家移动到下船点
            playerController.transform.position = exitPoint;
            
            playerController = null;
        }
    }
    
    private Vector3 FindNearestDockingPoint()
    {
        // 在这里可以实现更复杂的下船点查找逻辑
        // 现在简单返回船旁边的点
        return transform.position + transform.right * 2f;
    }
    
    public bool IsOccupied()
    {
        return isOccupied;
    }
    
    public bool CanDock()
    {
        return canDock;
    }
    
    private void OnDrawGizmosSelected()
    {
        // 在Scene视图中显示靠岸检测范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, dockingDistance);
    }
}