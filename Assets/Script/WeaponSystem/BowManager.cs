using UnityEngine;

public class BowManager : MonoBehaviour, PlayerController.IWeaponHandler
{
    public GameObject arrowPrefab;
    public GameObject bow;
    public Transform firePoint;
    public Animator anim;

    public float shootForce;
    public float minChargeTime;

    private bool isAiming = false;
    private float holdStartTime;
    private float originalSpeed;
    private GameObject currentArrow;

    private void OnEnable()
    {
        bow.SetActive(true);
        anim.SetBool("BowAttackActive", true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartAiming();
        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            ReleaseArrow();
        }
    }

    void StartAiming()
    {
        originalSpeed = PlayerController.instance.movementSpeed;
        PlayerController.instance.movementSpeed = PlayerController.instance.movementSpeed * 0.3f;

        currentArrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        currentArrow.transform.SetParent(firePoint);
        Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // ��������Ӱ�죬��ֹ��ǰ����
        }

        isAiming = true;
        holdStartTime = Time.time;
        anim.SetBool("IsAiming", true);
    }

    void ReleaseArrow()
    {
        PlayerController.instance.movementSpeed = originalSpeed;
        isAiming = false;

        float heldTime = Time.time - holdStartTime;

        if (heldTime >= minChargeTime && currentArrow != null)
        {
            anim.SetTrigger("Shoot");

            // �����
            currentArrow.transform.SetParent(null);
            Rigidbody rb = currentArrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = firePoint.forward * shootForce;
            }

            currentArrow = null; // �������
            Debug.Log("Shoot!!");
            anim.SetBool("IsAiming", false);
        }
        else
        {
            // ���̫�̣����ټ�
            if (currentArrow != null)
                Destroy(currentArrow);

            currentArrow = null;
            Debug.Log("Charge too short, no arrow released.");
            anim.SetBool("IsAiming", false);
        }
    }

    public void QuitWeapon()
    {
        isAiming = false;
        anim.SetBool("IsAiming", false);
        anim.SetBool("BowAttackActive", false);
        bow.SetActive(false);

        if (currentArrow != null)
        {
            Destroy(currentArrow); // ������ʱ���δ�����
            currentArrow = null;
        }
    }
}
