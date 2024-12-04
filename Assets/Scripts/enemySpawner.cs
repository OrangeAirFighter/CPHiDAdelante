using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int spawnTimeMax = 4;
    public int startSpawning = 6;

    private void Start()
    {
        Invoke("spawning", startSpawning);
    }

    public void spawning()
    {
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(-45,45), 0);
        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, randomRotation);
        Invoke("spawning", Random.Range(2,spawnTimeMax));
    }
}
