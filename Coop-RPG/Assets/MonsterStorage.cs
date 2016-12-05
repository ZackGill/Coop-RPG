using UnityEngine;
using System.Collections;
using AssemblyCSharp;
public class MonsterStorage : MonoBehaviour {

    public Monster monster;

	// Use this for initialization
	void Start () {
        monster = new Monster(0, 0, 0, 0, 0, false, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
