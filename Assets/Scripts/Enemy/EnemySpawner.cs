using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class EnemySpawner : MonoBehaviour
{

    public float DistanceToPlayerToSpawn;
    public Transform Player;
    public int MaxEnemiesSpawnedAtATime;
    public float SpawnCooldown;
    public GameObject EnemyToSpawn;
    private List<GameObject> enemies = new List<GameObject>();
    private float spawnTimer = 0;

    public void Update()
    {
        if (enemies.Count < MaxEnemiesSpawnedAtATime)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0 && Mathf.Abs((Player.transform.position - transform.position).magnitude) < DistanceToPlayerToSpawn)
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        spawnTimer = SpawnCooldown;
        if (enemies.Count >= MaxEnemiesSpawnedAtATime)
            return;

        GameObject enemy = Instantiate(EnemyToSpawn, transform.position, Quaternion.identity);
        enemies.Add(enemy);
        enemy.GetComponent<AttackableResource>().SetSpawnSource(gameObject);
    }

    public void RemoveFromArray(GameObject obj)
    {
        if (enemies.Contains(obj))
        {
            enemies.Remove(obj);
        }
    }
}
