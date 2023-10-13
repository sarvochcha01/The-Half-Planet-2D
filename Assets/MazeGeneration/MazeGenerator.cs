using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] float nodeSize;

    [SerializeField] private int exitRange;

    [SerializeField] private Vector3 playerPos;

    [SerializeField] private int maximumEnemySpawners;

    [SerializeField] private GameObject waveCountText;
    [SerializeField] private GameObject waveCountUpdate;
    [SerializeField] private bool shouldSpawn;

    public Transform enemySpawnerHolder;
    public Transform collectableSpawnerHolder;

    public Transform player;
    public bool playerSpawned;

    [SerializeField] private int enemySpawnerCount;

    //public AstarPath path;

    public GameObject enemySpawner;
    public GameObject collectableSpawner;

    public GameObject Agun;
    public GameObject Sgun;
    public GameObject Shield;

    public GameObject exit;
    public bool exitSpawned;

    public GameManager gameManager;

    public bool isMazeGenerated;

    private void Start()
    {
        //GenerateMazeInstant(mazeSize);
        //StartCoroutine(GenerateMaze(mazeSize));
        //StartCoroutine(GenerateMaze());
        shouldSpawn = true;
        waveCountText.SetActive(true);
        waveCountUpdate.SetActive(true);
        isMazeGenerated = false;
    }

    IEnumerator GenerateMaze()
    {
        yield return new WaitForSeconds(1f);
        mazeSize.x = gameManager.difficulty * 12;
        mazeSize.y = gameManager.difficulty * 12;
        exitRange = gameManager.difficulty * 100;

        switch(gameManager.enemySpawnCount)
        {
            case 0:     
                enemySpawnerCount = 0;  
                waveCountText.SetActive(false);
                waveCountUpdate.SetActive(false);
                shouldSpawn = false;
                break;
            case 1:     enemySpawnerCount = 6;      break;
            case 2:     enemySpawnerCount = 9;      break;
            case 3:     enemySpawnerCount = 12;     break;
            case 4:     enemySpawnerCount = 15;     break;
        }

        GenerateMazeInstant(mazeSize);
        SpawnEnemySpawner(mazeSize);
    }

    void GenerateMazeInstant(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        // Create nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3(-size.x / 2 + x + .5f, -size.y / 2 + y + .5f, 0) * nodeSize;
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);

                int random = Random.Range(0, 100);
                if (random < 50 && !playerSpawned)
                {
                    playerPos = nodePos;
                    player.position = nodePos;

                    if (shouldSpawn)
                    {
                        Instantiate(Agun, nodePos + new Vector3(0, 5, 0), Quaternion.identity);
                        Instantiate(Sgun, nodePos + new Vector3(-5, 0, 0), Quaternion.identity);
                        Instantiate(Shield, nodePos + new Vector3(5, 0, 0), Quaternion.identity);
                    }

                    playerSpawned = true;
                }
                else if (random < 10 && shouldSpawn)
                {
                    Instantiate(collectableSpawner, nodePos, Quaternion.identity, collectableSpawnerHolder);
                }

                if (playerSpawned && !exitSpawned && (nodePos - playerPos).magnitude > exitRange && random >= 97)
                {
                    Instantiate(exit, nodePos, Quaternion.identity);
                    exitSpawned = true;
                }
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        // Choose starting node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
            }
            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }

        //path = FindObjectOfType<AstarPath>();
        //path.Scan();
    }

    private void Update()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (gameManager != null && !isMazeGenerated)
        {
            StartCoroutine(GenerateMaze());
            isMazeGenerated = true;
        }
    }

    private void SpawnEnemySpawner(Vector2Int size)
    {
        for (int i = 0; i < enemySpawnerCount; i++)
        {
            Vector3 dir = GetRandomDir();

            Vector3 pos = dir * size.x * nodeSize;
            Instantiate(enemySpawner, pos, Quaternion.identity, enemySpawnerHolder);
        }
    }

    public Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0).normalized;
    }

    public bool ShouldSpawn()
    {
        return shouldSpawn;
    }
}