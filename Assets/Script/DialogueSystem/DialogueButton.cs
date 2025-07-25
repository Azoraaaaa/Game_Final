using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DialogueButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TimelineManager.Instance != null && TimelineManager.Instance.IsWaitingForInput())
        {
            TimelineManager.Instance.ContinueTimeline();
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnClick);
        }
    }
} 