using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    [HideInInspector] public Vector3 dir;
    public AudioSource explosionSound;

    //stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //Lifetime
    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;
    [HideInInspector] public WeaponManager weapon;

    

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //When to explode:
        if (collisions > maxCollisions) Explode();

        //Count down lifetime
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0) Explode();
    }

    private void Explode()
    {
        //Instantiate explosion
        if (explosion != null)
        {
            Instantiate(explosionSound, transform.position, Quaternion.identity);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        //Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            //Get component of enemy and call Take damage

            EnemyAi enemy = enemies[i].GetComponentInParent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }

            EnemyMonsterAi enemyMonster = enemies[i].GetComponentInParent<EnemyMonsterAi>();
            if (enemyMonster != null)
            {
                enemyMonster.TakeDamage(explosionDamage);
            }

            EnemyNumberAi enemyNumber = enemies[i].GetComponentInParent<EnemyNumberAi>();
            if (enemyNumber != null)
            {
                enemyNumber.TakeDamage(explosionDamage);
            }



            if (enemies[i].GetComponent<Rigidbody>()) 
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        //Add a little delay, just to make sure everything works fine
        Invoke("Delay", 0.001f);
    }

    private void Delay()
    {
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Count up collisions
        collisions++;

        //Explode if bullet hits the enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch)
        {
            Explode();
            
        }
        else if (collision.collider.CompareTag("Untagged") && explodeOnTouch)
        {
            Explode();
            
        }

        else if (collision.collider.CompareTag("GlassDoor") && explodeOnTouch)
        {
            Explode();

        }

        else if (collision.collider.CompareTag("Map") && explodeOnTouch)
        {
            Explode();

        }

    }

    private void Setup()
    {
        //Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //Set gravity 
        rb.useGravity = useGravity;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
