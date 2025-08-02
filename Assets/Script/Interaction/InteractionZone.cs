using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    [Header("交互设置")]
    public GameObject hiddenObject; // 要显示的隐藏物体
    public KeyCode interactKey = KeyCode.E; // 交互按键

    private bool playerInZone = false; // 玩家是否在区域中

    private void Start()
    {
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false); // 开始时隐藏物体
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered interaction zone: " + gameObject.name);
            // 确保触发器是触发器类型
            playerInZone = true;
            UIManager.instance.ShowNotification("Press <b>E</b> to Interact", 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(interactKey))
        {
            if (hiddenObject != null)
            {
                hiddenObject.SetActive(true); // 显示物体
            }
        }
    }
}
