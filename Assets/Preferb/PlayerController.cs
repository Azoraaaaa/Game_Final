using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float movementSpeed = 3f;
    public CameraController cam;

    Quaternion requiredRotation;
    public float rotSpeed = 450f;

    Animator anim;
    CharacterController CC;

    [Header("SurfaceCheck")]
    public float surfaceCheckRadius = 0.1f;
    public Vector3 surfaceCheckOffest;
    public LayerMask surfaceLayer;
    bool onSurface;

    [Header("Falling Gravity")]
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        CC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SurfaceCheck();
        //Debug.Log("Player onSurface" + onSurface);
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        //to detect movement based on movement keys, Clamp01: convert value to be 0 to 1 for Movement Tree Animation

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;

        var MovementDirection = cam.flatRotation * movementInput;
        //movement based on camera rotation (has flat rotation) and movement input

        //check if player is on surface to apply gravity
        if(onSurface)
        {
            fallingSpeed = 0f;
        }
        else
        {
            //we are falling
            fallingSpeed += (Physics.gravity.y * Time.deltaTime)/2;
        }

        moveDir = new Vector3(MovementDirection.x, fallingSpeed, MovementDirection.z);

        if (CC.enabled) //only move when character controller is enabled
        {
            CC.Move(moveDir * movementSpeed * Time.deltaTime);
        }
        if (movementAmount > 0)
        {
            requiredRotation = Quaternion.LookRotation(MovementDirection);//rotate based on input, capture the second rotation state
        }

        moveDir = MovementDirection;//when we are on the ground, we can move in the direction of the camera

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requiredRotation, rotSpeed * Time.deltaTime);
        //will make smooth rotation from one state to another based on speed

        anim.SetFloat("Speed", movementAmount, 0.2f, Time.deltaTime);
    }

    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffest), surfaceCheckRadius, surfaceLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffest), surfaceCheckRadius);
    }

    public void SetControl(bool hasControl)
    {
        CC.enabled = hasControl;
    }
}