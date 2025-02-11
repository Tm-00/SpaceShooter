using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEngine.InputManagerEntry;

public class Boss_Run : StateMachineBehaviour
{
    enemyAI eai; // Reference to the enemyAI script
    Transform player;
    Rigidbody rb;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        player = GameObject.Find("Player").transform;

        // Find and assign the enemyAI script
        eai = animator.GetComponent<enemyAI>();


        eai.agent = eai.GetComponent<NavMeshAgent>();
        
        
        // gets the rigidbody component from the "boss" and assigns it to rb variable
        rb = animator.GetComponent<Rigidbody>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Call the Update method from the enemyAI script
        if (eai != null)
        {
            //check for sight and attack range
            eai.playerInSightRange = Physics.CheckSphere(eai.transform.position, eai.sightRange, eai.whatIsPlayer);
            eai.playerInAttackRange = Physics.CheckSphere(eai.transform.position, eai.attackRange, eai.whatIsPlayer);

            //simple logic to determine the current state the Enemy Should be in
            if (!eai.playerInSightRange && !eai.playerInAttackRange) eai.Patroling();
            if (eai.playerInSightRange && !eai.playerInAttackRange) eai.ChasePlayer();
            if (eai.playerInSightRange && eai.playerInAttackRange) 
            {
                animator.SetTrigger("AttackR");
            } 

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("AttackR");
    }

}
