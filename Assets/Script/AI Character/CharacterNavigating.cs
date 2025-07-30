using UnityEngine;

public class CharacterNavigating : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached; 


    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    public void Walk()
    {
        if(transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;

            destinationDirection.y = 0f; //preventing character to float in the air
            float destinationDistance = destinationDirection.magnitude;

            if(destinationDistance >= stopSpeed)
            {
                //turning

                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

                //moving AI
                transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);

                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 1f, Vector3.down, out hit, 5f))
                {
                    transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                }
            }
            else
            {
                destinationReached = true;
            }

        }
    }

    public void LocateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }
}
