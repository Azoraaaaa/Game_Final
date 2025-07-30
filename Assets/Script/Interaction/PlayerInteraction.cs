using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("交互设置")]
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    
    [Header("射线检测")]
    [SerializeField] private Transform raycastOrigin; // 通常是玩家的相机或眼睛位置
    
    // 当前可交互物体
    private IInteractable currentInteractable;
    
    private void Start()
    {
        // 如果没有指定射线起点，使用玩家相机
        if (raycastOrigin == null)
        {
            Camera playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                raycastOrigin = playerCamera.transform;
            }
            else
            {
                raycastOrigin = transform;
            }
        }
    }
    
    private void Update()
    {
        CheckForInteractable();
        HandleInteraction();
    }
    
    private void CheckForInteractable()
    {
        RaycastHit hit;
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        
        // 绘制射线（仅在编辑器中可见，用于调试）
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.yellow);
        
        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                // 检查是否是新的可交互物体
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
                    
                    // 显示交互提示
                    if (NotificationCanvas.instance != null)
                    {
                        // 如果是宝箱，检查是否已经打开
                        TreasureChest chest = hit.collider.GetComponent<TreasureChest>();
                        if (chest != null && !chest.IsOpened())
                        {
                            NotificationCanvas.instance.ShowInteractPrompt("按 E 打开宝箱");
                        }
                    }
                }
            }
            else
            {
                ClearInteractable();
            }
        }
        else
        {
            ClearInteractable();
        }
    }
    
    private void HandleInteraction()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact();
            
            // 如果是宝箱，检查是否已经打开，如果打开了就清除提示
            TreasureChest chest = currentInteractable as TreasureChest;
            if (chest != null && chest.IsOpened())
            {
                ClearInteractable();
            }
        }
    }
    
    private void ClearInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            
            // 清除交互提示
            if (NotificationCanvas.instance != null)
            {
                NotificationCanvas.instance.HideInteractPrompt();
            }
        }
    }
}