using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class BattleHolderScript : NetworkBehaviour {
    public GameObject player;
	// Use this for initialization
	void Start () {
        //GetComponentInChildren<Canvas>().worldCamera = Camera.main;

        //Invoke("die", 60f);
	}
	
    public void die()
    {
        if (player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().infoDump != null)
        {
            player.GetComponent<PlayerMovement>().CmdPlayerToggle(true, null, player, player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().infoDump.gameObject, false);
            player.GetComponent<PlayerMovement>().CmdDestroyDump(player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().infoDump.gameObject);
        }
        else
        {
            player.GetComponent<PlayerMovement>().CmdPlayerToggle(true, null, player, null, false);

        }
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
