using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    public Vector3 rayOffset = new Vector3();
    public float rayLength = 10f, jumpPower = 10f;
    public LayerMask BarrierLayer;

    private Animator anim;
    private bool hardLanding, frontFlip;
    public PlayerController PlayerCon;
    private CharacterController CC;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hardLanding = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PerformRayCast();
    }

    private void PerformRayCast()
    {
        Vector3 rayDirection = -transform.up;

        //rotate the rayOffset vector based on player's rotation
        Vector3 rotatedOffset = Quaternion.Euler(transform.rotation.eulerAngles) * rayOffset;

        var rayOrigin = transform.position + rotatedOffset;
        RaycastHit hit;

        //perform the raycast
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength, BarrierLayer))
        {
            float distanceToHit = Vector3.Distance(transform.position, hit.point);
            if (distanceToHit > 1f && distanceToHit < 8f)
            {
                Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.black);
                //check Ledge
                CheckLedge(hit.point, distanceToHit);
            }

        }
    }

    void CheckLedge(Vector3 hitPoint, float distanceToHit)
    {
        var currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(distanceToHit);

        if (currentStateInfo.IsTag("FrontFlip"))
        {
            goto hallo;
        }

        if (!currentStateInfo.IsTag("MovementTree"))
        {
            return;
        }

    hallo:

        if (distanceToHit > 3f)
        {
            hardLanding = true;
        }

        if (!PlayerCon.onSurface || frontFlip)//in the air of falling
        {
            if (hardLanding)
            {
                //do hardlanding
                StartCoroutine(PerformHardLanding());
            }
            else
            {
                if (distanceToHit > 1f && distanceToHit < 3f)
                {
                    //do softlanding
                    StartCoroutine(PerformSoftLanding());
                }
            }
        }

        if (PlayerCon.onSurface)
        {
            if ((distanceToHit > 1f && distanceToHit < 10f) && Input.GetButtonDown("Jump"))
            {
                frontFlip = true;
            }
        }
    }

    IEnumerator PerformHardLanding()
    {
        yield return null;
        if (frontFlip)
        {
            anim.Play("FrontFlip");

            if (CC != null)
            {
                if (CC.enabled)
                {
                    CC.Move(Vector3.up * jumpPower * Time.deltaTime);//jumping
                }
            }

            yield return new WaitForSeconds(0.5f);
            hardLanding = true;
            frontFlip = false;
        }
        else
        {
            anim.CrossFade("HardLanding", 0.2f);//transition to hardLanding animation
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

            PlayerCon.SetControl(false);//disable the movement of the player
            yield return new WaitForSeconds(0.3f);//disable within this time

            PlayerCon.SetControl(true);//make him able to move again
            hardLanding = false;//switch off hardlanding
        }
    }

    IEnumerator PerformSoftLanding()
    {
        yield return null;
        if (frontFlip)
        {
            anim.Play("FrontFlip");

            if (CC != null)
            {
                if (CC.enabled)
                {
                    CC.Move(Vector3.up * jumpPower * Time.deltaTime);//jumping
                }
            }

            yield return new WaitForSeconds(0.5f);
            hardLanding = true;
            frontFlip = false;
        }
        else
        {
            anim.CrossFade("SoftLanding", 0.2f);//transition to hardLanding animation
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

            PlayerCon.SetControl(false);//disable the movement of the player
            yield return new WaitForSeconds(0.3f);//disable within this time

            PlayerCon.SetControl(true);//make him able to move again
            hardLanding = false;//switch off hardlanding
        }
    }
}