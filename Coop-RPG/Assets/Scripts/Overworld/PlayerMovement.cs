using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using AssemblyCSharp;
using Prototype.NetworkLobby;
using UnityEngine.UI;
[NetworkSettings(channel = 0)]
public class PlayerMovement : NetworkBehaviour
{

    public GameObject levelUp; // Prefab that is used to spawn level up holder.


    [SyncVar]
    public string charName;

    public float speed = 2.25F;

    [SyncVar]
    public int health;

    public GameObject battleFab;
    public GameObject overworldBattle;
    public bool inBattle = false;

    public Characters characterInfo;
    // Use this for initialization

    public void loadMenu()
    {
        if (!isLocalPlayer)
            return;
        if (!end.Done())
            return;
        if (!isServer)
        {
            print("loading menu");
            Network.CloseConnection(Network.connections[0], true);
            SceneManager.LoadScene("Menu");
        }

    }
    DungeonEnd end;
    GameObject tempLevel;
    public void levelUpFunc()
    {
        if (end != null)
            return;
        if(!isLocalPlayer)
            return;

        

        // Will try to implement this later. Too tired to do right now.
        /*tempLevel = (GameObject)Instantiate(levelUp, Vector3.zero, Quaternion.identity);
         end = tempLevel.GetComponent<DungeonEnd>();
        if (end.charLeveled(characterInfo))
        {
            end.classPerkHelper(characterInfo.getClass()); // calling as function? Should be good
            Invoke("PerkDisplay", 10f);
        }
        else // Didn't level, go back to menu after updating Firebase.
        {
            tempLevel.GetComponent<LevelUpHolder>().perkOption.enabled = false;
            tempLevel.GetComponent<LevelUpHolder>().perk2Option.enabled = false;
            tempLevel.GetComponent<LevelUpHolder>().perk3Option.enabled = false;

            end.levelUp(characterInfo, "", "");
            // Make sure to check if done before loading menu.
            InvokeRepeating("loadMenu", 5f, 1f);
        }*/

    }

    public string perkChoice;

    [Command]
    public void CmdQuit()
    {
        print("Trying to dissconnect");
        Network.Disconnect();
        SceneManager.LoadScene("Menu");
    }

    public void button0Click()
    {
        if (perkOptions.Length >= 1)
        {
            perkChoice = perkOptions[0];
            optionLevelUp();
        }
        else
            end.levelUp(characterInfo, "", "");

    }

    public void button1Click()
    {
        if (perkOptions.Length >= 2)
        {
            perkChoice = perkOptions[1];
            optionLevelUp();
        }
        else
            end.levelUp(characterInfo, "", "");
    }

    public void button2Click()
    {
        if (perkOptions.Length >= 3)
        {
            perkChoice = perkOptions[2];
            optionLevelUp();
        }
        else
            end.levelUp(characterInfo, "", "");

    }

    public void optionLevelUp()
    {
        end.levelUp(characterInfo, "", perkChoice);
    }

    string[] perkOptions;
    public void PerkDisplay()
    {
        string perks = end.perksForLevel(end.checkLevel(characterInfo.getExp() + end.dungeonExp));
        perkOptions = perks.Split(';');
        if (perkOptions.Length >= 2)
        {
            levelUp.GetComponent<LevelUpHolder>().perkOption.GetComponentInChildren<Text>().text = perkOptions[0];
            levelUp.GetComponent<LevelUpHolder>().perk2Option.GetComponentInChildren<Text>().text = perkOptions[0];
            levelUp.GetComponent<LevelUpHolder>().perk3Option.GetComponentInChildren<Text>().text = perkOptions[0];
        }
        else
            end.levelUp(characterInfo, "", "");

    }

    void Start()
    {
        if (isLocalPlayer)
        {
            InvokeRepeating("startPos", .1f, 1f);

        }
    }

