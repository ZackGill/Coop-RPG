using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using AssemblyCSharp;
public class LevelDump : NetworkBehaviour
 {

    public int seed;

    public List<Vector3> walls;
    public List<Vector3> floors;
    public List<Monster> monsters;
    public List<Character> players;
    public List<JoinManager> battles;

    

    // Use this for initialization
    void Start () {
        walls = new List<Vector3>();
        floors = new List<Vector3>();
        monsters = new List<Monster>();
        players = new List<Character>();
        battles = new List<JoinManager>();

        seed = (int)System.DateTime.Now.Ticks;

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
