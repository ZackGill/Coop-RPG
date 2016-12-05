using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
// This is kind of the logic behind the GUI and the states that influence it. The logic for the damage and moves being done is in 
// BattleAttackHandler.
public class BattleLogic : NetworkBehaviour
{
    // Networking stuff. Used to synch values properly.
    public OverworldBattle infoDump;
    public int playerNum; // Is this player 0, 1, or 2 in a battle?

 /*   public delegate void PlayerDamageDelegate(float amount, int playerNum);
    public delegate void EnemyDamageDelegate(float amount0, float amount1, float amount2);

    [SyncEvent]
    public event PlayerDamageDelegate EventPlayerDamage = delegate { };
    [SyncEvent]
    public event EnemyDamageDelegate EventEnemyDamage = delegate { };*/



    // Many of the following variables were needed for testing pre-firebase and should be removed.
    public int numPlayers;
    public int numEnemies;
    public int whichSkill = -1;
    // Flags so we don't attack more than once per turn.
    private bool playerAttackFlag = false;
    private bool enemyAttackFlag = false;
    // Information about the player.
    private string playerName;
    private float playerMaxHP;
    private float playerHP;
    private float playerExperience;
    private int playerLevel;
    public bool currentMoveSelected = false;
    // Information about the enemy.
    private Monster[] enemies;
    private float enemyHP;
    private float enemy2HP;
    private float enemy3HP;
    // Information about the battle itself.
    private string fightMessage;
    // So we can affect the state and timer when necessary.
    private ArrowSelection selection;
    private Characters character;
    private BattleScreenStates state;
    private ActiveTime activeTime;
    private ActiveTime enemy2ActiveTime;
    private ActiveTime enemy3ActiveTime;
    private BattleAttackHandler attack;
    public List<BattleScreenStates.FightStates> stateQueue;

    bool dumpLoad = true;

    // Used by Zack to test Mulitplayer synching. Using this so don't have to try skills and all,
    // since their use isn't completed in the battle code by Lex and Bob, don't want to spend hours porting it
    // to the prefab and solving merge issues if I'm just going to have to do it again. 
    [Command]
    public void CmdTest()
    {
        if (infoDump == null)
        {
            print("InfoDump is null. Darn");
            return;
        }

        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdPlayerDamage(infoDump.gameObject, 0, 10);
        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdEnemyDamage(infoDump.gameObject, 5, 0, 0);

        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdFightMessage(infoDump.gameObject, "Did the fight Message work");


        // infoDump.CmdPlayerDamage(10, 0);
        //  infoDump.CmdEnemyDamage(5, 0, 0);
    }

    public void sendFightMessage(string message)
    {
        if (infoDump == null)
            return;
        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdFightMessage(infoDump.gameObject, message);

    }

   public void sendEnemyDamage(float enemy0, float enemy1, float enemy2)
    {
        if (infoDump == null)
            return;
        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdEnemyDamage(infoDump.gameObject, enemy0, enemy1, enemy2);

    }

   public void sendPlayerDamage(float damage)
    {
        if (infoDump == null)
            return;
        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdPlayerDamage(infoDump.gameObject, playerNum, damage);

    }

    public void sendAttackFlag(bool flag)
    {
        if (infoDump == null)
            return;
        enemyAttackFlag = flag;
        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdAttackFlag(infoDump.gameObject, flag);
    }


    void sendEnemyTime()
    {
        if (infoDump == null)
            return;
        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdEnemyTime(infoDump.gameObject, activeTime.getEnemySec(), enemy2ActiveTime.getEnemySec(), enemy3ActiveTime.getEnemySec());
    }

    void sendEnemyMaxTime()
    {
        if (infoDump == null)
            return;
        transform.parent.GetComponent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdEnemyMaxTime(infoDump.gameObject, activeTime.getEnemyMax(), enemy2ActiveTime.getEnemyMax(), enemy3ActiveTime.getEnemyMax());
    }

    void setEnemyTime()
    {
        if (infoDump == null)
            return;
        if(activeTime != null)
        activeTime.setEnemySeconds(infoDump.info.enemy0Sec);
        if(enemy2ActiveTime != null)
        enemy2ActiveTime.setEnemySeconds(infoDump.info.enemy1Sec);
        if(enemy3ActiveTime != null)
        enemy3ActiveTime.setEnemySeconds(infoDump.info.enemy2Sec);

    }