    void startPos()
    {
        if (LoadingScript.Instance.loading)
            return;
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
        CancelInvoke();


    }
    // Update is called once per frame
    void Update()
    {
        if (LoadingScript.Instance.loading)
            return;
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
        if (Input.GetKey(KeyCode.Z))
        {
            speed = 4.0f;
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

        if(characterInfo == null)
        {

            for(int i = 0; i < LoadingScript.Instance.chList.Length; i++)
            {
                if(LoadingScript.Instance.chList[i].getName().CompareTo(charName) == 0)
                {
                    characterInfo = LoadingScript.Instance.chList[i];
                }
            }

        }
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);

    }
    public GameObject battle;
    public GameObject monster;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag.Contains("Enemy"))
        {
            if (isLocalPlayer)
            {
                if (LoadingScript.Instance.loading)
                    return;
                // Set forces to 0 to be sure to stop motion.
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;
               // GetComponent<Rigidbody2D>().isKinematic = true;

                speed = 0;
                battle = (GameObject)Instantiate(battleFab, Vector3.zero, Quaternion.identity);
                inBattle = true;
                monster = coll.gameObject;
                battle.GetComponent<BattleHolderScript>().player = gameObject;
                battle.GetComponent<BattleHolderScript>().monster = coll.gameObject;
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
                if (col.gameObject.GetComponent<OverworldBattle>().info.numPlayers >= 3)
                    return;
                if (col.gameObject.GetComponent<OverworldBattle>().info.numEnemies <= 0)
                    return;
                print("Dustcloud hit");
                battle = (GameObject)Instantiate(battleFab, Vector3.zero, Quaternion.identity);
                inBattle = true;
                monster = null;
                battle.GetComponent<BattleHolderScript>().player = gameObject;
                battle.GetComponent<BattleHolderScript>().monster = null;
                CmdPlayerToggle(false, null, gameObject, col.gameObject, true);

            }
        }
    }

    public GameObject battleDump;

    // Used to force level up calls on all clients
    [ClientRpc]
    public void RpcLevelUp()
    {
        levelUpFunc();
    }

    [ClientRpc]
    public void RpcMenu()
    {

        Network.Disconnect();

        SceneManager.LoadScene("Menu");
    }

    [Command]
    public void CmdKillMonster(GameObject monster)
    {
        if (monster.GetComponent<TotallyABoss_Overworld>() != null)
        {
            /*print("Level UP");
            levelUpFunc();
            RpcLevelUp();*/

            Network.Disconnect();

            SceneManager.LoadScene("Menu");

            RpcMenu();
        }
        print("\"Destroying\" monster");



    }

    [Command]
    public void CmdPlayerToggle(bool toggle, GameObject monster, GameObject player, GameObject battleDumpThing, bool existingBattle)
    {
        /*if (toggle)
        {
            if(battleDumpThing != null)
              battleDumpThing.GetComponentInChildren<CircleCollider2D>().enabled = true;
        }*/

       
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
                    temp2.CmdAddMonster(monster); // Make sure wandering monsters have this script

                temp2.CmdAddPlayer(player);

                if (temp2 == null)
                    print("Assigning a null to infodump");

                    if (temp2.battle0 != null)
                        if (temp2.battle1 != null)
                            if (temp2.battle2 != null)
                                return;
                            else
                                temp2.battle2 = player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>();
                        else
                            temp2.battle1 = player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>();

                if (player.GetComponent<PlayerMovement>().battle != null)
                    temp2.battle0 = player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>();
                else
                    print("battle is null");
                RpcUpdatePlayerDump(player, tempB);

            }
            else
            {
                battleDumpThing.GetComponent<OverworldBattle>().CmdAddPlayer(player);

                RpcUpdatePlayerDump(player, battleDumpThing);

            }

        }
        else
        {
            // Destroy the overwold battle thing. Not pulling any data, should be done already. Not doing network, seeing if that 
            // avoids issues from slow leaving.
            if (battleDumpThing != null)
            {
                /*battleDumpThing.GetComponent<CircleCollider2D>().enabled = false;
                battleDumpThing.GetComponent<SpriteRenderer>().enabled = false;
                */
            }
            else
            {
                print("batlte udmp thing null");

            }
            if (battleDump != null)
            {
                //battleDump.GetComponent<CircleCollider2D>().enabled = false;
                //Destroy(battleDump);
            }
           // Destroy(battleDumpThing);
          // battleDumpThing.GetComponent<OverworldBattle>().cmdR
            player.GetComponent<PlayerMovement>().inBattle = false;
        }
        print("Player hitbox and stuff setting " + toggle);
        player.GetComponent<Renderer>().enabled = toggle;
        player.GetComponent<BoxCollider2D>().enabled = toggle;
        if (monster != null)
        {
            monster.GetComponent<Renderer>().enabled = toggle;
            monster.GetComponent<BoxCollider2D>().enabled = toggle;
        }
        else
            print("Null monsters");

        RpcUpdatePlayer(toggle, monster, player, battleDumpThing);


    }

    [Command]
    public void CmdDestroyDump(GameObject dump)
    {
        print("Destroy Dump");
        //Network.Destroy(dump);
    }

    [ClientRpc]
    public void RpcUpdatePlayerDump(GameObject player, GameObject battleDump)
    {
        if (battleDump == null)
        {
            print("BattleDump null");
            Destroy(player.GetComponent<PlayerMovement>().battle);
            return;
        }
        if (battleDump.GetComponent<OverworldBattle>() == null)
            print("Battledump script is null");
        if (player.GetComponent<PlayerMovement>().battle != null)
        {
            print("Battle Logic should be getting info dumped now");
            player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().infoDump = battleDump.GetComponent<OverworldBattle>();
            player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().playerNum = battleDump.GetComponent<OverworldBattle>().info.numPlayers - 1;

          /*  if (battleDump.GetComponent<OverworldBattle>().player0 == null)
                battleDump.GetComponent<OverworldBattle>().player0 = player.GetComponent<Characters>();
            else if (battleDump.GetComponent<OverworldBattle>().player1 == null)
                battleDump.GetComponent<OverworldBattle>().player1 = player.GetComponent<Characters>();
            else if (battleDump.GetComponent<OverworldBattle>().player2 == null)
                battleDump.GetComponent<OverworldBattle>().player2 = player.GetComponent<Characters>();*/

        }
    }

    [ClientRpc]
    public void RpcUpdatePlayer(bool toggle, GameObject monster, GameObject player, GameObject battleDumpThing)
    {
        if (toggle)
        {
            player.GetComponent<PlayerMovement>().inBattle = false;
            if (battleDumpThing != null)
            {
                battleDumpThing.GetComponentInChildren<CircleCollider2D>().enabled = false;
                battleDumpThing.GetComponent<SpriteRenderer>().enabled = false;
            }
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
        else
        {
            print("monster is null");
        }

    }

    [Command]
    public void CmdPlayerDamage(GameObject battleDump, int player, float damage)
    {
        print("Player damage in player");
        battleDump.GetComponent<OverworldBattle>().CmdPlayerDamage(damage, player);


    }

    [Command]
    public void CmdEnemyDamage(GameObject battleDump, float enemy1HP, float enemy2HP, float enemy3HP)
    {
        print("Enemy damage in player");
        battleDump.GetComponent<OverworldBattle>().CmdEnemyDamage(enemy1HP, enemy2HP, enemy3HP);

    }

    [Command]
    public void CmdFightMessage(GameObject battleDump, string fightMessage)
    {

        print("setting fight Message");
        battleDump.GetComponent<OverworldBattle>().CmdFightMessage(fightMessage);

    }

    [Command]
    public void CmdAttackFlag(GameObject battleDump, bool flag)
    {
        print("Setting attack flag");
        battleDump.GetComponent<OverworldBattle>().CmdAttackFlag(flag);
    }

    [Command]
    public void CmdEnemyTime(GameObject battleDump, float one, float two, float three)
    {
        battleDump.GetComponent<OverworldBattle>().CmdEnemyTimes(one, two, three);
    }

    [Command]
    public void CmdEnemyMaxTime(GameObject battleDump, float one, float two, float three)
    {
        battleDump.GetComponent<OverworldBattle>().CmdEnemyMaxTimes(one, two, three);
    }

}
