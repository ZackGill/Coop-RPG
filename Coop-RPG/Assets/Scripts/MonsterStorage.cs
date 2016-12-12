using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.Networking;
public class MonsterStorage : NetworkBehaviour {

    public Monster monster;
    public int health;
	// Use this for initialization
	void Start () {
        //monster = new Monster(0, 0, 0, 0, 0, false, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void CheckStuff()
    {
        if (monster == null)
        {
            print(gameObject + "monster is null");
        }
    }

    void OnDestroy()
    {
        print("Monster Holder Destroyed");
    }


}
