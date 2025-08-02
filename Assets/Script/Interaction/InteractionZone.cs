using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    [Header("��������")]
    public GameObject hiddenObject; // Ҫ��ʾ����������
    public KeyCode interactKey = KeyCode.E; // ��������

    private bool playerInZone = false; // ����Ƿ���������

    private void Start()
    {
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false); // ��ʼʱ��������
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered interaction zone: " + gameObject.name);
            // ȷ���������Ǵ���������
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
                hiddenObject.SetActive(true); // ��ʾ����
            }
        }
    }
}
