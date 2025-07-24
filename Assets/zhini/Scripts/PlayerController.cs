using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 3f;
    public CameraController1 Cam;

    Quaternion RequiredRotation;
    public float RotSpeed = 450f;

    Animator Anim;
    CharacterController Cc;

    [Header("SurfaceCheck")]
    public float SurfaceCheckRadius = 0.1f;
    public Vector3 SurfaceCheckOffset;
    public LayerMask SurfaceLayer;
    bool ontheSurface;

    [Header("Falling Gravity")]
    [SerializeField] float FallingSpeed;
    [SerializeField] Vector3 MoveDir;

    void Start()
    {
        Anim = GetComponent<Animator>();
        Cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayersMovement();
        SurfacetoCheck();
        Debug.Log("Player on Surface" + ontheSurface);
    }

    void PlayersMovement()
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
            FallingSpeed = 0f; //reset falling speed
        }
        else
        {
            //we are falling
            FallingSpeed += Physics.gravity.y * Time.deltaTime / 2; //devide by 2 will make player lighter
        }

        MoveDir = new Vector3(MovementDirection.x, FallingSpeed, MovementDirection.z);

        if (movementAmount > 0)
        {
            Cc.Move(MoveDir * MovementSpeed * Time.deltaTime);

            RequiredRotation = Quaternion.LookRotation(MovementDirection); //rotate based on input, capture the second rotation state 
        }

        moveDir = MovementDirection; //when we are on the ground already

        transform.rotation = Quaternion.RotateTowards(transform.rotation, RequiredRotation, RotSpeed * Time.deltaTime);
        //will make smooth rotation from one state to another based on speed

        Anim.SetFloat("Speed", movementAmount, 0.2f, Time.deltaTime);
    }

    void SurfacetoCheck()
    {
        ontheSurface = Physics.CheckSphere(transform.TransformPoint(SurfaceCheckOffset), SurfaceCheckRadius, SurfaceLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(SurfaceCheckOffset), surfaceCheckRadius);
    }

}

