using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ParkourController : MonoBehaviour
{
    public BarrierChecker barrierChecker;
    bool playerInAction;
    public Animator anim;
    public List<NewParkourSystem> newParkourActions;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump") && !playerInAction)
        {
            var hitData = barrierChecker.CheckBarrier();

            if (hitData.hitFound)
            {
                anim.SetFloat("Speed", 0.8f);
                Debug.Log("Object Founded" + hitData.hitInfo.transform.name);

                foreach (var action in newParkourActions)
                {
                    if (action.CheckBarrierHeight(hitData, transform))
                    //if the barrier height within the threshold, it becomes true
                    {
                        StartCoroutine(PerformParkourAction(action));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator PerformParkourAction(NewParkourSystem action)
    {
        playerInAction = true;
        PlayerController.instance.SetControl(false);

        anim.CrossFade(action.animationName, 0.2f);
        yield return null; //wait for a frame

        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        float timeCounter = 0f;

        while (timeCounter <= anim.GetCurrentAnimatorStateInfo(0).length)
        {
            timeCounter += Time.deltaTime;

            //Make player to look toward the barrier/obstacle
            if (action.lookAtBarrier)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.RequiredRotation, PlayerController.instance.rotSpeed * Time.deltaTime);
            }
            if (action.allowTargetMatching && !anim.IsInTransition(0))
            {
                CompareTarget(action);
            }
            if(anim.IsInTransition(0) && timeCounter > 0.5f)
            {
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(action.parkourActionDelay);

        PlayerController.instance.SetControl(true);
        playerInAction = false;
    }

    void CompareTarget(NewParkourSystem action)
    {
        anim.MatchTarget(action.comparePosition, transform.rotation, action.compareBodyPart, new MatchTargetWeightMask(new Vector3(0, 1, 1), 0), action.compareStartTime, action.compareEndTime);
        //comparing position of body part in Y and Z axis withing certain startTime and endTIme of animation (based on the body part)
    }

}