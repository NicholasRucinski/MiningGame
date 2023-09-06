using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class MapGenerator : NetworkBehaviour
{
    [SerializeField]
    private float renderDistance = 20;
    public int width = 16;
    public int height = 16;
    public int xStart = 8;
    public int yStart = 0;
    public GameObject wallPrefab;
    public GameObject diamondPrefab;
    public GameObject ironPrefab;
    public GameObject EmeraldPrefab;
    public GameObject AmethystPrefab;

    public string seed;
    NetworkVariable<float> networkSeed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int randomFillPercent;
 
    NetworkList<int> map;
    //int[] map = new int[width * height];
    //public NetworkList<Block> gameMap;
    GameObject[,] gameMap;
    public GameObject player;

    private void Awake()
    {
        gameMap = new GameObject[width, height];
        map = new NetworkList<int>();
        networkSeed = new NetworkVariable<float>();

    }
    private void Start()
    {
        GenerateMap();
        player = GameObject.FindGameObjectWithTag("Player");
        

    }

    private void Update()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (gameMap[x,y] != null && Vector3.Distance(player.transform.position, gameMap[x,y].transform.position) > renderDistance)
                {
                    gameMap[x, y].SetActive(false);
                }
                else if(gameMap[x, y] != null && Vector3.Distance(player.transform.position, gameMap[x, y].transform.position) <= renderDistance)
                {
                    gameMap[x, y].SetActive(true);
                }
            }
        }
        
    }

    void GenerateMap()
    {
        RandomFillMap();
        
        for (int i = 0; i < 3; ++i)
        {
            SmoothMap();
        }
        oreFill(2, 0.5f);
        oreFill(3,3f);
        oreFill(4, 0.5f);
        oreFill(5, 0.5f);
        fillGameMap();
        spawnPlayer(35);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        fillGameMap();
    }

    private void spawnPlayer(int spawnChance)
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (map[x * width + y] == 0  && map[x * width + (y-1)] == 1)
                {
                    GameObject go = Instantiate(player, new Vector3(x-xStart, y-yStart), Quaternion.identity);
                    go.GetComponent<NetworkObject>().Spawn();
                    return;
                }

            }
        }
    }
    private void fillGameMap()
    {
        Debug.Log("Filling Game Map");
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    //Generate stone
                    if (map[x * width + y] == 1)
                    {
                        gameMap[x, y] = GameObject.Instantiate(wallPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);


                    }//Generate diamonds
                    else if (map[x * width + y] == 2)
                    {
                        gameMap[x, y] = GameObject.Instantiate(diamondPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }//generate iron
                    else if (map[x * width + y] == 3)
                    {
                        gameMap[x, y] = GameObject.Instantiate(ironPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }
                    else if (map[x * width + y] == 4)
                    {
                        gameMap[x, y] = GameObject.Instantiate(EmeraldPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }
                    else if (map[x * width + y] == 5)
                    {
                        gameMap[x, y] = GameObject.Instantiate(AmethystPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }
                }
            }
        Debug.Log("Finished Filling Game Map");

    }

    void RandomFillMap()
    {
        if (useRandomSeed && IsHost)
        {
            networkSeed.Value = Time.time;
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for(int x = 0; x < width; ++x){
            for (int y = 0; y < height; ++y)
            {
                if (x == 0 || x == width-1 || y == 0 || y == height-1)
                {
                    map.Add(1);
                }
                else
                {
                    map.Add((pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0);
    }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                int neighborWallTiles = GetSurroundingWallCount(x,y);

                if (neighborWallTiles > 4)
                {
                    map[x * width + y] = 1;
                } else if (neighborWallTiles < 4)
                {
                    map[x * width + y] = 0;
                }
            }
        }
    }

    void oreFill(int oreNumber, float oreFillPercent)
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (map[x * width + y] == 1 && pseudoRandom.Next(0, 100) < oreFillPercent)
                {
                    map[x * width + y] = oreNumber;
                }
                
            }
        }
    }
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
        {
            for (int neighborY = gridY - 1; neighborY <= gridY +1; neighborY++)
            {
                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (neighborX != gridX || neighborY != gridY)
                    {
                        wallCount += map[neighborX * width + neighborY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    public bool getSouth(int x, int y)
    {
        return y != 0 && map[x * width + (y - 1)] >= 1;
    }
    public bool getNorth(int x, int y)
    {
        return y != height-1 && map[x * width + (y + 1)] >= 1;
    }
    public bool getWest(int x, int y)
    {
        return x != 0 && map[(x-1) * width + y] >= 1;
    }
    public bool getEast(int x, int y)
    {
        return x != width-1 && map[(x + 1) * width + y] >= 1;
    }
    public void updateBlock(int x, int y, int destroyed)
    {
        map[x * width + y] = destroyed;
    }
    public void updateMap(int xPos, int yPos)
    {
        for (int x = xPos-2; x <= xPos+2; ++x)
        {
            for (int y = yPos-2; y <= yPos+2; ++y)
            {
                
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (gameMap[x, y] != null)
                    {
                        gameMap[x, y].GetComponent<Block>().updateNeighbors();
                    }
                }
                
            }
        }
    }
    public void updateGameObjects(int xPos, int yPos)
    {
        for (int x = xPos - 2; x <= xPos + 2; ++x)
        {
            for (int y = yPos - 2; y <= yPos + 2; ++y)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (gameMap[x, y] != null)
                    {

                    }
                    //Generate stone
                    if (map[x * width + y] == 1)
                    {
                        gameMap[x, y] = GameObject.Instantiate(wallPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);


                    }//Generate diamonds
                    else if (map[x * width + y] == 2)
                    {
                        gameMap[x, y] = GameObject.Instantiate(diamondPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }//generate iron
                    else if (map[x * width + y] == 3)
                    {
                        gameMap[x, y] = GameObject.Instantiate(ironPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }
                    else if (map[x * width + y] == 4)
                    {
                        gameMap[x, y] = GameObject.Instantiate(EmeraldPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }
                    else if (map[x * width + y] == 5)
                    {
                        gameMap[x, y] = GameObject.Instantiate(AmethystPrefab, new Vector2(x - xStart, y - yStart), Quaternion.identity, transform);
                        gameMap[x, y].GetComponent<Block>().setPos(x, y);
                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        map.Dispose();
    }
}
