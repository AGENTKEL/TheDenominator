using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyNumberAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public int maxHealth;
    public int currentHealth;
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
    PlayerMoney playerMoney;
    Image hitMarkerDamage;
    Image hitMarkerDeath;

    private ZM_Spawner waveSpawner;

    public Renderer enemyRenderer; // Reference to the enemy's renderer component
    public Material originalMaterial; // The original material of the enemy

    private void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.Find("PlayerZM").transform;
        hitMarkerDamage = GameObject.FindGameObjectWithTag("HitMarkerDamage").GetComponent<Image>();
        hitMarkerDeath = GameObject.FindGameObjectWithTag("HitMarkerDeath").GetComponent<Image>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerMoney = GameObject.FindGameObjectWithTag("PlayerMoney").GetComponent<PlayerMoney>();
    }

    private void Start()
    {
        waveSpawner = GetComponentInParent<ZM_Spawner>();
        originalMaterial = enemyRenderer.material;
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
            StartCoroutine(HitMarkerDamageOn());
            if (currentHealth <= 0)
            {
                Die();
                StartCoroutine(HitMarkerDeathOn());
            }
        }
    }

    private void Die()
    {
        isDead = true;

        

        EnemyNumberAi enemyMovement = GetComponent<EnemyNumberAi>();
        NavMeshAgent enemyMovement2 = GetComponent<NavMeshAgent>();
        Collider enemyCollider = GetComponent<Collider>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
            enemyMovement2.enabled = false;
            enemyCollider.enabled = false;
        }

        animator.SetBool("IsDead", true);
        Invoke("Delay", 5f);
        playerMoney.AddMoney(enemyPrice);

        waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;


    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    public IEnumerator HitMarkerDamageOn()
    {
        hitMarkerDamage.enabled = true;
        yield return new WaitForSeconds(0.05f);
        hitMarkerDamage.enabled = false;
    }

    public IEnumerator HitMarkerDeathOn()
    {
        hitMarkerDeath.enabled = true;
        yield return new WaitForSeconds(0.2f);
        hitMarkerDeath.enabled = false;
    }

}
