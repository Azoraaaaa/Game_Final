
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
        if(shopPanel != null) shopPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CloseShopPanel()
    {
        if(shopPanel != null) shopPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void CloseShopPanelAndEndDialogue()
    {
        CloseShopPanel();
        if(DialogueManager.Instance != null) DialogueManager.Instance.EndDialogue();
    }

    public void OpenBagPanel()
    {
        if(bagPanel != null) bagPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CloseBagPanel()
    {
        if(bagPanel != null) bagPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void CloseBagPanelAndEndDialogue()
    {
        CloseBagPanel();
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