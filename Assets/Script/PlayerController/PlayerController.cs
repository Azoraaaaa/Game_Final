using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float movementSpeed = 3f;
    public Camera cam;

    Quaternion requiredRotation;
    public float rotSpeed = 450f;

    Animator anim;
    CharacterController CC;

    public bool canMove;

    [Header("SurfaceCheck")]
    public float surfaceCheckRadius = 0.1f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    public bool onSurface;

    [Header("Falling Gravity")]
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;

    [Header("Weapon Switch")]
    public Weapon activeWeapon;
    public List<Weapon> allWeapons = new List<Weapon>();
    public int currentWeapon;
    public bool isSwitching = false;


    [Header("Jump / Dash / Roll Settings")]
    public float jumpForce = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.5f;
    public float rollSpeed = 7f;
    public float rollDuration = 0.5f;

    private bool isJumping = false;
    private bool isDashing = false;
    private bool isRolling = false;

    [Header("技能使用")]
    [SerializeField] private float dashSkillCost = 30f;
    [SerializeField] private float healSkillCost = 40f;
    [SerializeField] private float shieldSkillCost = 50f;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        CC = GetComponent<CharacterController>();
        canMove = true; // 默认允许移动
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SurfaceCheck();
        //Debug.Log("Player on Surface" + onSurface);

        DoAttack();


        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && onSurface && !isJumping && !isDashing && !isRolling)
        {
            StartCoroutine(Jump());
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && onSurface && !isDashing && !isJumping && !isRolling)
        {
            if (PlayerHealthSystem.instance.HasEnoughSkillPoints(dashSkillCost))
            {
                if (PlayerHealthSystem.instance.ConsumeSkillPoints(dashSkillCost))
                {
                    StartCoroutine(Dash());
                }
            }
            else
            { 
                if (UIManager.instance != null)
                {
                    UIManager.instance.ShowNotification("Insufficient skill points!", 2f);
                }
            }
        }

        // Roll
        if (Input.GetKeyDown(KeyCode.LeftControl) && onSurface && !isRolling && !isJumping && !isDashing)
        {
            StartCoroutine(Roll());
        }
    }

    void PlayerMovement()
    {
        // 检查是否允许移动
        if (!canMove)
        {
            // 如果不能移动，设置动画速度为0并返回
            anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 MovementDirection = (camForward * vertical) + (camRight * horizontal);
        float movementAmount = Mathf.Clamp01(MovementDirection.magnitude);

        // Gravity Handling
        if (onSurface && !isJumping)
        {
            fallingSpeed = 0f;
        }
        else
        {
            fallingSpeed += (Physics.gravity.y * Time.deltaTime) * 2;
        }

        Vector3 finalMove = MovementDirection.normalized * movementSpeed;
        finalMove.y = fallingSpeed;

        if (CC.enabled && !isDashing && !isRolling)
        {
            CC.Move(finalMove * Time.deltaTime);
        }

        // Rotate to movement direction (not just forward)
        if (movementAmount > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(MovementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotSpeed * Time.deltaTime);
        }

        anim.SetFloat("Speed", movementAmount, 0.2f, Time.deltaTime);
    }

    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

    public void SetControl(bool hasControl)
    {
        CC.enabled = hasControl;
    }
    public void SwitchWeaponTo(int weaponIndex)
    {
        if (weaponIndex == currentWeapon && activeWeapon != null)
        {
            var handler = activeWeapon.GetComponent<IWeaponHandler>();
            if (handler != null)
            {
                handler.QuitWeapon();
            }

            activeWeapon.gameObject.SetActive(false);
            activeWeapon = null;
            currentWeapon = -1;

            WeaponManager.instance.changeWeaponUI(-1);
            return;
        }

        if (weaponIndex >= 0 && weaponIndex < allWeapons.Count)
        {
            if (!allWeapons[weaponIndex].isUnlocked)
            {
                Debug.Log($"Weapon {weaponIndex} is not unlocked and cannot be switched to.");
                return;
            }

            if (activeWeapon != null)
            {
                var handler = activeWeapon.GetComponent<IWeaponHandler>();
                if (handler != null)
                {
                    handler.QuitWeapon();
                }

                activeWeapon.gameObject.SetActive(false);
            }

            currentWeapon = weaponIndex;
            activeWeapon = allWeapons[currentWeapon];
            activeWeapon.gameObject.SetActive(true);

            WeaponManager.instance.changeWeaponUI(currentWeapon);


            //UIController.instance.AmmoText.text = "Ammo: " + activeGun.currentAmmo;
        }
    }
    public void DoAttack()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("Key1");
            SwitchWeaponTo(0);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("Key2");
            SwitchWeaponTo(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("Key3");
            SwitchWeaponTo(2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("Key4");
            SwitchWeaponTo(3);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("Key5");
            SwitchWeaponTo(4);
        }
    }
    public interface IWeaponHandler
    {
        void QuitWeapon();
    }


    IEnumerator Jump()
    {
        isJumping = true;
        anim.SetBool("isJumping", true);

        fallingSpeed = jumpForce;

        yield return new WaitForSeconds(0.8f); // 根据你的跳跃动画长度调整

        anim.SetBool("isJumping", false);
        isJumping = false;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        anim.SetBool("isDashing", true);

        float timer = 0f;
        //Vector3 dashDirection = transform.forward;
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 dashDirection = (cam.transform.TransformDirection(inputDir)).normalized;
        if (dashDirection == Vector3.zero) dashDirection = transform.forward; 

        while (timer < dashDuration)
        {
            CC.Move(dashDirection * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("isDashing", false);
        isDashing = false;
    }



    IEnumerator Roll()
    {
        isRolling = true;
        anim.SetBool("isRolling", true);

        Vector3 rollDirection = transform.forward;

        float timer = 0f;
        while (timer < rollDuration)
        {
            CC.Move(rollDirection * rollSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("isRolling", false);
        isRolling = false;
    }

}