using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int runVal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public CombatMove ChooseMove()
    {
        return new CombatMove();
    }
}
