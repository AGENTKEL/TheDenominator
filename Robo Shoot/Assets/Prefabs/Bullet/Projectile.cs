using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10; // amount of damage the projectile will do to the player
    public float speed = 10f; // speed at which the projectile moves
    public float lifespan = 2f; // how long the projectile will exist before being destroyed

    private Rigidbody rb; // rigidbody component of the projectile

    public AudioSource shootingSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Instantiate(shootingSound, transform.position, Quaternion.identity);
        Destroy(gameObject, lifespan); // destroy the projectile after its lifespan has expired
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime); // move the projectile forward
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // if the projectile hits the player, do damage
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("PlayerNoWeapon")) // if the projectile hits the player, do damage
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Map")) 
        {
            
            Destroy(gameObject);
        }
    }
}
