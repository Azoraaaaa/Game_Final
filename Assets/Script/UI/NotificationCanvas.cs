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
            DontDestroyOnLoad(gameObject); // 确保不被销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ShowInteractPrompt(string message)
    {
        Debug.Log($"ShowInteractPrompt 被调用，消息: {message}");
        
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(true);
            Debug.Log("interactPrompt 已激活");
            
            if (interactText != null)
            {
                interactText.text = message;
                Debug.Log($"interactText 已设置: {message}");
            }
            else
            {
                Debug.LogWarning("interactText 为空！");
            }
            
            // 检查动画控制器是否存在并且有对应的参数
            if (promptAnimator != null && HasAnimatorParameter(promptAnimator, showTrigger))
            {
                promptAnimator.SetTrigger(showTrigger);
                Debug.Log($"动画触发器 {showTrigger} 已激活");
            }
            else
            {
                // 如果没有动画控制器或参数不存在，直接显示
                interactPrompt.SetActive(true);
                Debug.Log("没有动画控制器，直接显示提示");
            }
        }
        else
        {
            Debug.LogError("interactPrompt 为空！请检查UI设置");
        }
    }
    
    public void HideInteractPrompt()
    {
        Debug.Log("HideInteractPrompt 被调用");
        
        if (interactPrompt != null)
        {
            // 检查动画控制器是否存在并且有对应的参数
            if (promptAnimator != null && HasAnimatorParameter(promptAnimator, hideTrigger))
            {
                promptAnimator.SetTrigger(hideTrigger);
            }
            else
            {
                // 如果没有动画控制器或参数不存在，直接隐藏
                interactPrompt.SetActive(false);
                Debug.Log("没有动画控制器，直接隐藏提示");
            }
        }
        else
        {
            Debug.LogError("interactPrompt 为空！请检查UI设置");
        }
    }
    
    // 检查Animator是否有指定的参数
    private bool HasAnimatorParameter(Animator animator, string parameterName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;
            
        AnimatorControllerParameter[] parameters = animator.parameters;
        foreach (var param in parameters)
        {
            if (param.name == parameterName)
                return true;
        }
        return false;
    }
}