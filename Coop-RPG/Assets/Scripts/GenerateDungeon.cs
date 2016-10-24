using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Globalization;
using AssemblyCSharp;
public class GenerateDungeon : MonoBehaviour {
    // Use this for initialization
    public LevelDump dump;
    public GameObject floor;
    public GameObject wall;
	int xRooms = 6, yRooms = 5, zoneSize = 15;
	bool[,] isFloor = new bool[0,0];
    int seed;

    void Start() {
        dump = GameObject.Find("LevelDump").GetComponent<LevelDump>();
        seed = dump.seed;
        System.DateTime start = System.DateTime.Now;
        UnityEngine.Random.InitState(seed);
        isFloor = new bool[zoneSize * xRooms + 2, zoneSize * yRooms + 2];
        int[,] centers = new int[xRooms * yRooms, 3];
        int roomcount = 0;
        int ULX, ULY, LRX, LRY;
        double odds = 115 - 10 * Math.Log(xRooms * yRooms, 2.71828);
        print("ODDS: " + odds);

        // Check if the level dump has data to use. If count == 0, then do random. Otherwise, set everything based on that.
        // Update Level Dump with separate
        // If the scripts of monster and all do not work by simply copying it from the scene, can make a new one to add to list.
        if (dump.floors.Count > 0)
        {
            foreach(Vector3 a in dump.floors)
            {
                Instantiate(floor, a, Quaternion.identity);
            }

            foreach (Vector3 a in dump.walls)
            {
                Instantiate(wall, a, Quaternion.identity);
            }

            foreach (Monster a in dump.monsters)
            {
                // Spawn a monster prefab, fill the details out from this object.
            }

            foreach (Character a in dump.players)
            {
                // Spawn a Player fab, fill out the info. Will see if need to do something special to make right client get right
                // Character. Maybe add something to Character to keeep track of owner.
                

            }

            foreach (JoinManager a in dump.battles)
            {
                // Spawn Battle Prefab, fill it's join manager with this stuff.
            }

        }
        else
        {
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
                        roomcount++;

                        print("(" + (ULX + zoneSize * J) + "," + (ULY + zoneSize * I) + ") - " + (LRX - ULX) + "x" + (LRY - ULY) +
                                " with center (" + centers[roomcount - 1, 0] + "," + centers[roomcount - 1, 1] + ")");
                        for (int x = ULX; x < LRX; x++)
                        {
                            for (int y = ULY; y < LRY; y++)
                            {
                                isFloor[x + zoneSize * J + 1, y + zoneSize * I + 1] = true;
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
                for (int i = loops % yRooms; i < roomcount; i += yRooms)
                {
                    int j = UnityEngine.Random.Range(0, roomcount - 1);
                    {
                        if (centers[i, 2] != centers[j, 2]) // If rooms arent connected, connect
                        {
                            int r1x = centers[i, 0], r1y = centers[i, 1], r2x = centers[j, 0], r2y = centers[j, 1];
                            //Four cases: R1 above/below R2, and R1 left/right of R2
                            print("Making hallway (" + r1x + "," + r1y + ") to (" + r2x + "," + r2y + ")");
                            int x = r1x;
                            if (x < r2x)
                            {
                                for (x = r1x; x < r2x; x++)
                                {
                                    isFloor[x, r1y] = true;
                                }
                                x--;
                            }
                            else
                            {
                                for (x = r1x; x >= r2x; x--)
                                {
                                    isFloor[x, r1y] = true;
                                }
                                x++;
                            }
                            int y = r1y;
                            if (y < r2y)
                            {
                                for (y = r1y; y < r2y; y++)
                                {
                                    isFloor[x, y] = true;
                                }
                            }
                            else
                            {
                                for (y = r1y; y >= r2y; y--)
                                {
                                    isFloor[x, y] = true;
                                }
                            }
                            if (centers[i, 2] < centers[j, 2])
                            {
                                int room2Value = centers[j, 2];
                                for (int k = 0; k < roomcount; k++)
                                {
                                    if (centers[k, 2] == room2Value) centers[k, 2] = centers[i, 2];
                                }
                            }
                            else
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
                for (int z = 0; z < roomcount; z++)
                {
                    if (centers[z, 2] != 0) allConnected = false;
                }
            }
            print("Took " + loops + " iterations.");

            string dungeon = "";
            GameObject temp;
            for (int i = 0; i < yRooms * zoneSize + 2; i++)
            {
                for (int j = 0; j < xRooms * zoneSize + 2; j++)
                {
                    if (isFloor[j, i])
                    {
                        temp = (GameObject)(Instantiate(floor, new Vector3(j, i, 0f), Quaternion.identity));
                        dungeon += '░';
                        dump.floors.Add(temp.transform.position);
                    }
                    else
                    {
                        temp = (GameObject)Instantiate(wall, new Vector3(j, i, 0f), Quaternion.identity);
                        dungeon += '█';
                        dump.walls.Add(temp.transform.position);

                    }
                }
                dungeon += "  // " + i + "\n";
            }
        }
        //dungeonOut.text = dungeon;
        System.DateTime end = System.DateTime.Now;
        print("Time: " + (start - end));
    }
	// Update is called once per frame
	void Update ()
    {
        
	}
}
