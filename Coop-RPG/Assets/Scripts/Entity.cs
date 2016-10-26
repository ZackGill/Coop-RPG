using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	int _xp;
	string _class;
	int _attack, _defense, _magic;
	int _hpMax, _hpCurrent;
	CombatMove[] moveList = new CombatMove[3];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
