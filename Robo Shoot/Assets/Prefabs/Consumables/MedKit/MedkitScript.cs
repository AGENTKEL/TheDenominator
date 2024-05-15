using UnityEngine;

public class MedkitScript : MonoBehaviour
{
    public GameObject medkit; // assign the medkit object in the inspector
    public int healAmount = 10; // how much health to restore
    public AudioSource medKitUsed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check if the collider belongs to the player
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>(); // get the health system component of the player
            health.Heal(healAmount); // restore health to the player
            Instantiate(medKitUsed, transform.position, Quaternion.identity);
            Destroy(medkit); // destroy the medkit object
        }
    }
}
