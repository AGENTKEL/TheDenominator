using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwap : MonoBehaviour
{
    public GameObject playerToSpawn;
    public GameObject playerToDespawn;
    public GameObject crosshair;
    public GameObject gunPickupSound;

    public GameObject rifleDespawn;
    public GameObject pistolDespawn;
    public GameObject shotgunDespawn;
    public GameObject grenadelauncherDespawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerNoWeapon")
        {
            PlayerSpawner();
        }
    }

    private void PlayerSpawner()
    {
        playerToSpawn.SetActive(true);
        playerToDespawn.SetActive(false);
        crosshair.SetActive(true);
        rifleDespawn.SetActive(false);
        pistolDespawn.SetActive(false);
        shotgunDespawn.SetActive(false);
        grenadelauncherDespawn.SetActive(false);
        Instantiate(gunPickupSound);
    }
}
