using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float turnSmoothTime = 0.1f;
    public float speedSmoothTime = 0.1f;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    [Header("Animation")]
    public Animator animator;
    
    private CharacterController controller;
    private Camera mainCamera;
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float currentSpeed;
    private float verticalVelocity;
    private bool isGrounded;
    
    // Animation parameter hashes
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        
        // 如果没有设置动画器，尝试获取
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
            
        // 确保有地面检测点
        if (groundCheck == null)
            Debug.LogWarning("No ground check point assigned to ThirdPersonController!");
    }
    
    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateAnimations();
    }
    
    void HandleMovement()
    {
        // 获取输入
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;
        
        // 计算目标速度
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        targetSpeed *= input.magnitude;
        
        // 平滑过渡到目标速度
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        
        // 根据相机方向计算移动方向
        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * currentSpeed * Time.deltaTime);
        }
    }
    
    void HandleJump()
    {
        // 地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        
        // 跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (animator != null)
                animator.SetTrigger(jumpHash);
        }
        
        // 应用重力
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            // 更新动画参数
            animator.SetFloat(speedHash, currentSpeed);
            animator.SetBool(isGroundedHash, isGrounded);
        }
    }
} 