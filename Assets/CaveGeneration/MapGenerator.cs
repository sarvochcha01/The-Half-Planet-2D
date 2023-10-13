using Cinemachine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform wallHolder;
    public Transform enemySpawnerHolder;
    public Transform collectableSpawnerHolder;

    public Transform player;
    public bool playerSpawned;

    public AstarPath path;
    public GameObject wallTile;

    public GameObject enemySpawner;
    public GameObject collectableSpawner;

    public int tileSize;

    [Min(15)]
    public int width;
    [Min(15)]
    public int height;

    public int smoothingIterations;

    public int exitSize;
    public bool randomizeSize;
    public int exitStartPosition;
    public bool randomizePosition;
    public int exitDepth;
    public bool randomizeDepth;

    public enum ExitPosition
    {
        leftSideExit,
        rightSideExit,
        topExit,
        bottomExit,
    }

    public ExitPosition exitPosition;
    public bool randomizeExitPosition;

    public int seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int fillPercent;

    int[,] map;

    public GameObject[,] tiles;

    void Start()
    {
        playerSpawned = false;
        GenerateGrid();
    }

    void GenerateGrid()
    {
        map = new int[width, height];
        tiles = new GameObject[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothingIterations; i++)
        {
            map = SmoothMap();
        }

        AddExit();
        GenerateMap();
        AddColliders();

        path = FindObjectOfType<AstarPath>();
        path.Scan();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = (DateTime.Now.TimeOfDay.Hours + DateTime.Now.TimeOfDay.Minutes) * DateTime.Now.TimeOfDay.Seconds * DateTime.Now.TimeOfDay.Milliseconds;
        }

        System.Random random = new System.Random(seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x <= 1 || x == width - 2 || y <= 1 || y == height - 2)
                {
                    map[x, y] = 1;
                }

                else map[x, y] = random.Next(0, 100) < fillPercent ? 1 : 0;
            }
        }
    }

    int[,] SmoothMap()
    {
        int[,] tempMap = (int[,])map.Clone();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4) tempMap[x, y] = 1;
                else if (neighbourWallTiles < 4) tempMap[x, y] = 0;

            }
        }

        return tempMap;
    }

    public int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;

        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else wallCount++;
            }
        }

        return wallCount;
    }

    void AddExit()
    {
        System.Random random = new System.Random();
        if (randomizeSize)
        {
            exitSize = random.Next(5, 10);
        }
        if (randomizePosition)
        {
            exitStartPosition = random.Next(0, width - exitSize);
        }
        if (randomizeDepth)
        {
            exitDepth = random.Next(1, 10);
        }
        if (randomizeExitPosition)
        {
            exitPosition = (ExitPosition)random.Next(0, 4);
        }

        switch (exitPosition)
        {
            case ExitPosition.leftSideExit:
                ExitLeft();
                break;
            case ExitPosition.rightSideExit:
                ExitRight();
                break;
            case ExitPosition.topExit:
                ExitTop();
                break;
            case ExitPosition.bottomExit:
                ExitBottom();
                break;
            default:
                break;
        }
    }

    void ExitLeft()
    {
        for (int x = 0; x < exitDepth; x++)
        {
            for (int y = exitStartPosition; y < exitStartPosition + exitSize; y++)
            {
                if (y >= exitStartPosition && y < exitStartPosition + exitSize)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    void ExitRight()
    {
        for (int x = width - exitDepth; x < width; x++)
        {
            for (int y = exitStartPosition; y < exitStartPosition + exitSize; y++)
            {
                if (y >= exitStartPosition && y < exitStartPosition + exitSize)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    void ExitTop()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = height - 1; y > height - 1 - exitDepth; y--)
            {
                if (x >= exitStartPosition && x < exitStartPosition + exitSize)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    void ExitBottom()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < exitDepth; y++)
            {
                if (x >= exitStartPosition && x < exitStartPosition + exitSize)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    void GenerateMap()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(-width / 2 + x + tileSize/2, -height / 2 + y + tileSize/2, 0) * tileSize*2;
                    if (map[x, y] == 1)
                    {
                        var instantiatedTile = Instantiate(wallTile, pos, Quaternion.identity);
                        tiles[x, y] = instantiatedTile;
                        instantiatedTile.transform.parent = wallHolder;
                    }
                    else
                    {
                        System.Random rand = new System.Random();
                        int random = rand.Next(0, 100);
                        if (random < 1)
                        {
                            var spawner = Instantiate(enemySpawner, pos, Quaternion.identity);
                            spawner.transform.parent = enemySpawnerHolder;
                        }
                        else if (random < 2)
                        {
                            var spawner = Instantiate(collectableSpawner, pos, Quaternion.identity);
                            spawner.transform.parent = collectableSpawnerHolder;
                        }
                        else if (random < 3 && !playerSpawned)
                        {
                            playerSpawned = true;
                            player.position = pos;
                        }
                    }
                }
            }
        }
    }

    void AddColliders()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x,y] == 1)
                {
                    bool shouldAddCollider = GetSurroundingWallCount(x, y) == 8 ? false : true;

                    if (shouldAddCollider)
                    {
                        GameObject curTile = tiles[x, y];
                        curTile.AddComponent<BoxCollider2D>();
                        curTile.GetComponent<BoxCollider2D>().size = new Vector2(tileSize, tileSize);
                    }
                }
            }
        }
    }

    void SpawnPlayer()
    {
        System.Random rand = new System.Random();
        int randomX =  rand.Next(0, width);
        int randomY = rand.Next(0, height);

        if (tiles[randomX, randomY] != null)
        {
            return;
        }

        Instantiate(player, tiles[randomX, randomY].transform.position, quaternion.identity);
        playerSpawned = true;
    }
}
