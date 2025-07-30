using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationCanvas : MonoBehaviour
{
    public static NotificationCanvas instance;
    
    [Header("交互提示UI")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image interactIcon; // 可选：用于显示按键图标
    
    [Header("动画设置")]
    [SerializeField] private Animator promptAnimator;
    [SerializeField] private string showTrigger = "Show";
    [SerializeField] private string hideTrigger = "Hide";
    
    private void Awake()
    {
        // 单例模式
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ShowInteractPrompt(string message)
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(true);
            
            if (interactText != null)
            {
                interactText.text = message;
            }
            
            if (promptAnimator != null)
            {
                promptAnimator.SetTrigger(showTrigger);
            }
        }
    }
    
    public void HideInteractPrompt()
    {
        if (interactPrompt != null)
        {
            if (promptAnimator != null)
            {
                promptAnimator.SetTrigger(hideTrigger);
                // 注意：你可能需要在动画结束时才真正隐藏提示
                // 可以通过动画事件或协程来实现
            }
            else
            {
                interactPrompt.SetActive(false);
            }
        }
    }
}