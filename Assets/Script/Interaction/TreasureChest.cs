using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("动画设置")]
    [SerializeField] private Animator animator;
    [SerializeField] private string openTriggerName = "Open";
    
    [Header("特效设置")]
    [SerializeField] private ParticleSystem openEffect;
    [SerializeField] private AudioSource openSound;
    
    [Header("物品设置")]
    [SerializeField] private Transform itemSpawnPoint; // 物品生成点
    [SerializeField] private GameObject[] itemPrefabs; // 物品预制体数组
    [SerializeField] private float spawnHeight = 1f; // 生成高度偏移
    [SerializeField] private float spawnRadius = 0.5f; // 生成半径
    [SerializeField] private int minItems = 1; // 最少生成物品数量
    [SerializeField] private int maxItems = 3; // 最多生成物品数量
    [SerializeField] private bool useRandomItems = true; // 是否随机选择物品
    
    [Header("调试选项")]
    [SerializeField] private bool showDebugLogs = true;
    
    [Header("状态")]
    [SerializeField] private bool isOpened = false;
    
    private void Start()
    {
        // 获取组件（如果没有在Inspector中指定）
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null && showDebugLogs)
            {
                Debug.LogWarning("宝箱缺少Animator组件！", this);
            }
        }
        
        if (openSound == null)
        {
            openSound = GetComponent<AudioSource>();
            if (openSound == null && showDebugLogs)
            {
                Debug.LogWarning("宝箱缺少AudioSource组件！", this);
            }
        }
        
        // 如果没有设置物品生成点，使用宝箱位置
        if (itemSpawnPoint == null)
        {
            itemSpawnPoint = transform;
        }
        
        // 验证设置
        ValidateSetup();
    }
    
    private void ValidateSetup()
    {
        if (showDebugLogs)
        {
            if (animator != null)
            {
                // 检查Animator Controller是否设置
                if (animator.runtimeAnimatorController == null)
                {
                    Debug.LogError("宝箱的Animator没有设置Controller！", this);
                }
                else
                {
                    // 检查是否存在Open触发器
                    AnimatorControllerParameter[] parameters = animator.parameters;
                    bool hasOpenTrigger = false;
                    foreach (var param in parameters)
                    {
                        if (param.name == openTriggerName && param.type == AnimatorControllerParameterType.Trigger)
                        {
                            hasOpenTrigger = true;
                            break;
                        }
                    }
                    if (!hasOpenTrigger)
                    {
                        Debug.LogError($"Animator中找不到名为 '{openTriggerName}' 的触发器！", this);
                    }
                }
            }
            
            if (openEffect == null)
            {
                Debug.LogWarning("没有设置开启特效！", this);
            }
            
            // 验证物品设置
            if (itemPrefabs == null || itemPrefabs.Length == 0)
            {
                Debug.LogWarning("没有设置物品预制体！宝箱打开后不会生成物品。", this);
            }
            
            if (minItems > maxItems)
            {
                Debug.LogError("最少物品数量不能大于最多物品数量！", this);
            }
            
            if (minItems < 0 || maxItems < 0)
            {
                Debug.LogError("物品数量不能为负数！", this);
            }
        }
    }
    
    public void Interact()
    {
        if (!isOpened)
        {
            OpenChest();
        }
    }
    
    private void OpenChest()
    {
        if (showDebugLogs)
        {
            Debug.Log("尝试打开宝箱...", this);
        }
        
        isOpened = true;
        
        // 播放动画
        if (animator != null)
        {
            if (showDebugLogs)
            {
                Debug.Log($"触发动画: {openTriggerName}", this);
            }
            animator.SetTrigger(openTriggerName);
        }
        else if (showDebugLogs)
        {
            Debug.LogError("无法播放动画：Animator组件丢失", this);
        }
        
        // 播放特效
        if (openEffect != null)
        {
            if (showDebugLogs)
            {
                Debug.Log("播放开启特效", this);
            }
            openEffect.Play();
        }
        else if (showDebugLogs)
        {
            Debug.LogWarning("无法播放特效：特效组件未设置", this);
        }
        
        // 播放音效
        if (openSound != null)
        {
            if (showDebugLogs)
            {
                Debug.Log("播放开启音效", this);
            }
            openSound.Play();
        }
        else if (showDebugLogs)
        {
            Debug.LogWarning("无法播放音效：AudioSource组件丢失", this);
        }
        
        // 生成物品
        SpawnItems();
    }
    
    public bool IsOpened()
    {
        return isOpened;
    }
    
    /// <summary>
    /// 生成物品
    /// </summary>
    private void SpawnItems()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0)
        {
            if (showDebugLogs)
            {
                Debug.LogWarning("没有设置物品预制体，跳过物品生成", this);
            }
            return;
        }
        
        // 确定生成物品数量
        int itemCount = Random.Range(minItems, maxItems + 1);
        
        if (showDebugLogs)
        {
            Debug.Log($"开始生成 {itemCount} 个物品", this);
        }
        
        for (int i = 0; i < itemCount; i++)
        {
            // 选择要生成的物品
            GameObject itemToSpawn = SelectItemToSpawn();
            
            if (itemToSpawn != null)
            {
                // 计算生成位置
                Vector3 spawnPosition = CalculateSpawnPosition(i, itemCount);
                
                // 生成物品
                GameObject spawnedItem = Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
                
                if (showDebugLogs)
                {
                    Debug.Log($"生成物品: {itemToSpawn.name} 在位置: {spawnPosition}", this);
                }
            }
        }
    }
    
    /// <summary>
    /// 选择要生成的物品
    /// </summary>
    private GameObject SelectItemToSpawn()
    {
        if (itemPrefabs.Length == 0) return null;
        
        if (useRandomItems)
        {
            // 随机选择一个物品
            return itemPrefabs[Random.Range(0, itemPrefabs.Length)];
        }
        else
        {
            // 按顺序选择物品
            return itemPrefabs[0]; // 简化版本，总是选择第一个
        }
    }
    
    /// <summary>
    /// 计算物品生成位置
    /// </summary>
    private Vector3 CalculateSpawnPosition(int itemIndex, int totalItems)
    {
        Vector3 basePosition = itemSpawnPoint.position;
        
        // 添加高度偏移
        basePosition.y += spawnHeight;
        
        // 如果有多个物品，在圆形区域内分布
        if (totalItems > 1)
        {
            float angle = (360f / totalItems) * itemIndex;
            float radians = angle * Mathf.Deg2Rad;
            
            float xOffset = Mathf.Cos(radians) * spawnRadius;
            float zOffset = Mathf.Sin(radians) * spawnRadius;
            
            basePosition.x += xOffset;
            basePosition.z += zOffset;
        }
        
        return basePosition;
    }
    
    // 用于在编辑器中验证设置
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(openTriggerName))
        {
            openTriggerName = "Open";
        }
        
        // 确保数值合理
        if (minItems < 0) minItems = 0;
        if (maxItems < minItems) maxItems = minItems;
        if (spawnHeight < 0) spawnHeight = 0;
        if (spawnRadius < 0) spawnRadius = 0;
    }
    
    // 在Scene视图中显示生成区域
    private void OnDrawGizmosSelected()
    {
        if (itemSpawnPoint != null)
        {
            // 显示生成点
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(itemSpawnPoint.position, 0.1f);
            
            // 显示生成区域
            Vector3 spawnAreaCenter = itemSpawnPoint.position + Vector3.up * spawnHeight;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spawnAreaCenter, spawnRadius);
            
            // 显示高度线
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(itemSpawnPoint.position, spawnAreaCenter);
        }
    }
}