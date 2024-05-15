using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
    [HideInInspector] public WeaponManager weapon;
    [HideInInspector] public Vector3 dir;

    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a Health component
        EnemyAi enemyHealth = collision.gameObject.GetComponentInParent<EnemyAi>();
        if (enemyHealth != null)
        {
            // Deal damage to the enemy and apply kickback force if it dies
            if (enemyHealth.currentHealth <= 0 && enemyHealth.isDead == false)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(dir * weapon.enemyKickBackForce, ForceMode.Impulse);
                enemyHealth.isDead = true;
            }
        }

        // Destroy the bullet object regardless of whether it hit an enemy or not
        Destroy(this.gameObject);
    }
}
