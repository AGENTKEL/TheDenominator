using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryEvent : MonoBehaviour
{
    public GameObject monster;
    public Transform spawnPos;
    public GameObject scarySound;

    private void Awake()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(monster, spawnPos.position, spawnPos.rotation);
            Invoke("DestroyEvent", 0.8f);
        }

        if (other.CompareTag("PlayerNoWeapon"))
        {
            Instantiate(monster, spawnPos.position, spawnPos.rotation);
            Instantiate(scarySound);
            Invoke("DestroyEvent", 0.1f);
        }
    }

    private void DestroyEvent()
    {
        Destroy(gameObject);
    }
}
