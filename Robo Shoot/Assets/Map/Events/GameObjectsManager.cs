using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsManager : MonoBehaviour
{
    public ZM_Spawner zmSpawner; // Reference to the ZM_Spawner
    public Transform[] spawnPointsToActivate;
    public Collider[] colliders;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            zmSpawner.AddSpawnPoints(spawnPointsToActivate);
            DeactivateAllColliders();
        }
    }

    public void DeactivateAllColliders()
    {
        foreach (Collider collider in colliders)
        {
            if (collider != null)
                collider.gameObject.SetActive(false);
        }
    }
}
