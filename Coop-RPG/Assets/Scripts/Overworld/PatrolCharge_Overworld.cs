using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using AssemblyCSharp;
public class PatrolCharge_Overworld : NetworkBehaviour
{
    private GameObject[] playerPos = null;
    public int sightRange = 15;
    public float speed = 1.65F, chargeTimer = 0F;
    private int x, y, tX, tY, lX, lY, steps, NORTH = 0, EAST = 1, SOUTH = 2, WEST = 3, dir = 0, patrolStep = 0;
    private coord[] patrolRoute = new coord[4];
    private float stuckInc = 0F, CHARGETIME = 3F, RAM_MOD = 3.25F;
    private Stack myPath;
    private Queue checkPos;
    public bool[,] map;
    bool[,] visited;
    private coord next;
    bool exists = false, hunting = false;
    //why changes no push

    void OnDestroy()
    {
        print("PatrolCharge destroyed");
    }


    // Use this for initialization
    void Start()
    {
        map = transform.parent.GetComponent<GenerateDungeon>().isFloor;
        exists = true;
        do
        {
            x = Random.Range(0, map.GetLength(0));
            y = Random.Range(0, map.GetLength(1));
        } while (!map[x, y]);
        patrolRoute[0] = new coord(x, y);
        do
        {
            x = Random.Range(0, map.GetLength(0));
            y = Random.Range(0, map.GetLength(1));
        } while (!map[x, y]);
        patrolRoute[1] = new coord(x, y);
        do
        {
            x = Random.Range(0, map.GetLength(0));
            y = Random.Range(0, map.GetLength(1));
        } while (!map[x, y]);
        patrolRoute[2] = new coord(x, y);
        do
        {
            x = Random.Range(0, map.GetLength(0));
            y = Random.Range(0, map.GetLength(1));
        } while (!map[x, y]);
        patrolRoute[3] = new coord(x, y);
       /* print("Patrol is |" + patrolRoute[0].x + "," + patrolRoute[0].y + "| -> |"
            + patrolRoute[1].x + "," + patrolRoute[1].y + "| -> |"
            + patrolRoute[2].x + "," + patrolRoute[2].y + "| -> |"
            + patrolRoute[3].x + "," + patrolRoute[3].y + "| -> |" + patrolRoute[0].x + "," + patrolRoute[0].y + "|");*/
    }

    bool canSeeEachOther(int x1, int y1, int x2, int y2)
    {
        if(Mathf.Abs(x2-x1) < Mathf.Abs(y2-y1))
        {
            float invSlope = (float)(x2 - x1) / (float)(y2 - y1);
            //For each X, find it's approx Y on the line of sight. If that coord is a wall, return false;
            if (y1 < y2)
            {
                for (int yPos = y1; yPos < y2; yPos++)
                {
                    if (!map[y1 + Mathf.RoundToInt((yPos - y1) * invSlope),yPos]) return false;
                }
            }
            else
            {
                for (int yPos = y2; yPos < y1; yPos++)
                {
                    if (map.Length < y1 + Mathf.RoundToInt((yPos - y2) * invSlope))
                    {
                        continue;
                    }
                    if (!map[y1 + Mathf.RoundToInt((yPos - y2) * invSlope), yPos]) return false;
                }
            }

            return true;
        }
        
        float slope = (float)(y2 - y1) / (float)(x2 - x1);
        //For each X, find it's approx Y on the line of sight. If that coord is a wall, return false;
        if (x1 < x2)
        {
            for (int xPos = x1; xPos < x2; xPos++)
            {
                if (!map[xPos, y1 + Mathf.RoundToInt((xPos - x1) * slope)]) return false;
            }
        }
        else
        {
            for (int xPos = x2; xPos < x1; xPos++)
            {
                if (!map[xPos, y1 + Mathf.RoundToInt((xPos - x2) * slope)]) return false;
            }
        }
        
        return true;
    }

    void makePathToTarg()
    {
        x = Mathf.FloorToInt(transform.position.x);
        y = Mathf.FloorToInt(transform.position.y);
        visited = new bool[map.GetLength(0), map.GetLength(1)];
        checkPos = new Queue();
        myPath = new Stack();
        coord st = new coord(x, y);
        checkPos.Enqueue(st);
        steps = 0;
        //print("Pathing " + x + "," + y + " to " + tX + ',' + tY);
        int tries = 0;
        coord cur = null;
        int queuesize = 1;
        do
        {
            tries++;
            if (queuesize > 0) cur = (coord)checkPos.Dequeue();
            else break;
            queuesize--;
            if (!visited[cur.x, cur.y])
            {
                visited[cur.x, cur.y] = true;
                if (y > 0 && map[cur.x, cur.y - 1] && !visited[cur.x, cur.y - 1])
                {
                    checkPos.Enqueue(new coord(cur.x, cur.y - 1, cur)); queuesize++;
                }
                if (y < map.GetLength(1) && map[cur.x, cur.y + 1] && !visited[cur.x, cur.y + 1])
                {
                    checkPos.Enqueue(new coord(cur.x, cur.y + 1, cur)); queuesize++;
                }
                if (x > 0 && map[cur.x - 1, cur.y] && !visited[cur.x - 1, cur.y])
                {
                    checkPos.Enqueue(new coord(cur.x - 1, cur.y, cur)); queuesize++;
                }
                if (x < map.GetLength(0) && map[cur.x + 1, cur.y] && !visited[cur.x + 1, cur.y])
                {
                    checkPos.Enqueue(new coord(cur.x + 1, cur.y, cur)); queuesize++;
                }
            }
        } while (tries < map.GetLength(0) * map.GetLength(1) && !(cur.x == tX && cur.y == tY));
        while (cur.hasParent)
        {
            steps++;
            myPath.Push(cur);
            cur = cur.parent;
        }
        steps--;
        if(myPath.Count > 0)
         next = (coord)myPath.Pop();
    }

