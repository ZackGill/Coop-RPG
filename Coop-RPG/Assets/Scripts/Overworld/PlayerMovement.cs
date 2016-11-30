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
                speed = 0;
                Vector3 temp = transform.position;
                temp.z = -.5f;
                transform.position = temp;
                if (Camera.main == null)
                    return;
                Camera.main.GetComponent<moveCamera>().player = gameObject;
                return;
            }
            else
            {
                speed = 2.25F;
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
        transform.rotation = Quaternion.Euler(Vector3.zero);

    }
    public GameObject battle;
    public GameObject monster;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Enemy")
        {
            if (isLocalPlayer)
            {
                // Set forces to 0 to be sure to stop motion.
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;
               // GetComponent<Rigidbody2D>().isKinematic = true;

                speed = 0;
                battle = (GameObject)Instantiate(battleFab, Vector3.zero, Quaternion.identity);
                inBattle = true;
                monster = coll.gameObject;
                battle.GetComponent<BattleHolderScript>().player = gameObject;
                if (battle != null)
                {
                    

                    coll.gameObject.GetComponent<Renderer>().enabled = false;
                    coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;

                    CmdPlayerToggle(false, coll.gameObject, gameObject, battleDump, false);

                }

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
                if (inBattle)
                    return;
                print("Dustcloud hit");
                battle = (GameObject)Instantiate(battleFab, Vector3.zero, Quaternion.identity);
                inBattle = true;
                monster = null;
                battle.GetComponent<BattleHolderScript>().player = gameObject;

              


                CmdPlayerToggle(false, null, gameObject, col.gameObject, true);

            }
        }
    }

    public GameObject battleDump;

    [Command]
    public void CmdPlayerToggle(bool toggle, GameObject monster, GameObject player, GameObject battleDumpThing, bool existingBattle)
    {
        /*if (toggle)
        {
            if(battleDumpThing != null)
              battleDumpThing.GetComponentInChildren<CircleCollider2D>().enabled = true;
        }*/

        player.GetComponent<Renderer>().enabled = toggle;
        player.GetComponent<BoxCollider2D>().enabled = toggle;
        if(monster != null){
            monster.GetComponent<Renderer>().enabled = toggle;
            monster.GetComponent<BoxCollider2D>().enabled = toggle;
        }
        // Battle?
        if(toggle == false)
        {
            if (!existingBattle)
            {
                print("Network server battle dump should spawn");                
                    print("Spawn and auth?");
                    GameObject tempB = (GameObject)Instantiate(overworldBattle, transform.position, Quaternion.identity);
                    NetworkServer.SpawnWithClientAuthority(tempB, connectionToClient);

                    OverworldBattle temp2 = tempB.GetComponent<OverworldBattle>();
                    temp2.enemy0 = monster.GetComponent<Monster>(); // Make sure wandering monsters have this script
                    temp2.info.numPlayers = 1;
                    temp2.info.numEnemies = 1;

                if (temp2 == null)
                    print("Assigning a null to infodump");
                if (player.GetComponent<PlayerMovement>().battle != null)
                {
                    player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().infoDump = temp2;
                    player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().playerNum = temp2.info.numPlayers - 1;
                }
                    if (temp2.battle0 != null)
                        if (temp2.battle1 != null)
                            if (temp2.battle2 != null)
                                return;
                            else
                                temp2.battle2 = battle.GetComponentInChildren<BattleLogic>();
                        else
                            temp2.battle1 = battle.GetComponentInChildren<BattleLogic>();
                    temp2.battle0 = battle.GetComponentInChildren<BattleLogic>();

                RpcUpdatePlayerDump(player, battleDumpThing);
                
            }
        }
        else
        {
            // Destroy the overwold battle thing. Not pulling any data, should be done already.
            Network.Destroy(battleDumpThing);
            player.GetComponent<PlayerMovement>().inBattle = false;
        }
        RpcUpdatePlayer(toggle, monster, player, battleDumpThing);
    }

    [ClientRpc]
    public void RpcUpdatePlayerDump(GameObject player, GameObject battleDump)
    {
        if (battleDump == null)
        {
            print("BattleDump null");
            return;
        }
        if (battleDump.GetComponent<OverworldBattle>() == null)
            print("Battledump script is null");
        if (player.GetComponent<PlayerMovement>().battle != null)
        {
            player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().infoDump = battleDump.GetComponent<OverworldBattle>();
            player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().playerNum = battleDump.GetComponent<OverworldBattle>().info.numPlayers - 1;
        }
    }

    [ClientRpc]
    public void RpcUpdatePlayer(bool toggle, GameObject monster, GameObject player, GameObject battleDumpThing)
    {
        if (toggle)
        {
            player.GetComponent<PlayerMovement>().inBattle = false;
           /* if (battleDumpThing != null)
                battleDumpThing.GetComponentInChildren<CircleCollider2D>().enabled = true;*/
            
        }
        else
        {
            if(battleDumpThing != null)
              player.GetComponent<PlayerMovement>().battleDump = battleDumpThing;

        }

        player.GetComponent<Renderer>().enabled = toggle;
        player.GetComponent<BoxCollider2D>().enabled = toggle;
        if (monster != null)
        {
            print("Setting monster stuff");
            monster.GetComponent<Renderer>().enabled = toggle;
            monster.GetComponent<BoxCollider2D>().enabled = toggle;
        }

    }
}
