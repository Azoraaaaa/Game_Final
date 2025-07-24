using UnityEngine;

public class CameraController1 : MonoBehaviour
{
    public Transform target;

    public float gap = 4f;
    float rotX;
    float rotY;

    public float minVerAngle = -45f;
    public float maxVerAngle = 45f;

    public Vector2 framingBalance;
    public float rotSpeed = 2f;

    public bool invertX, invertY;
    float invertXValue, invertYValue;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        invertXValue = (invertX) ? -1 : 1; //invert rotation of the camera, true(-1) or false(1) value
        invertYValue = (invertX) ? -1 : 1;

        Debug.Log("CameraController is updating...");


        rotX += Input.GetAxis("Mouse Y") * invertYValue * rotSpeed;
        rotX = Mathf.Clamp(rotX, minVerAngle, maxVerAngle); //limiting rotation angle of rotX (moving mouse up n down)
        rotY += Input.GetAxis("Mouse X") * invertXValue * rotSpeed;



        var targetRotation = Quaternion.Euler(rotX, rotY, 0);

        var focusPos = target.position + new Vector3(framingBalance.x, framingBalance.y);//focus to player

        transform.position = focusPos - targetRotation * new Vector3(0, 0, gap);
        transform.rotation = targetRotation;
    }

    public Quaternion flatRotation => Quaternion.Euler(0, rotY, 0); //x and z will not rotate
}
