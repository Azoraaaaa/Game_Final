using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("动画设置")]
    [SerializeField] private Animator animator;
    [SerializeField] private string openTriggerName = "Open";
    
    [Header("特效设置")]
    [SerializeField] private ParticleSystem openEffect;
    [SerializeField] private AudioSource openSound;
    
    [Header("状态")]
    [SerializeField] private bool isOpened = false;
    
    private void Start()
    {
        // 获取组件（如果没有在Inspector中指定）
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (openSound == null)
        {
            openSound = GetComponent<AudioSource>();
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
        isOpened = true;
        
        // 播放动画
        if (animator != null)
        {
            animator.SetTrigger(openTriggerName);
        }
        
        // 播放特效
        if (openEffect != null)
        {
            openEffect.Play();
        }
        
        // 播放音效
        if (openSound != null)
        {
            openSound.Play();
        }
        
        // 这里可以添加奖励物品的逻辑
        Debug.Log("宝箱已打开！");
    }
    
    // 用于检查宝箱是否已经打开
    public bool IsOpened()
    {
        return isOpened;
    }
}