    // Update is called once per frame
    void Update()
    {
        if (LoadingScript.Instance.loading)
            return;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        if (playerPos == null)
        {
            playerPos = GameObject.FindGameObjectsWithTag("Player");
        }
        if (exists)
        {
            if (steps <= 0 || (x == tX && y == tY))
            {
                checkForPlayer(dir);
                if(!hunting)chargeTimer = 0F;
                for (int i = 0; i < 4; i++)
                {
                    checkForPlayer(i);
                    if (hunting) i = 5;
                }
                if (!hunting)
                    //If not hunting, continue patrolling
                {
                    tX = patrolRoute[patrolStep].x;
                    tY = patrolRoute[patrolStep].y;
                    patrolStep++;
                    patrolStep %= 4;
                }
                makePathToTarg();
            }
            
            if (hunting && chargeTimer < CHARGETIME)
            {

                chargeTimer += Time.deltaTime;
                //While preparing for a charge, make sure we can still see the player.
                for (int i = 0; i < 4; i++)
                {
                    checkForPlayer(i);
                    if (hunting) i = 5;
                }
                //We saw the player! Pause for 2 seconds, then charge.
                if (chargeTimer >= CHARGETIME) ;
            }
            else
            {
                if(!hunting)chargeTimer = 0F;
                //                else checkForPlayer(dir);
                //If not hunting slow down, otherwise check if you can see the player.
                if (next == null)
                    return;
                int nX = next.x;
                int nY = next.y;
                float translation = Time.deltaTime * speed;
                if (hunting) translation *= RAM_MOD;
                //If we are charging, go 3x speed
                if (x < nX)
                {
                    transform.Translate(new Vector3(translation, 0, 0));
                    x = Mathf.FloorToInt(transform.position.x + 0.1F);
                    dir = EAST;
                }
                else if (x > nX)
                {
                    transform.Translate(new Vector3(-translation, 0, 0));
                    x = -1 * Mathf.FloorToInt(-1 * (transform.position.x + 0.1F));
                    dir = WEST;
                }
                if (y < nY)
                {
                    transform.Translate(new Vector3(0, translation, 0));
                    y = Mathf.FloorToInt(transform.position.y + 0.1F);
                    dir = NORTH;
                }
                else if (y > nY)
                {
                    transform.Translate(new Vector3(0, -translation, 0));
					y = -1 * Mathf.FloorToInt(-1 * (transform.position.y + 0.1F));
                    dir = SOUTH;
                }
                if (x == nX && y == nY && !(x == tX && y == tY))
                {
                    next = (coord)myPath.Pop();
                    steps--;
                }

                if (x == lX && y == lY) stuckInc += Time.deltaTime; else stuckInc = 0;
				/*Ray rayForward = new Ray(transform.position, transform.forward);
				RaycastHit hit;
				if (Physics.Raycast (rayForward, out hit, 1.75f)) {
					if (hit.transform.tag == "Wall") {
						print (hit.transform.tag);
						steps = -1;
					}
				}*/


                if (stuckInc > 0.5F) steps = -1; //we're stuck, lets path to a new spot.
                lX = x;
                lY = y;
                if (!hunting)
                {
                    checkForPlayer(dir);
                    if (hunting)
                        steps = -1;
                    //PLAYER SPOTTED ON PATROL. PREPARE TO CHARGE.
                }
            }
        }
    }

    void checkForPlayer(int direction)
    {
        float dist = float.MaxValue;
        bool tFound = false;
        for (int i = 0; i < playerPos.Length; i++)
        {
            int pX = Mathf.FloorToInt(playerPos[i].transform.position.x + 0.5F);
            int pY = Mathf.FloorToInt(playerPos[i].transform.position.y + 0.5F);
            float pDistance = Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y));
            if (pDistance < dist)
            {
                if (direction == EAST && pX > x && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                                && canSeeEachOther(x, y, pX, pY))
                {
                    tX = pX + 1;
                    tY = pY;
                    tFound = true;
                    dist = pDistance;
                    hunting = true;
                }
                if (direction == WEST && pX < x && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                                && canSeeEachOther(x, y, pX, pY))
                {
                    tX = pX - 1;
                    tY = pY;
                    tFound = true;
                    dist = pDistance;
                    hunting = true;
                }
                if (direction == NORTH && pY > y && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                                && canSeeEachOther(x, y, pX, pY))
                {
                    tX = pX;
                    tY = pY + 1;
                    tFound = true;
                    dist = pDistance;
                    hunting = true;
                }
                if (direction == SOUTH && pY < y && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                                && canSeeEachOther(x, y, pX, pY))
                {
                    tX = pX;
                    tY = pY - 1;
                    tFound = true;
                    dist = pDistance;
                    hunting = true;
                }
            }
        }
        hunting = tFound;
        return;
    }
}
