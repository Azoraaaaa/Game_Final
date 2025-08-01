using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    //private Rigidbody theRB;
    private bool chasing;
    public float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f;
    private Vector3 targetPoint, startPoint;
    private NavMeshAgent agent;

    public float keepChasingTime = 5f;
    private float chaseCounter;

    public Animator anim;

    private bool isDead = false;
    private bool isAttacking = false;

    [Header("Optional Patrol Settings")]
    public bool enablePatrol = false;
    public Transform patrolPointA;
    public Transform patrolPointB;
    private Transform currentPatrolTarget;
    public float patrolPauseTime = 2f;
    private float patrolPauseCounter;

    private void Awake()
    {
        //theRB = GetComponent<Rigidbody>();
        startPoint = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //startPoint = transform.position; //store enemy initial position as the game started
        if (enablePatrol && patrolPointA != null && patrolPointB != null)
        {
            currentPatrolTarget = patrolPointA;
            GoToPatrolPoint(currentPatrolTarget);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!chasing) //he is not chasing player
        {
            //anim.SetBool("isRunning", false);

            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
                anim.SetBool("isRunning", true);
            }

            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime; //count down

                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }


            if (enablePatrol && patrolPointA != null && patrolPointB != null && chaseCounter <= 0)
            {
                anim.SetBool("isRunning", true);

                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    patrolPauseCounter += Time.deltaTime;
                    if (patrolPauseCounter >= patrolPauseTime)
                    {
                        currentPatrolTarget = (currentPatrolTarget == patrolPointA) ? patrolPointB : patrolPointA;
                        GoToPatrolPoint(currentPatrolTarget);
                        patrolPauseCounter = 0f;
                    }
                }
            }
            else
            {
                anim.SetBool("isRunning", false);
            }


        }
        else //he is chasing player
        {
            //transform.LookAt(targetPoint);

            //theRB.linearVelocity = transform.forward * moveSpeed;

            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop) //more than 2m distance
            {
                agent.destination = targetPoint; //chasing the player

                anim.SetBool("isRunning", true);
            }
            else
            {
                //he will stop here 2m distance
                agent.destination = transform.position;
                anim.SetBool("isRunning", false);

                if (!isAttacking)
                {
                    isAttacking = true;
                    anim.SetTrigger("Attack");
                    StartCoroutine(AttackCooldown());
                }
            }


            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;

                agent.destination = transform.position; //stop him at his own position    
                                                        //agent.destination = startPoint;
                chaseCounter = keepChasingTime;

                anim.SetBool("isRunning", false);
            }
        }


    }

    void GoToPatrolPoint(Transform point)
    {
        agent.SetDestination(point.position);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f); 
        isAttacking = false;
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        chasing = false;

        agent.isStopped = true;
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isDead", true);

        Destroy(gameObject, 5f);
    }
}