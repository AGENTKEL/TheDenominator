using UnityEngine;
using UnityEngine.AI;

public class EnemyMonsterAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public int maxHealth;
    public int currentHealth;
    Ragdoll ragdoll;
    public bool isDead;

    public int enemyPrice = 500;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    Animator animator;
    SoundManager deathSound;
    PlayerMoney playerMoney;

    private ZM_Spawner waveSpawner;

    private void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("PlayerZM").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        deathSound = GetComponent<SoundManager>();
        playerMoney = GameObject.FindGameObjectWithTag("PlayerMoney").GetComponent<PlayerMoney>();
    }

    private void Start()
    {
        waveSpawner = GetComponentInParent<ZM_Spawner>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
            agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

            transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here


            animator.SetInteger("AttackIndex", Random.Range(0, 2));
            animator.SetTrigger("Attack");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;

        EnemyMonsterAi enemyMovement = GetComponent<EnemyMonsterAi>();
        NavMeshAgent enemyMovement2 = GetComponent<NavMeshAgent>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
            enemyMovement2.enabled = false;
        }

        deathSound.PlaySound();
        ragdoll.ActivateRagdoll();
        Invoke("Delay", 5f);
        playerMoney.AddMoney(enemyPrice);

        waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;

    }

    private void Delay()
    {
        Destroy(gameObject);
    }

}
