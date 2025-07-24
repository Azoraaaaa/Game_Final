using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public Transform player;
    public float height = 70f;
    public float rotationSpeed = 10f;

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        // Position
        Vector3 newPosition = player.position;
        newPosition.y = height;
        transform.position = newPosition;

        // Rotation
        Quaternion newRotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }
} 