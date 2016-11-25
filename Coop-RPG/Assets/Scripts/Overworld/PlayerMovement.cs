using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using AssemblyCSharp;

[NetworkSettings(channel = 0)]
public class PlayerMovement : NetworkBehaviour
{
    public float speed = 2.25F;

    public GameObject battleFab;
    public GameObject overworldBattle;
    public bool inBattle = false;

    // Use this for initialization
    void Start()
    {
        if(isLocalPlayer)
          Invoke("startPos", .1f); 
    }

    void startPos()
    {
        if (!isLocalPlayer)
            return;
        GenerateDungeon temp = GameObject.Find("DungeonGen").GetComponent<GenerateDungeon>();
        switch(Random.Range(0, 4))
        {
            case 0:
                transform.position = temp.spawnLocal.transform.position;
                break;
            case 1:
                transform.position = temp.spawnLocal1.transform.position;
                break;
            case 2:
                transform.position = temp.spawnLocal2.transform.position;
                break;
            case 3:
                transform.position = temp.spawnLocal3.transform.position;
                break;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {

            return;
        }
        else
        {
            if (inBattle)
            {

                return;
            }
            else
            {
                GetComponent<Renderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
            }
            Vector3 pos = transform.position;
            pos.z = -.5f;
            transform.position = pos;
            if (Camera.main == null)
                return;
            Camera.main.GetComponent<moveCamera>().player = gameObject;
        }
        float translation = Time.deltaTime * speed;
		int dir = 0;
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(new Vector3(translation, 0, 0));
			dir = 4;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(new Vector3(-translation, 0, 0));
			dir = 3;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(new Vector3(0, -translation, 0));
			dir = 2;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(new Vector3(0, translation, 0));
			dir = 1;
		}
		GetComponent<Animator> ().SetInteger ("Dir", dir);
    }

    void FixedUpdate()
    {


    }
    public GameObject battle;
    public GameObject monster;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Enemy")
        {
            if (isLocalPlayer)
            {
                battle = (GameObject)Instantiate(battleFab, Vector3.zero, Quaternion.identity);
                inBattle = true;
                CmdPlayerToggle(false, coll.gameObject, gameObject);
                monster = coll.gameObject;
                battle.GetComponent<BattleHolderScript>().player = gameObject;
            }
        }
        if(coll.gameObject.tag == "Player")
        {
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "DustCloud")
        {
            if (isLocalPlayer)
            {
                battle = (GameObject)Instantiate(battleFab, Vector3.zero, Quaternion.identity);
                inBattle = true;
                CmdPlayerToggle(false, null, gameObject);
                monster = null;
                battle.GetComponent<BattleHolderScript>().player = gameObject;

            }
        }
    }

    public GameObject battleDump;

    [Command]
    public void CmdPlayerToggle(bool toggle, GameObject monster, GameObject player)
    {

        player.GetComponent<Renderer>().enabled = toggle;
        player.GetComponent<BoxCollider2D>().enabled = toggle;
        if(monster != null){
            monster.GetComponent<Renderer>().enabled = toggle;
            monster.GetComponent<BoxCollider2D>().enabled = toggle;
        }
        // Battle?
        if(toggle == false)
        {
            battleDump = (GameObject)Instantiate(overworldBattle, player.transform.position, Quaternion.identity);
            OverworldBattle temp2 = battleDump.GetComponent<OverworldBattle>();
            temp2.enemy0 = monster.GetComponent<Monster>(); // Make sure wandering monsters have this script
            temp2.info.numPlayers = 1;
            temp2.info.numEnemies = 1;
            if (battle != null)
            {
                battle.GetComponentInChildren<BattleLogic>().infoDump = temp2;
                battle.GetComponentInChildren<BattleLogic>().playerNum = temp2.info.numPlayers - 1;

                if (temp2.battle0 != null)
                    if (temp2.battle1 != null)
                        if (temp2.battle2 != null)
                            return;
                        else
                            temp2.battle2 = battle.GetComponentInChildren<BattleLogic>();
                    else
                        temp2.battle1 = battle.GetComponentInChildren<BattleLogic>();
                temp2.battle0 = battle.GetComponentInChildren<BattleLogic>();
            }
            NetworkServer.Spawn(battleDump);
        }
        else
        {
            // Destroy the overwold battle thing. Not pulling any data, should be done already.
            Network.Destroy(battleDump);
            inBattle = false;
        }
        RpcUpdatePlayer(toggle, monster, player);
    }

    [ClientRpc]
    public void RpcUpdatePlayer(bool toggle, GameObject monster, GameObject player)
    {
        player.GetComponent<Renderer>().enabled = toggle;
        player.GetComponent<BoxCollider2D>().enabled = toggle;
        if (monster != null)
        {
            monster.GetComponent<Renderer>().enabled = toggle;
            monster.GetComponent<BoxCollider2D>().enabled = toggle;
        }
    }
}
