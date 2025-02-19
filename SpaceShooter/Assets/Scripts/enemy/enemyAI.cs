using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    //allows the nav mesh to be referenced
    public NavMeshAgent agent;


    //reference for the script that will allow the enemy to find the player
    public Transform player;


    //referece for the scrip that will allow the enemy to differenciate the ground/player
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    //finds a point for the enemy to walk to
    public Vector3 walkPoint;
    //checks if a new walk point has been set
    bool walkPointSet;
    //sets a max distance the enemy can travel between
    public float walkPointRange;


    //Attacking
    // allows for a cooldown between attacks
    public float attackCD;
    //chekcs if the enemy has attacked
    bool alreadyAttacked;
    //references the projectile game object
    public GameObject projectile;

    //States
    //sets the max distance the enemy can see for detection purposes
    //sets a max distance the enemy can attack from
    public float sightRange, attackRange;
    //checks if the player is seen and if it is in attack range 
    public bool playerInSightRange, playerInAttackRange;

    //;
    WaveSpawner spawner;

    public void Awake()
    {
        //sets the target for the enemy to find in this case the player
        player = GameObject.Find("Player").transform;
        //references the Nav Mesh so that a walkable area is set
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //simple logic to determine the current state the Enemy Should be in
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if ( playerInSightRange && !playerInAttackRange) ChasePlayer();
        if ( playerInSightRange &&  playerInAttackRange) AttackPlayer();


    }


        public void Patroling()
    {
        //if there isn't a walk point use the function to create one
        if (!walkPointSet) SearchWalkPoint();

        // if the walk point is set use it to patrol
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        //calculates distance to walk point
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //if walkpoint reached find a new walk point
        if ( distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void SearchWalkPoint()
    {
        //calculates a random value as the walk point for the enemy to travel to dependent on the max range 
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        //same as randomZ but gives a second number that we can apply to another axis
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //asigns the random values we generated to give x and z values, y is not used as we don't need the enemy to use that axis 
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //uses a raycast to check if the point generated is on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    public void ChasePlayer()
    {
        //sets the target as the player
        //Vector3 distanceToPlayer = transform.position - walkPoint;
        agent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        //stops the enemy from moving once attacking
        agent.SetDestination(transform.position);

        //makes the enemy face the player
        transform.LookAt(player);

        //function that will allow the enemy to attack repeatedly dependent on attackCD
        if (!alreadyAttacked)
        {
            //everytime the already attacked function is set to true spawn a projectile and give it force 
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 14f, ForceMode.Impulse);
            rb.AddForce(transform.up * 4f, ForceMode.Impulse);

            Destroy(rb, 3f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCD);
        }

    }

    public void UpdateEnemyAI()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Simple logic to determine the current state the Enemy Should be in
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    public void ResetAttack()
    {
        //set to false so that the attack player function can repeat
        alreadyAttacked = false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


    public void killEnemy()
    {
        if (spawner != null)
        {
            spawner.currentMonster.Remove(this.gameObject);
            spawner = null;
        }
        Destroy(gameObject);
    }


    public void SetSpawner(WaveSpawner _spawner)
    {
        spawner = _spawner;
    }

    public void BossAttackPlayer()
    {
        //stops the enemy from moving once attacking
        agent.SetDestination(transform.position);

        //makes the enemy face the player
        transform.LookAt(player);

        //function that will allow the enemy to attack repeatedly dependent on attackCD
        if (!alreadyAttacked)
        {
            Transform gunTip = transform.Find("MediumMechStrikerArmRight");

            if (gunTip != null)
            {
             //everytime the already attacked function is set to true spawn a projectile and give it force 
             Rigidbody rb = Instantiate(projectile, gunTip.position, gunTip.rotation).GetComponent<Rigidbody>();

             rb.AddForce(transform.forward * 1f, ForceMode.Impulse);

             rb.AddForce(transform.up * 1f, ForceMode.Impulse);

            }
            else
            {
                Debug.LogError("GunTip not found on the character. Make sure to name the part correctly.");
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCD);
        }

    }
}
