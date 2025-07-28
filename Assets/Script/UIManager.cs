
using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Panels")]
    public GameObject shopPanel;
    public GameObject bagPanel;

    [Header("Notifications")]
    public TextMeshProUGUI notificationText;
    private Coroutine notificationCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }

    // --- Panel Management ---
    public void OpenShopPanel()
    {
        Debug.Log("UIManager: Attempting to open shop panel...");
        if(shopPanel != null) 
        {
            shopPanel.SetActive(true);
            Debug.Log("UIManager: Shop panel activated successfully!");
            
            // 添加更详细的调试信息
            Canvas canvas = shopPanel.GetComponent<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"UIManager: Shop Canvas - Sort Order: {canvas.sortingOrder}, Render Mode: {canvas.renderMode}");
            }
            
            // 检查子对象
            int activeChildren = 0;
            int totalChildren = 0;
            foreach (Transform child in shopPanel.transform)
            {
                totalChildren++;
                if (child.gameObject.activeInHierarchy)
                {
                    activeChildren++;
                }
            }
            Debug.Log($"UIManager: Shop panel has {activeChildren}/{totalChildren} active children");
        }
        else
        {
            Debug.LogError("UIManager: Shop panel reference is null! Please check the UIManager configuration.");
        }
    }

    public void CloseShopPanel()
    {
        Debug.Log("UIManager: Closing shop panel...");
        if(shopPanel != null) shopPanel.SetActive(false);
        // Optional: when closing shop, end the dialogue
        if(DialogueManager.Instance != null) DialogueManager.Instance.EndDialogue();
    }

    public void OpenBagPanel()
    {
        Debug.Log("UIManager: Attempting to open bag panel...");
        if(bagPanel != null) 
        {
            bagPanel.SetActive(true);
            Debug.Log("UIManager: Bag panel activated successfully!");
            
            // 添加更详细的调试信息
            Canvas canvas = bagPanel.GetComponent<Canvas>();
            if (canvas != null)
            {
                Debug.Log($"UIManager: Bag Canvas - Sort Order: {canvas.sortingOrder}, Render Mode: {canvas.renderMode}");
            }
            
            // 检查子对象
            int activeChildren = 0;
            int totalChildren = 0;
            foreach (Transform child in bagPanel.transform)
            {
                totalChildren++;
                if (child.gameObject.activeInHierarchy)
                {
                    activeChildren++;
                }
            }
            Debug.Log($"UIManager: Bag panel has {activeChildren}/{totalChildren} active children");
        }
        else
        {
            Debug.LogError("UIManager: Bag panel reference is null! Please check the UIManager configuration.");
        }
    }

    public void CloseBagPanel()
    {
        Debug.Log("UIManager: Closing bag panel...");
        if(bagPanel != null) bagPanel.SetActive(false);
        // Optional: when closing bag, end the dialogue
        if(DialogueManager.Instance != null) DialogueManager.Instance.EndDialogue();
    }


    // --- Notifications ---

    // 显示一条持续存在的提示
    public void ShowPersistentNotification(string message)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
    }

    // 显示一条会在几秒后自动消失的提示
    public void ShowNotification(string message, float duration)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = StartCoroutine(ShowNotificationCoroutine(message, duration));
    }

    private IEnumerator ShowNotificationCoroutine(string message, float duration)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        notificationText.gameObject.SetActive(false);
    }

    // 隐藏提示
    public void HideNotification()
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationText.gameObject.SetActive(false);
    }
} 