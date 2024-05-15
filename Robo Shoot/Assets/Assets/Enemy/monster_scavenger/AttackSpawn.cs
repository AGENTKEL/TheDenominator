using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpawn : MonoBehaviour
{
    public Transform monsterProjectileSpawnPos;
    public GameObject monsterProjectile;

    public void SpawnMonsterProjectile()
    {
        Instantiate (monsterProjectile, monsterProjectileSpawnPos);
    }
}
