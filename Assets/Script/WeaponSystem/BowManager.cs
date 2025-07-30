using UnityEngine;

public class BowManager : MonoBehaviour, PlayerController.IWeaponHandler
{
    public GameObject arrowPrefab;
    public GameObject bow;
    public Transform firePoint;
    public Animator anim;

    public float shootForce = 25f;
    public float minChargeTime = 0.2f;

    private bool isAiming = false;
    private float holdStartTime;
    private float originalSpeed;

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

        isAiming = true;
        holdStartTime = Time.time;
        anim.SetBool("IsAiming", true); // ²¥·ÅÃé×¼¶¯»­
    }

    void ReleaseArrow()
    {
        PlayerController.instance.movementSpeed = originalSpeed;

        float heldTime = Time.time - holdStartTime;
        isAiming = false;

        if (heldTime >= minChargeTime)
        {
            anim.SetTrigger("Shoot");
            ShootArrow();
            Debug.Log("Shoot!!");
        }
        else
        {
            Debug.Log("Charge too short, no arrow released.");
            anim.SetBool("IsAiming", false);
        }
    }

    void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * shootForce;
        }
        anim.SetBool("IsAiming", false);
    }

    public void QuitWeapon()
    {
        isAiming = false;
        anim.SetBool("IsAiming", false);
        anim.SetBool("BowAttackActive", false);
        bow.SetActive(false);
    }
}
