using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeletoncontrolor : MonoBehaviour
{
    public Transform[] tragetPoint;
    public int currentPoint;

    public NavMeshAgent agent;

    public Animator Animator;

    public float waitAtpoint = 2f;
    public float waitCounter;

    public enum AIState
    {
        isDead, isSeekTargerPoint, isSeekPlayer ,isAttack
    }
    public AIState state;

   

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtpoint; 
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPalyer = Vector3.Distance(transform.position, playercontrolor.instance.transform.position);

        if (!playercontrolor.instance.isdead)
        {
            if ((distanceToPalyer >= 1 && distanceToPalyer <= 8f))
         {
             state = AIState.isSeekPlayer;
         }
         else if ((distanceToPalyer > 8f))
         {
            state = AIState.isSeekTargerPoint;
         }
         else
         {
            state = AIState.isAttack;
         } 
        }
        else
        {
            state = AIState.isSeekTargerPoint;
            Animator.SetBool("Attack", false);
            Animator.SetBool("Run", true);
        }
       

       switch (state)
        {
            case AIState.isDead:
                break;
            case AIState.isSeekPlayer:
                agent.SetDestination(playercontrolor.instance.transform.position);
                Animator.SetBool("Run", true);
                Animator.SetBool("Attack", false);
                break;
            case AIState.isSeekTargerPoint:
                agent.SetDestination(tragetPoint[currentPoint].position);
                agent.stoppingDistance = 0;
                if (agent.remainingDistance <= .2f)
                {
                    if (waitCounter > 0)
                    {
                        waitCounter -= Time.deltaTime;
                        Animator.SetBool("Run", false);
                    }
                    else
                    {
                        currentPoint++;
                        waitCounter = waitAtpoint;
                        Animator.SetBool("Run", true);
                    }

                    if (currentPoint >= tragetPoint.Length)
                    {
                        currentPoint = 0;
                    }
                    agent.SetDestination(tragetPoint[currentPoint].position);
                }
                break;
            case AIState.isAttack:
                RotateTowardPlayer();
                agent.stoppingDistance = 1;
                Animator.SetBool("Attack", true);
                Animator.SetBool("Run", false);
                break;
        }

        void RotateTowardPlayer()
        {
            Vector3 direction = (playercontrolor.instance.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
       
        
    }
}
