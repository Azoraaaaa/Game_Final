using System.Collections;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    public Vector3 rayOffset = new Vector3();
    public float rayLength = 10f, jumpPower = 5f;
    public LayerMask BarrierLayer;

    private Animator anim;
    private bool hardLanding, frontFlip;
    private PlayerController PlayerCon;
    private CharacterController CC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        hardLanding = false;
        CC = GetComponent<CharacterController>();
        PlayerCon = GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        PerformRayCast();
    }

    private void PerformRayCast()
    {
        Vector3 rayDirection = -transform.up; //down position relative to player position

        //rotate the rayOffset vector based on player's rotation
        Vector3 rotateOffset = Quaternion.Euler(transform.rotation.eulerAngles) * rayOffset;

        var rayOrigin = transform.position + rotateOffset;
        RaycastHit hit;

        //Perform the rayCast

        if(Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength, BarrierLayer))
        {
            float distanceToHit = Vector3.Distance(transform.position, hit.point);
            if(distanceToHit > 1f && distanceToHit < 8f)
            {
                Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.black);
                CheckLedge(hit.point, distanceToHit);
            }
        }
    }

    void CheckLedge(Vector3 hitPoint, float distanceToHit)
    {
        var currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(currentStateInfo.IsTag("FrontFlip"))
        {
            goto hello;
        }
        if(!currentStateInfo.IsTag("MovementTree"))
        {
            return;
        }

    hello:
        if(distanceToHit > 3f)
        {
            hardLanding = true;
        }

        if(!PlayerCon.onSurface || frontFlip) //in the air or falling oR frontFlip
        {
            if(hardLanding)
            {
                //do hardLanding
                StartCoroutine(PerformHardLanding());
            }
            else
            {
                if(distanceToHit > 1f && distanceToHit < 3f)
                {
                    //do softLanding
                    StartCoroutine(PerformSoftLanding());
                }
            }
        }

        if(PlayerCon.onSurface)
        {
            if((distanceToHit > 1f && distanceToHit < 10f) && Input.GetButtonDown("Jump"))
                    {
                        frontFlip = true;
                    }
        }
    }

    IEnumerator PerformHardLanding()
    {
        yield return null; //refresh animation

        if(frontFlip)
        {
            anim.Play("FrontFlip");

            if(CC != null)
            {
                if(CC.enabled)
                {
                    CC.Move(Vector3.up * jumpPower * Time.deltaTime); //jumping
                }
            }

            yield return new WaitForSeconds(0.5f);
            hardLanding = true;
            frontFlip = false;
        }
        else
        {
            anim.CrossFade("HardLanding", 0.2f); //transition to hard landing animation
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            //wait until animation above completed

            PlayerCon.SetControl(false); //disable the movement of the player
            yield return new WaitForSeconds(0.3f); //disable the movement within this time

            PlayerCon.SetControl(true); //we can move again here
            hardLanding = false;
        }
    }

    IEnumerator PerformSoftLanding()
    {
        yield return null; //refresh animation

        if (frontFlip)
        {
            anim.Play("FrontFlip");

            if (CC != null)
            {
                if (CC.enabled)
                {
                    CC.Move(Vector3.up * jumpPower * Time.deltaTime); //jumping
                }
            }

            yield return new WaitForSeconds(0.5f);
            hardLanding = false;
            frontFlip = false;
        }
        else
        {
            anim.CrossFade("SoftLanding", 0.2f); //transition to hard landing animation
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            //wait until animation above completed

            PlayerCon.SetControl(false); //disable the movement of the player
            yield return new WaitForSeconds(0.3f); //disable the movement within this time

            PlayerCon.SetControl(true); //we can move again here
            hardLanding = false;
        }
    }
}
