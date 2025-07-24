using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3f;
    public CameraController cam;

    Quaternion requiredRotation;
    public float rotSpeed = 450f;

    Animator anim;
    CharacterController CC;

    [Header("SurfaceCheck")]
    public float surfaceCheckRadius = 0.1f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    bool onSurface;

    [Header("Falling Gravity")]
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        CC = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SurfaceCHeck();
        Debug.Log("Player on Surface" + onSurface);
    }

    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        //to detect movement based on movement keys, Clamp01 convert value from 0 to 1 from Movement Tree Animation

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;

        var MovementDirection = cam.flatRotation * movementInput;
        //movement based on camera rotation (has flat rotation) and movement input

        //check if the player is on the surface to apply gravity
        if (onSurface)
        {
            fallingSpeed = 0f; //reset falling speed
        }
        else
        {
            //we are falling
            fallingSpeed += Physics.gravity.y * Time.deltaTime / 2; //devide by 2 will make player lighter
        }

        moveDir = new Vector3(MovementDirection.x, fallingSpeed, MovementDirection.z);

        if (movementAmount > 0)
        {
            CC.Move(moveDir * movementSpeed * Time.deltaTime);

            requiredRotation = Quaternion.LookRotation(MovementDirection); //rotate based on input, capture the second rotation state 
        }

        moveDir = MovementDirection; //when we are on the ground already

        transform.rotation = Quaternion.RotateTowards(transform.rotation, requiredRotation, rotSpeed * Time.deltaTime);
        //will make smooth rotation from one state to another based on speed

        anim.SetFloat("Speed", movementAmount, 0.2f, Time.deltaTime);
    }

    void SurfaceCHeck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

}

