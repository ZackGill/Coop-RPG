using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Globalization;

public class GenerateDungeon : MonoBehaviour {
    // Use this for initialization
    public Transform floor;
    public Transform wall;
    public Transform player;
    public Transform boss;
    private GameObject[] enemy;
    public GameObject wanderStalk;
    public GameObject patrolCharge;
    int xRooms = 4, yRooms = 4, zoneSize = 12, enemyCount;
    //xRooms and yRooms must be min. 4
	public bool[,] isFloor = new bool[0,0];
    int seed = (int)System.DateTime.Now.Ticks;
    int BUFFER = 2;

    void Start() {
        enemy = new GameObject[2];
        enemy[0] = wanderStalk;
        enemy[1] = patrolCharge;
        enemyCount =  UnityEngine.Random.Range(1, Mathf.CeilToInt(xRooms * yRooms/2));
        UnityEngine.Random.InitState(seed);
        isFloor = new bool[zoneSize * xRooms + 2*BUFFER, zoneSize * yRooms + 2*BUFFER];
        int[,] centers = new int[xRooms * yRooms, 3];
        int roomcount = 0;
        int ULX, ULY, LRX, LRY;
        double odds = 115 - 10 * Math.Log(xRooms * yRooms, 2.71828);
        print("ODDS: " + odds);
        for (int J = 0; J < xRooms; J++)
        {
            for (int I = 0; I < yRooms; I++)
            {
                if (UnityEngine.Random.Range(0, 100) < odds) //The more rooms, the lower odds this specific room will be added.
                {
                    ULX = UnityEngine.Random.Range(0, zoneSize / 2);
                    ULY = UnityEngine.Random.Range(0, zoneSize / 2);
                    LRX = UnityEngine.Random.Range(ULX+4, zoneSize);
                    LRY = UnityEngine.Random.Range(ULY+4, zoneSize);

                    centers[roomcount, 0] = (ULX + zoneSize * J + LRX + zoneSize * J) / 2;
                    centers[roomcount, 1] = (ULY + zoneSize * I + LRY + zoneSize * I) / 2;
                    centers[roomcount, 2] = roomcount;
                    roomcount++;

                    print("(" + (ULX + zoneSize * J) + "," + (ULY + zoneSize * I) + ") - " + (LRX - ULX) + "x" + (LRY - ULY) +
                            " with center (" + centers[roomcount - 1, 0] + "," + centers[roomcount - 1, 1] + ")");
                    for (int x = ULX; x < LRX; x++)
                    {
                        for (int y = ULY; y < LRY; y++)
                        {
                            isFloor[x + zoneSize * J + BUFFER, y + zoneSize * I + BUFFER] = true;
                        }
                    }
                }
            }
        }
        bool allConnected = false;
        int loops = 0;
        while (!allConnected)
        {
            loops++;
            for (int i = loops%yRooms; i < roomcount; i+=yRooms)
            {
                int j = UnityEngine.Random.Range(0, roomcount - 1);
                {
                    if (centers[i, 2] != centers[j, 2]) // If rooms arent connected, connect
                    {
                        int r1x = centers[i, 0], r1y = centers[i, 1], r2x = centers[j, 0], r2y = centers[j, 1];
                        //Four cases: R1 above/below R2, and R1 left/right of R2
                        print("Making hallway (" + r1x + "," + r1y + ") to (" + r2x + "," + r2y + ")");
                        int x = r1x;
                        if(x < r2x)
                        {
                            for(x = r1x; x < r2x; x++)
                            {
                                isFloor[x, r1y] = true;
                            }
                            x--;
                        }else
                        {
                            for(x = r1x; x >= r2x; x--)
                            {
                                isFloor[x, r1y] = true;
                            }
                            x++;
                        }
                        int y = r1y;
                        if(y < r2y)
                        {
                            for (y = r1y; y < r2y; y++) {
                                isFloor[x, y] = true;
                            }
                        }
                        else
                        {
                            for(y = r1y; y >= r2y; y--)
                            {
                                isFloor[x, y] = true;
                            }
                        }
                        if(centers[i,2] < centers[j, 2])
                        {
                            int room2Value = centers[j, 2];
                            for (int k = 0; k < roomcount; k++)
                            {
                                if (centers[k, 2] == room2Value) centers[k, 2] = centers[i, 2];
                            }
                        }else
                        {
                            int room2Value = centers[i, 2];
                            for (int k = 0; k < roomcount; k++)
                            {
                                if (centers[k, 2] == room2Value) centers[k, 2] = centers[j, 2];
                            }
                        }
                    }
                }
            }
            allConnected = true;
            for(int z = 0; z < roomcount; z++)
            {
                if (centers[z, 2] != 0) allConnected = false;
            }
        }
        print("Took " + loops + " iterations.");
        
        for (int i = 0; i < yRooms * zoneSize+2; i++)
        {
            for (int j = 0; j < xRooms * zoneSize+2; j++)
            {
                if (isFloor[j, i])
                {
                    Instantiate(floor, new Vector3(j, i, 0f), Quaternion.identity);
                }
                else
                {
                    Instantiate(wall, new Vector3(j, i, 0f), Quaternion.identity);
                }
            }
        }

        int pX, pY;
        do
        {
            pX = UnityEngine.Random.Range(0, zoneSize * 2+BUFFER);
            pY = UnityEngine.Random.Range(0, zoneSize * 2+BUFFER);
        } while (!isFloor[pX, pY]);

        var pc = Instantiate(player, new Vector3(pX, pY, -.5f), Quaternion.identity);
        pc.name = "PlayerChar";

        do
        {
            pX = UnityEngine.Random.Range(0, zoneSize * xRooms);
            pY = UnityEngine.Random.Range(0, zoneSize * yRooms);
        } while (!isFloor[pX, pY] || (pX < (zoneSize * 3) && pY < (zoneSize * 3)));
        //Boss must be outside the bottom-left 3 rooms.
        Instantiate(boss, new Vector3(pX, pY, -.5f), Quaternion.identity);

        for (int e = 0; e < enemyCount; e++)
        {
            do
            {
                pX = UnityEngine.Random.Range(0, zoneSize * xRooms);
                pY = UnityEngine.Random.Range(0, zoneSize * yRooms);
            } while (!isFloor[pX, pY]);
            Instantiate(enemy[UnityEngine.Random.Range(0,enemy.Length)], new Vector3(pX, pY, -.5f), Quaternion.identity);
        }
    }
	// Update is called once per frame
	void Update ()
    {
        
	}
}
