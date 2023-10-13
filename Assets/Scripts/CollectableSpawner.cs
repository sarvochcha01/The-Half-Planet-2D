using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> collectableList = new List<GameObject>();
    //[SerializeField] private Transform player;

    [SerializeField] private bool collectableSpawned = false;

    //[SerializeField] private GameObject spawnedItem;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //collectableSpawned = false;

        //SpawnCollectable();

    }


    public void SpawnCollectable()
    {
        if (!collectableSpawned)
        {
            int random = Random.Range(0, collectableList.Count);
            Instantiate(collectableList[random], transform.position, Quaternion.identity, transform);
            collectableSpawned = true;
        }
    }

    public void SetSpawnBool(bool value)
    {
        collectableSpawned = value;
    }
}
