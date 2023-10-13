using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private bool hasSpawnedEnemies = false;

    public void SpawnEnemies(int count)
    {
        if (!hasSpawnedEnemies)
        {
            hasSpawnedEnemies = true;
            StartCoroutine(StartEnemySpawn(count));
        }
    }

    IEnumerator StartEnemySpawn(int count)
    {
        int i = 0;
        while (i < count)
        {
            yield return new WaitForSeconds(2);
            Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
            i++;
        }

    }

    public void SetSpawnEnemyBool(bool value)
    {
        hasSpawnedEnemies = value;
    }
}