    void setEnemyMaxTime()
    {
        if (infoDump == null)
            return;
        activeTime.setEnemyMaxTime(infoDump.info.enemy0Max);
        if(enemy2ActiveTime != null)
            enemy2ActiveTime.setEnemyMaxTime(infoDump.info.enemy1Max);
        if(enemy3ActiveTime != null)
            enemy3ActiveTime.setEnemyMaxTime(infoDump.info.enemy2Max);

    }

    void setEnemyHP()
    {
        if (infoDump == null)
            return;
        enemyHP = infoDump.info.enemyHP;
        enemy2HP = infoDump.info.enemy2HP;
        enemy3HP = infoDump.info.enemy3HP;

    }

    void setPlayerHP()
    {
        if (infoDump == null)
            return;
        switch (playerNum)
        {
            case 0:
                playerHP = infoDump.info.player0HP;
                break;
            case 1:
                playerHP = infoDump.info.player1HP;
                break;
            case 2:
                playerHP = infoDump.info.player2HP;
                break;
            default:
                break;
        }
    }

    void setFightMessage()
    {
        if (infoDump == null)
            return;
        fightMessage = infoDump.info.fightMessage;

    }


    // Instead of start function, call this with a little delay (maybe .1s). 
    // This will assign everything from infoDump. Delay is used to make sure Info dump has been assigned.
    void startFromDump()
    {
        attack = GetComponent<BattleAttackHandler>();
        state = GetComponent<BattleScreenStates>();
        selection = GetComponent<ArrowSelection>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();


        enemies = new Monster[3];
        if (infoDump != null)
        {
            enemies[0] = infoDump.enemy0;
            enemies[1] = infoDump.enemy1;
            enemies[2] = infoDump.enemy2;
            activeTime.setEnemyMaxTime(infoDump.info.enemy0Max);
            activeTime.setEnemySeconds(infoDump.info.enemy0Sec);

            if(infoDump.info.numEnemies > 2)
            {
                enemy2ActiveTime.setEnemyMaxTime(infoDump.info.enemy1Max);
                enemy2ActiveTime.setEnemySeconds(infoDump.info.enemy1Sec);
                enemy3ActiveTime.setEnemyMaxTime(infoDump.info.enemy2Max);
                enemy3ActiveTime.setEnemySeconds(infoDump.info.enemy2Sec);
            }
            else if(infoDump.info.numEnemies > 1)
            {
                enemy2ActiveTime.setEnemyMaxTime(infoDump.info.enemy1Max);
                enemy2ActiveTime.setEnemySeconds(infoDump.info.enemy1Sec);
            }

            numPlayers = infoDump.info.numPlayers;
            numEnemies = infoDump.info.numEnemies;
            playerNum = infoDump.info.numPlayers - 1;
            setEnemyHP();
            
            switch (playerNum)
            {
                case 0:
                    character = infoDump.player0;
                    break;
                case 1:
                    character = infoDump.player1;
                    break;
                case 2:
                    character = infoDump.player2;
                    break;
                default:
                    print("Character still null in Battle Logic");
                    break;
            }
            playerMaxHP = character.getHP();
            setPlayerHP();

            enemyAttackFlag = infoDump.info.enemyAttackFlag;


            setEnemyMaxTime();
            setEnemyTime();
            GetComponent<BattleScreenGUI>().fillSkillButtons();
        }
        else
        {
            print("infoDUmp null");
        }
        dumpLoad = false;

    }

    // TODO: Replace new monsters and characters with pulling references from collided objects.
    void Start()
    {
        Invoke("startFromDump", 2f);
    }

