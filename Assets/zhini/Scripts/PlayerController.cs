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

    [Header("SurfaceCheck")]
    public float surfaceCheckRadius = 0.1f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    public bool onSurface;

    [Header("Falling Gravity")]
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
        CC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SurfaceCheck();
        //Debug.Log("Player on Surface" + onSurface);
    }

    void PlayerMovement()
    {
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
        if (onSurface)
        {
            fallingSpeed = 0f;
        }
        else
        {
            fallingSpeed += (Physics.gravity.y * Time.deltaTime) * 2;
        }

        Vector3 finalMove = MovementDirection.normalized * movementSpeed;
        finalMove.y = fallingSpeed;

        if (CC.enabled)
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
}