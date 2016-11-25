﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class BattleHolderScript : NetworkBehaviour {
    public GameObject player;
	// Use this for initialization
	void Start () {
        Invoke("die", 120f);
	}
	
    void die()
    {
        player.GetComponent<PlayerMovement>().CmdPlayerToggle(true, null, player);
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
