using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.Networking;
// This is kind of the logic behind the GUI and the states that influence it. The logic for the damage and moves being done is in 
// BattleAttackHandler.
public class BattleLogic : NetworkBehaviour
{
    // Networking stuff. Used to synch values properly.
    public OverworldBattle infoDump;
    public int playerNum; // Is this player 0, 1, or 2 in a battle?

    public delegate void PlayerDamageDelegate(float amount, int playerNum);
    public delegate void EnemyDamageDelegate(float amount0, float amount1, float amount2);

    [SyncEvent]
    public event PlayerDamageDelegate EventPlayerDamage = delegate { };
    [SyncEvent]
    public event EnemyDamageDelegate EventEnemyDamage = delegate { };



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
    List<BattleScreenStates.FightStates> stateQueue;

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

       // infoDump.CmdPlayerDamage(10, 0);
      //  infoDump.CmdEnemyDamage(5, 0, 0);
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

    void Start()
    {
        attack = GetComponent<BattleAttackHandler>();
        //character = GetComponent<Characters>(); // Replace with pulling from a holder gameobject
        state = GetComponent<BattleScreenStates>();
        selection = GetComponent<ArrowSelection>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
       

	enemies = new Monster[3];

	playerMaxHP = 100;
	playerHP = 97;


        enemies[0] = new Monster(5, 1, 1, 1, 1, false, 1, 1, 1);
	enemyHP = enemies[0].getHP();

	fightMessage = "Squaktopus silthers hither";
        StartCoroutine(updateCharacter());

        numEnemies = 1;
	numPlayers = 1;
       


        //Invoke("CmdTest", 5f);
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
            if(infoDump != null)
            infoDump.info.fightMessage = fightMessage; // Need to call command for this.
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
        if (enemyHP <= 0)
        {
            enemies[0].setDead(true);
            activeTime.disable();
            checkIfAllAreDead();
        }
        if (numEnemies >= 2 && enemy2HP <= 0)
        {
            enemies[1].setDead(true);
            enemy2ActiveTime.disable();
            checkIfAllAreDead();
        }
        if (numEnemies == 3 && enemy3HP <= 0)
        {
            enemies[2].setDead(true);
            enemy3ActiveTime.disable();
            checkIfAllAreDead();
        }

    }

    void checkIfAllAreDead()
    {
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
        if (activeTime.GetEnemyRatio() == 1)
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            activeTime.setEnemySeconds(0);
            enemyAttackFlag = true;
            if (infoDump != null) 
             infoDump.info.enemyAttackFlag = true; // Need Command to do this
        }

        if(numEnemies == 2 && enemy2ActiveTime.GetEnemyRatio() == 1)
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            enemy2ActiveTime.setEnemySeconds(5);
            enemyAttackFlag = true;
        }
        if (numEnemies == 3 && enemy3ActiveTime.GetEnemyRatio() == 1)
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            enemy3ActiveTime.setEnemySeconds(10);
            enemyAttackFlag = true;
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
                playerHP += attack.giveDamage(whichSkill); // Replace with Command

            //WHICH ENEMY GETS HURT?
            else if(selection.getArrowPos() == 0)
                enemy2HP -= attack.giveDamage(whichSkill); // Replace with Command
            else if (selection.getArrowPos() == 1)
                enemyHP -= attack.giveDamage(whichSkill);	// Replace with Command
            else if (selection.getArrowPos() == 2)
                enemy3HP -= attack.giveDamage(whichSkill);	// Replace with Command

            playerAttackFlag = false;
            currentMoveSelected = false;
            fightMessage = attack.getFightMessage();
            whichSkill = -1;
        }

        if (state.curState == BattleScreenStates.FightStates.ENEMYTURN && enemyAttackFlag == true)
        {

            // Check Target. Random for now. Refer to BattleAttackHandler.cs
            if(attack.getTarget() == 0)
                playerHP -= attack.enemyAttacks(); // Replace with Command
            else
                print(attack.enemyAttacks());

            enemyAttackFlag = false; // Update dump with Command
            fightMessage = attack.getFightMessage(); // Update from Command if wanted same message network wide.
        }
        if (state.curState == BattleScreenStates.FightStates.LOSE)
            fightMessage = playerName + " fainted. Try again."; // Update from Command
        if (state.curState == BattleScreenStates.FightStates.WIN)
            fightMessage = "Enemy was defeated! " + playerName + " wins!"; // Update from Command
        if (state.curState == BattleScreenStates.FightStates.SECONDENEMYJOINS)
        {

		fightMessage = "Good grief! A new enemy joins in";
		// Update message and num enemies with Command
        }
        if (state.curState == BattleScreenStates.FightStates.THIRDENEMYJOINS)
        {
		fightMessage = "Just my luck! It's another enemy!";
		// Update message and possibly num enemies with Command.
        }
        if (state.curState == BattleScreenStates.FightStates.FRIENDJOINS)
        {
            fightMessage = "Good news! Help has arrived!"; // Replace with command if want network wide message
        }
        if (state.curState == BattleScreenStates.FightStates.PICKANENEMY)
            fightMessage = "Select a target.";
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