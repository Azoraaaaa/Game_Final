
using UnityEngine;

public class LocationTrigger1 : MonoBehaviour
{
    public static LocationTrigger1 instance;
    private void Awake()
    {
        instance = this;
    }

    public string locationID; // ��MapLocation�е�ID��Ӧ
    private bool playerIsInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = true;
            GameManager.Instance.isNearTeleporter = true;

            // ���ص��Ƿ��ѱ�����
            if (!GameManager.Instance.discoveredLocations.Contains(locationID))
            {
                UIManager.instance.ShowPersistentNotification("Press <b>E</b> Activate the Teleportation Point");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInZone = false;
            GameManager.Instance.isNearTeleporter = false;
            UIManager.instance.HideNotification();
        }
    }

    void Update()
    {
        // �������������ڲ��Ұ�����E��
        if (playerIsInZone && Input.GetKeyDown(KeyCode.E))
        {
            // ���ص��Ƿ��ǵ�һ�α�����
            if (GameManager.Instance.discoveredLocations.Add(locationID))
            {
                Debug.Log("Location discovered: " + locationID);
                UIManager.instance.ShowNotification("Teleportation Point Activated", 2f); // ��ʾһ�����ݵĳɹ���ʾ

                ChaseTrigger.instance.isTriggered = true;
            }
        }
    }
}