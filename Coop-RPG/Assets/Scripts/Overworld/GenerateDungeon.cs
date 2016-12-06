using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Globalization;
using AssemblyCSharp;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class GenerateDungeon : NetworkBehaviour {
    // Use this for initialization
    public GameObject floor;
    public GameObject wall;
    public GameObject player;
    public GameObject boss;
    private GameObject[] enemy;
    public GameObject wanderStalk;
    public GameObject patrolCharge;
    [SyncVar]
    public GameObject spawnLocal;
    [SyncVar]
    public GameObject spawnLocal1;
    [SyncVar]
    public GameObject spawnLocal2;
    [SyncVar]
    public GameObject spawnLocal3;
    int xRooms = 4, yRooms = 4, zoneSize = 12, enemyCount;
    //xRooms and yRooms must be min. 4
	public bool[,] isFloor = new bool[0,0];
    int[,] connectID = new int[0, 0], centers = new int[0,0];
    int seed = (int)System.DateTime.Now.Ticks;
    int BUFFER = 2;
    private int SMALL = 1, MED = 2, LARGE = 3; //Dungeon size

    [SyncVar]
    int pX, pY;


    public GameObject network;
 
    void Awake()
    {

        Start();

    }

    void Start()
    {

        network = GameObject.Find("LobbyManager");
        UnityEngine.Random.InitState(seed);

        if (!isServer)
            return;
        if (Dungeon.Size == 0)
        { //Dungeon size not set, pick one at random
            Dungeon.Size = UnityEngine.Random.Range(SMALL, LARGE);
        }
        if (Dungeon.Size == SMALL)
        {
            xRooms = UnityEngine.Random.Range(4, 6);
            yRooms = UnityEngine.Random.Range(4, 6);
            zoneSize = UnityEngine.Random.Range(8, 16);
        }
        else if (Dungeon.Size == MED)
        {
            xRooms = UnityEngine.Random.Range(5, 9);
            yRooms = UnityEngine.Random.Range(5, 9);
            zoneSize = UnityEngine.Random.Range(10, 20);
        }
        else if (Dungeon.Size == LARGE)
        {
            xRooms = UnityEngine.Random.Range(7, 12);
            yRooms = UnityEngine.Random.Range(7, 12);
            zoneSize = UnityEngine.Random.Range(12, 24);
        }
        enemy = new GameObject[2];
        enemy[0] = wanderStalk;
        enemy[1] = patrolCharge;
        enemyCount = UnityEngine.Random.Range(1, Mathf.CeilToInt(xRooms * yRooms / 2));
        isFloor = new bool[zoneSize * xRooms + 2 * BUFFER, zoneSize * yRooms + 2 * BUFFER];
        connectID = new int[zoneSize * xRooms + 2 * BUFFER, zoneSize * yRooms + 2 * BUFFER];
        centers = new int[xRooms * yRooms, 3];
        int roomcount = 0;
        int ULX, ULY, LRX, LRY;
        double odds = 115 - 10 * Math.Log(xRooms * yRooms, 2.71828);
        //print("ODDS: " + odds);
        for (int J = 0; J < xRooms; J++)
        {
            for (int I = 0; I < yRooms; I++)
            {
                if (UnityEngine.Random.Range(0, 100) < odds) //The more rooms, the lower odds this specific room will be added.
                {
                    ULX = UnityEngine.Random.Range(0, zoneSize / 2);
                    ULY = UnityEngine.Random.Range(0, zoneSize / 2);
                    LRX = UnityEngine.Random.Range(ULX + 4, zoneSize);
                    LRY = UnityEngine.Random.Range(ULY + 4, zoneSize);

                    centers[roomcount, 0] = (ULX + zoneSize * J + LRX + zoneSize * J) / 2;
                    centers[roomcount, 1] = (ULY + zoneSize * I + LRY + zoneSize * I) / 2;
                    centers[roomcount, 2] = roomcount;

                    for (int x = ULX; x < LRX; x++)
                    {
                        for (int y = ULY; y < LRY; y++)
                        {
                            isFloor[x + zoneSize * J + BUFFER, y + zoneSize * I + BUFFER] = true;
                            connectID[x + zoneSize * J + BUFFER, y + zoneSize * I + BUFFER] = roomcount;
                            //When a room is made, assign all its floor tiles a 'connect ID' for making halls and the like.
                        }
                    }
                    roomcount++;
                }
            }
        }
        bool allConnected = false;
        int loops = 0;
        while (!allConnected)
        {
            loops++;
            for (int i = loops % yRooms; i < roomcount; i += yRooms)
            {
                int j = UnityEngine.Random.Range(0, roomcount - 1);
                {
                    if (connectID[centers[i, 0], centers[i, 1]] != connectID[centers[j, 0], centers[j, 1]]) // If rooms arent connected, connect
                    {
                        int r1x = centers[i, 0], r1y = centers[i, 1], r2x = centers[j, 0], r2y = centers[j, 1];
                        //Four cases: R1 above/below R2, and R1 left/right of R2
                         print("Making hallway (" + r1x + "," + r1y + ") to (" + r2x + "," + r2y + ")");
                        int startID = connectID[r1x, r1y];
                        int x = r1x;
                        if (x < r2x)
                        {
                            for (x = r1x; x < r2x; x++)
                            {
                                if (isFloor[x, r1y] && connectID[x,r1y] != startID)
                                {
                                    connectRooms(startID, connectID[x, r1y], roomcount);
                                    break;
                                }
                                else
                                {
                                    isFloor[x, r1y] = true;
                                    connectID[x, r1y] = startID;
                                }
                            }
                            x--;
                        }
                        else
                        {
                            for (x = r1x; x >= r2x; x--)
                            {
                                if (isFloor[x, r1y] && connectID[x, r1y] != startID)
                                {
                                    connectRooms(startID, connectID[x, r1y], roomcount);
                                    break;
                                }
                                else
                                {
                                    isFloor[x, r1y] = true;
                                    connectID[x, r1y] = startID;
                                }
                            }
                            x++;
                        }
                        int y = r1y;
                        if (y < r2y)
                        {
                            for (y = r1y; y < r2y; y++)
                            {
                                if (isFloor[x, y] && connectID[x, y] != startID)
                                {
                                    connectRooms(startID, connectID[x, y], roomcount);
                                    break;
                                }
                                else
                                {
                                    isFloor[x, y] = true;
                                    connectID[x, y] = startID;
                                }
                            }
                        }
                        else
                        {
                            for (y = r1y; y >= r2y; y--)
                            {
                                if (isFloor[x, y] && connectID[x, y] != startID)
                                {
                                    connectRooms(startID, connectID[x, y], roomcount);
                                    break;
                                }
                                else {
                                    isFloor[x, y] = true;
                                    connectID[x, y] = startID;
                                }
                            }
                        }
                        

                    }
                }
            }
            allConnected = true;
            for (int x = 0; x < connectID.GetLength(0); x++)
            {
                for (int y = 0; y < connectID.GetLength(1); y++)
                {
                    if (connectID[x, y] > 0) allConnected = false;
                }
            }
        }
        spawnEntities();
    }

    public void connectRooms(int a, int b, int roomcount)
    {
        print("Connecting " + a + " to " + b);
        if (a < b)
        {
            for(int x = 0; x < connectID.GetLength(0); x++)
            {
                for(int y = 0; y < connectID.GetLength(1); y++)
                {
                    if (connectID[x, y] == b) connectID[x, y] = a;
                }
            }
        }
        else
        {
            for (int x = 0; x < connectID.GetLength(0); x++)
            {
                for (int y = 0; y < connectID.GetLength(1); y++)
                {
                    if (connectID[x, y] == a) connectID[x, y] = b;
                }
            }
        }
    }

    public void spawnEntities() { 
        GameObject temp;
        for (int i = 0; i < yRooms * zoneSize + 2; i++)
        {
            for (int j = 0; j < xRooms * zoneSize + 2; j++)
            {
                if (isFloor[j, i])
                {
                    temp = (GameObject)Instantiate(floor, new Vector3(j, i, 0f), Quaternion.identity);
                    NetworkServer.Spawn(temp);
                }
                else
                {
                    temp = (GameObject)Instantiate(wall, new Vector3(j, i, 0f), Quaternion.identity);
                    NetworkServer.Spawn(temp);
                }
            }
        }

        do
        {
            pX = UnityEngine.Random.Range(0, zoneSize * 2 + BUFFER);
            pY = UnityEngine.Random.Range(0, zoneSize * 2 + BUFFER);
        } while (!isFloor[pX, pY] || !isFloor[pX + 1, pY] || !isFloor[pX - 1, pY] || !isFloor[pX, pY + 1]);

        spawnLocal.transform.position = new Vector3(pX, pY, -.5f);
        spawnLocal1.transform.position = new Vector3(pX - 1, pY, -.5f);
        spawnLocal2.transform.position = new Vector3(pX + 1, pY, -.5f);
        spawnLocal3.transform.position = new Vector3(pX, pY + 1, -.5f);


        do
        {
            pX = UnityEngine.Random.Range(0, zoneSize * xRooms);
            pY = UnityEngine.Random.Range(0, zoneSize * yRooms);
        } while (!isFloor[pX, pY] || (pX < (zoneSize * 3) && pY < (zoneSize * 3)));
        //Boss must be outside the bottom-left 3 rooms.
        GameObject monster = (GameObject)Instantiate(boss, new Vector3(pX, pY, -.5f), Quaternion.identity);
        NetworkServer.Spawn(monster);

        for (int e = 0; e < enemyCount; e++)
        {
            do
            {
                pX = UnityEngine.Random.Range(0, zoneSize * xRooms);
                pY = UnityEngine.Random.Range(0, zoneSize * yRooms);
            } while (!isFloor[pX, pY]);
            GameObject tempE = (GameObject)Instantiate(enemy[UnityEngine.Random.Range(0, enemy.Length)], new Vector3(pX, pY, -.5f), Quaternion.identity);
            tempE.transform.SetParent(transform);
            NetworkServer.Spawn(tempE);
            //dump.monsters.Add(temp.GetComponentInChildren<Monster>());
        }

        // Spawning players

        //dungeonOut.text = dungeon;
        System.DateTime end = System.DateTime.Now;
        // print("Time: " + (start - end));
    }
	// Update is called once per frame
	void Update ()
    {
        
	}
}
