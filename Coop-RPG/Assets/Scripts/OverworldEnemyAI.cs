using UnityEngine;
using System.Collections;
class coord
{
    public int x, y;
    public coord parent;
    public bool hasParent;
    public coord(int xP, int yP, coord myParent)
    {
        x = xP;
        y = yP;
        parent = myParent;
        hasParent = true;
    }
    public coord(int xP, int yP)
    {
        x = xP;
        y = yP;
        parent = null;
        hasParent = false;
    }
}
public class OverworldEnemyAI : MonoBehaviour
{
    public float speed = 2.25F;
    private int x, y, tX, tY, lX, lY, steps;
    private float stuckInc = 0F;
    private Stack  myPath;
    private Queue checkPos;
    public bool[,] map;
    bool[,] visited;
    private coord next;
    bool exists = false;
    // Use this for initialization
    void Start()
    {
        GameObject scr = GameObject.Find("EventSystem");
        map = GameObject.Find("EventSystem").GetComponent<GenerateDungeon>().isFloor;
        exists = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (exists)
        {
            if (steps <= 0 || (x == tX && y == tY))
            {
                do
                {
                    tX = Random.Range(0, map.GetLength(0));
                    tY = Random.Range(0, map.GetLength(1));
                } while (!map[tX, tY]);
                x = Mathf.FloorToInt(transform.position.x);
                y = Mathf.FloorToInt(transform.position.y);
                visited = new bool[map.GetLength(0), map.GetLength(1)];
                checkPos = new Queue();
                myPath = new Stack();
                coord st = new coord(x, y);
                checkPos.Enqueue(st);
                steps = 0;
                print("Pathing " + x + "," + y + " to " + tX + ',' + tY);
                int tries = 0;
                coord cur;
                do
                {
                    tries++;
                    cur = (coord)checkPos.Dequeue();
                    if (!visited[cur.x, cur.y]) {
                        visited[cur.x, cur.y] = true;
                        if (y > 0 && map[cur.x, cur.y - 1] && !visited[cur.x, cur.y - 1]) checkPos.Enqueue(new coord(cur.x, cur.y - 1, cur));
                        if (y < map.GetLength(1) && map[cur.x, cur.y + 1] && !visited[cur.x, cur.y + 1]) checkPos.Enqueue(new coord(cur.x, cur.y + 1, cur));
                        if (x > 0 && map[cur.x - 1, cur.y] && !visited[cur.x - 1, cur.y]) checkPos.Enqueue(new coord(cur.x - 1, cur.y, cur));
                        if (x < map.GetLength(0) && map[cur.x + 1, cur.y] && !visited[cur.x + 1, cur.y]) checkPos.Enqueue(new coord(cur.x + 1, cur.y, cur));
                    }
                } while (tries < map.GetLength(0) * map.GetLength(1) && !(cur.x == tX && cur.y == tY));
                while (cur.hasParent)
                {
                    steps++;
                    myPath.Push(cur);
                    cur = cur.parent;
                }
                steps--;
                next = (coord)myPath.Pop();
            }

            int nX = next.x;
            int nY = next.y;
            float translation = Time.deltaTime * speed;
            if (x < nX)
            {
                transform.Translate(new Vector3(translation, 0, 0));
                x = Mathf.FloorToInt(transform.position.x);
            }
            else if (x > nX || (transform.position.x - x) > translation*2)
            {
                transform.Translate(new Vector3(-translation, 0, 0));
                x = -1*Mathf.FloorToInt(-1*transform.position.x);
            }
            if (y < nY)
            {
                transform.Translate(new Vector3(0, translation, 0));
                y = Mathf.FloorToInt(transform.position.y);
            }
            else if (y > nY || (transform.position.y - y) > translation * 2)
            {
                transform.Translate(new Vector3(0, -translation, 0));
                y = -1 * Mathf.FloorToInt(-1 * transform.position.y);
            }
            if (x == nX && y == nY && !(x == tX && y == tY))
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
}