    void Update() {
        if (infoDump == null)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("DustCloud");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.position.x >= transform.parent.GetComponent<BattleHolderScript>().player.transform.position.x - .6f && temp[i].transform.position.x <= transform.parent.GetComponent<BattleHolderScript>().player.transform.position.x + .6f)
                {
                    if (temp[i].transform.position.y >= transform.parent.GetComponent<BattleHolderScript>().player.transform.position.y - .6f && temp[i].transform.position.y <= transform.parent.GetComponent<BattleHolderScript>().player.transform.position.y + .6f) {
                        print("Assign infodump");
                        infoDump = temp[i].GetComponent<OverworldBattle>();
                    }
                }
            }

        }
        if (playerNum >= 0 || playerNum < 3)
        {
            setEnemyHP();
            setPlayerHP();
            setFightMessage();
        }
       // print(state.curState);

	//FOR TESTING TODO: REMOVE
	if(Input.GetKeyDown("space"))
		toggleState();
	
	if(Input.GetKeyDown(KeyCode.J))
	{
		numEnemies++;
		if(numEnemies > 3)
			numEnemies = 3;
		moreEnemies();
		toggleState();

	}
	if(Input.GetKeyDown(KeyCode.K))
	{
		numPlayers++;
		morePlayers();
		toggleState();
	}
        if(!dumpLoad)
            checkBattleOver();
        stateCheck();
    }

	// This is fetching data from database. Could probably not do that now that
	// we have a loading script and Characters should be set.
    IEnumerator updateCharacter()
    {
        yield return new WaitForSeconds(50);
        character = attack.getCharacter();
        //enemies[0] = attack.getEnemy();
        playerName = "Harry";
        playerMaxHP = character.getHP();
        //enemyHP = enemies[0].getHP();
    }

    void checkBattleOver()
    {
        if (playerHP <= 0)
            stateQueue.Add(BattleScreenStates.FightStates.LOSE);
        if (enemyHP <= 0 && !enemies[0].getDead())
        {
            enemies[0].setDead(true);
            activeTime.disable();
            checkIfAllAreDead();
           // transform.GetComponentInParent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdRemoveEnemy(infoDump.gameObject);
        }
        if (numEnemies >= 2 && enemy2HP <= 0 && !enemies[1].getDead())
        {
            enemies[1].setDead(true);
            enemy2ActiveTime.disable();
            checkIfAllAreDead();
           // transform.GetComponentInParent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdRemoveEnemy(infoDump.gameObject);

        }
        if (numEnemies == 3 && enemy3HP <= 0 && !enemies[2].getDead())
        {
            enemies[2].setDead(true);
            enemy3ActiveTime.disable();
            checkIfAllAreDead();
          //  transform.GetComponentInParent<BattleHolderScript>().player.GetComponent<PlayerMovement>().CmdRemoveEnemy(infoDump.gameObject);

        }

    }

    void checkIfAllAreDead()
    {
        if (enemies == null)
            return;

        switch (numEnemies)
        {
            case 1:
                if (enemies[0].getDead() == true)
                    stateQueue.Add(BattleScreenStates.FightStates.WIN);
                break;
            case 2:
                if (enemies[0].getDead() == true && enemies[1].getDead() == true)
                    stateQueue.Add(BattleScreenStates.FightStates.WIN);
                break;
            case 3:
                if (enemies[0].getDead() == true && enemies[1].getDead() == true && enemies[2].getDead() == true)
                    stateQueue.Add(BattleScreenStates.FightStates.WIN);
                break;
        }
    }

    void stateCheck()
    {
        if (activeTime == null)
            return;
        if (activeTime.GetEnemyRatio() == 1)
        {
            print("Enemy attacking");
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            activeTime.setEnemySeconds(0);
            if (infoDump != null)
                sendAttackFlag(true); // Need Command to do this
        }

        if(numEnemies == 2 && enemy2ActiveTime.GetEnemyRatio() == 1)
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            enemy2ActiveTime.setEnemySeconds(5);
            if (infoDump != null)
                sendAttackFlag(true); // Need Command to do this
        }
        if (numEnemies == 3 && enemy3ActiveTime.GetEnemyRatio() == 1)
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            enemy3ActiveTime.setEnemySeconds(10);
            if (infoDump != null)
                sendAttackFlag(true); // Need Command to do this
        }

        if (currentMoveSelected && numEnemies > 1 && state.curState == BattleScreenStates.FightStates.NEUTRAL && !playerAttackFlag)
        {
            stateQueue.Add(BattleScreenStates.FightStates.PICKANENEMY);
            currentMoveSelected = false;
            toggleState();
        }
        else if (state.curState == BattleScreenStates.FightStates.PICKANENEMY)
        {
            if (Input.GetKeyDown("space"))
            {
                toggleState();
                currentMoveSelected = true;
                playerAttackFlag = true;
            }
        }
        else if (activeTime.GetRatio() == 1 && currentMoveSelected)
        {
            stateQueue.Add(BattleScreenStates.FightStates.PLAYERTURN);
            activeTime.setSeconds(0);
            currentMoveSelected = false;
            playerAttackFlag = true;
            toggleState();
        }
        stateTakesEffect();
    }

    void toggleState()
    {
        if (stateQueue.Count == 1)
            stateQueue.Add(BattleScreenStates.FightStates.NEUTRAL);
        stateQueue.RemoveAt(0);
        state.curState = stateQueue[0];
    }

    void stateTakesEffect()
    {
        if (state.curState == BattleScreenStates.FightStates.PLAYERTURN && playerAttackFlag == true)
        {

            if ((whichSkill >= 0) && character.getSkills()[whichSkill].getType() == "heal")
                sendPlayerDamage(-attack.giveDamage(whichSkill)); // Replace with Command

            //WHICH ENEMY GETS HURT?
            else if (selection.getArrowPos() == 0)
                sendEnemyDamage(0, attack.giveDamage(whichSkill), 0);
            else if (selection.getArrowPos() == 1)
                sendEnemyDamage(attack.giveDamage(whichSkill), 0, 0);
            else if (selection.getArrowPos() == 2)
                sendEnemyDamage(0, 0, attack.giveDamage(whichSkill));

            playerAttackFlag = false;
            currentMoveSelected = false;
            sendFightMessage(attack.getFightMessage());
            whichSkill = -1;

            checkBattleOver();

        }

        if (state.curState == BattleScreenStates.FightStates.ENEMYTURN && enemyAttackFlag == true)
        {

            // Check Target. Random for now. Refer to BattleAttackHandler.cs
            if(attack.getTarget() == playerNum)
                sendPlayerDamage(attack.enemyAttacks()); // Replace with Command
            else
                print(attack.enemyAttacks());

            if (infoDump != null)
                sendAttackFlag(false); // Need Command to do this
            sendFightMessage(attack.getFightMessage());

            checkBattleOver();

        }
        if (state.curState == BattleScreenStates.FightStates.LOSE)
        {
            sendFightMessage(playerName + " fainted. Try again."); // Update from Command
            // When lose, just send back to lobby.
            if(!transform.GetComponentInParent<BattleHolderScript>().isServer) // Make it so if Server dies, game goes on. Tough crap server player, get gid scrub. Or we could be nice.
                SceneManager.LoadScene("Menu");
            else if(Network.connections.Length <= 1)
                SceneManager.LoadScene("Menu");

        }
        if (state.curState == BattleScreenStates.FightStates.WIN)
        {
            print("Battle Win");
            sendFightMessage("Enemy was defeated! " + playerName + " wins!"); // Update from Command
            // On win, call die in battle Holder. Will handle showing player again and all.
            transform.GetComponentInParent<BattleHolderScript>().die();
        }
        if (state.curState == BattleScreenStates.FightStates.SECONDENEMYJOINS)
        {

		sendFightMessage("Good grief! A new enemy joins in");
		// Update message and num enemies with Command
        }
        if (state.curState == BattleScreenStates.FightStates.THIRDENEMYJOINS)
        {
		sendFightMessage("Just my luck! It's another enemy!");
		// Update message and possibly num enemies with Command.
        }
        if (state.curState == BattleScreenStates.FightStates.FRIENDJOINS)
        {
            print("FriendJoins");
            sendFightMessage("Good news! Help has arrived!"); // Replace with command if want network wide message
        }
        if (state.curState == BattleScreenStates.FightStates.PICKANENEMY)
            sendFightMessage("Select a target.");
    }

    void moreEnemies()
    {
        if (numEnemies == 2)
        {
            enemies[1] = new Monster(5, 1, 1, 1, 1, false, 1, 1, 1);
            enemy2HP = enemies[1].getHP();
            stateQueue.Add(BattleScreenStates.FightStates.SECONDENEMYJOINS);
            enemy2ActiveTime = transform.FindChild("EnemyPanel/Enemy2").GetComponent<ActiveTime>();
            enemy2ActiveTime.setEnemyMaxTime(20);
        }
        else if (numEnemies == 3)
        {
            enemies[2] = new Monster(5, 1, 1, 1, 1, false, 1, 1, 1);
            enemy3HP = enemies[2].getHP();
            stateQueue.Add(BattleScreenStates.FightStates.THIRDENEMYJOINS);
            enemy3ActiveTime = transform.FindChild("EnemyPanel/Enemy3").GetComponent<ActiveTime>();
            enemy3ActiveTime.setEnemyMaxTime(25);
        }
    }

    void morePlayers()
    {
        stateQueue.Add(BattleScreenStates.FightStates.FRIENDJOINS);
    }

    public float getPlayerHP()
    {
        return playerHP;
    }

    public float getPlayerMaxHP()
    {
        return playerMaxHP;
    }

    public string getFightMessage()
    {
        return fightMessage;
    }

    public Characters getCharacter()
    {
        return character;
    }

    public int getNumEnemies()
    {
        return numEnemies;
    }

    public Monster[] getEnemies()
    {
        return enemies;
    }

    public int getNumPlayers()
    {
        return numPlayers;
    }

}