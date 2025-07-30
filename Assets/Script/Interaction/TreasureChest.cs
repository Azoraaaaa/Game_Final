using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("动画设置")]
    [SerializeField] private Animator animator;
    [SerializeField] private string openTriggerName = "Open";
    
    [Header("特效设置")]
    [SerializeField] private ParticleSystem openEffect;
    [SerializeField] private AudioSource openSound;
    
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
    }
    
    public bool IsOpened()
    {
        return isOpened;
    }
    
    // 用于在编辑器中验证设置
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(openTriggerName))
        {
            openTriggerName = "Open";
        }
    }
}