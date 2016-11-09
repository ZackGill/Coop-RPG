using UnityEngine;
using System.Collections;

public class TotallyABossOverworld : MonoBehaviour
{
    private GameObject playerPos = null;
    public int sightRange = 30;
    public float speed = 1.25F;
    private int x, y, tX, tY, lX, lY, steps, NORTH = 0, EAST = 1, SOUTH = 2, WEST = 3, dir = 0, WANDERRADIUS = 2, homeX, homeY;
    private float stuckInc = 0F, idleTimer = float.MaxValue;
    private Stack myPath;
    private Queue checkPos;
    public bool[,] map;
    bool[,] visited;
    private coord next;
    bool exists = false, hunting = false;
    // Use this for initialization
    void Start()
    {
        GameObject scr = GameObject.Find("EventSystem");
        map = GameObject.Find("DungeonGen").GetComponent<GenerateDungeon>().isFloor;
        exists = true;
        playerPos = GameObject.FindGameObjectWithTag("Player");
        homeX = Mathf.FloorToInt(transform.position.x);
        homeY = Mathf.FloorToInt(transform.position.y);
    }
    bool canSeeEachOther(int x1, int y1, int x2, int y2)
    {
        if (Mathf.Abs(x2 - x1) < Mathf.Abs(y2 - y1))
        {
            float invSlope = (float)(x2 - x1) / (float)(y2 - y1);
            //For each X, find it's approx Y on the line of sight. If that coord is a wall, return false;
            if (y1 < y2)
            {
                for (int yPos = y1; yPos < y2; yPos++)
                {
                    if (!map[y1 + Mathf.RoundToInt((yPos - y1) * invSlope), yPos]) return false;
                }
            }
            else
            {
                for (int yPos = y2; yPos < y1; yPos++)
                {
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
        x = Mathf.FloorToInt(transform.position.x + 0.5F);
        y = Mathf.FloorToInt(transform.position.y + 0.5F);
        visited = new bool[map.GetLength(0), map.GetLength(1)];
        checkPos = new Queue();
        myPath = new Stack();
        coord st = new coord(x, y);
        checkPos.Enqueue(st);
        steps = 0;
        print("Pathing " + x + "," + y + " to " + tX + ',' + tY);
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
        if(x != tX || y != tY) next = (coord)myPath.Pop();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos == null)
            playerPos = GameObject.Find("PlayerChar");
        if (exists)
        {
            if (!hunting)
            {
                for (int i = 0; i < 4; i++)
                {
                    checkForPlayer(i);
                    if (hunting) i = 5;
                }
                //No player found, wander randomly
                x = Mathf.FloorToInt(transform.position.x + 0.5F);
                y = Mathf.FloorToInt(transform.position.y + 0.5F);
                do
                {
                    tX = homeX + UnityEngine.Random.Range(-WANDERRADIUS, WANDERRADIUS) * 2;
                    tY = homeY + UnityEngine.Random.Range(-WANDERRADIUS, WANDERRADIUS) * 2;
                    if (tY < 0) tY = 0;
                    if (tX < 0) tX = 0;
                    if (tY >= map.GetLength(1)) tY = map.GetLength(1)-1;
                    if (tX >= map.GetLength(0)) tX = map.GetLength(0)-1;
                    //All steps are at least 2 away from spawn
                } while (!map[tX, tY] || (tX == x && tY == y));

                idleTimer += Time.deltaTime;
                //Wander only every 3 seconds.
                if (idleTimer > 3F || hunting)
                {
                    makePathToTarg();
                    idleTimer = 0F;
                }
            }
            else
            {
                print("CHASE");
                //A player has been spotted at some point. Hunt down a player!
                tX = Mathf.FloorToInt(playerPos.transform.position.x + 0.5F);
                tY = Mathf.FloorToInt(playerPos.transform.position.y + 0.5F);
                makePathToTarg();
            }

            int nX = next.x;
            int nY = next.y;

            float translation = Time.deltaTime * speed;
            if (hunting) translation *= 1.5F;
            //Run 50% faster while chasing
            if (x < nX)
            {
                dir = EAST;
                transform.Translate(new Vector3(translation, 0, 0));
                x = Mathf.FloorToInt(transform.position.x + 0.5F);
            }
            else if (x > nX)
            {
                dir = WEST;
                transform.Translate(new Vector3(-translation, 0, 0));
                x = -1 * Mathf.FloorToInt(-1 * (transform.position.x + 0.5F));
            }
            else if (Mathf.Abs(transform.position.x - x) > translation)
            {
                if(x < transform.position.x)
                    transform.Translate(new Vector3(-1 * translation, 0, 0));
                else
                    transform.Translate(new Vector3(translation, 0, 0));
            }
            if (y < nY)
            {
                dir = NORTH;
                transform.Translate(new Vector3(0, translation, 0));
                y = Mathf.FloorToInt(transform.position.y + 0.5F);
                dir = NORTH;
            }
            else if (y > nY)
            {
                dir = SOUTH;
                transform.Translate(new Vector3(0, -translation, 0));
                y = -1 * Mathf.FloorToInt(-1 * (transform.position.y + 0.5F));
            }
            else if (Mathf.Abs(transform.position.y - y) > translation)
            {
                if (y < transform.position.y)
                    transform.Translate(new Vector3(0, -1 * translation, 0));
                else
                    transform.Translate(new Vector3(0, translation, 0));
            }

            if (steps > 0 && x == nX && y == nY && !(x == tX && y == tY))
            {
                next = (coord)myPath.Pop(); 
                steps--;
            }
            if (x == lX && y == lY) stuckInc += Time.deltaTime; else stuckInc = 0;
            if (stuckInc > 0.5F) steps = -1; //we're stuck, lets path to a new spot.
            lX = x;
            lY = y;
        }
    }

    void checkForPlayer(int direction)
    {
        int pX = Mathf.FloorToInt(playerPos.transform.position.x + 0.5F);
        int pY = Mathf.FloorToInt(playerPos.transform.position.y + 0.5F);
        if (direction == EAST && pX > x && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                        && canSeeEachOther(x, y, pX, pY))
        {
            tX = pX;
            tY = pY;
            hunting = true;
            return;
        }
        if (direction == WEST && pX < x && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                        && canSeeEachOther(x, y, pX, pY))
        {
            tX = pX;
            tY = pY;
            hunting = true;
            return;
        }
        if (direction == NORTH && pY > y && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                        && canSeeEachOther(x, y, pX, pY))
        {
            tX = pX;
            tY = pY;
            hunting = true;
            return;
        }
        if (direction == SOUTH && pY < y && Mathf.Sqrt((pX - x) * (pX - x) + (pY - y) * (pY - y)) <= sightRange
                        && canSeeEachOther(x, y, pX, pY))
        {
            tX = pX;
            tY = pY;
            hunting = true;
            return;
        }
        hunting = false;
        return;
    }
}
