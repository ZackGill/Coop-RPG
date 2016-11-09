using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class BattleLogic : MonoBehaviour {
    public int numEnemies = 0;
    // Flags so we don't attack more than once per turn.
    private bool playerAttackFlag = false;
    private bool enemyAttackFlag = false;
    // Information about the player.
    private string playerName;
    private float playerHP;
    private float playerSpeed;
    private float playerExperience;
    private int playerLevel;

    // Information about the enemy.
	private Enemy e;
    private string enemyName;
    private float enemyHP;
    private float enemySpeed;
    private float enemyExperienceHeld;

    // Information about the battle itself.
    private string fightMessage;
    // So we can affect the state and timer when necessary.
    private ArrowSelection selection;
    private BattleScreenStates state;
    private ActiveTime activeTime;
    private EnemyQuantity enemyQuantity;
    List<BattleScreenStates.FightStates> stateQueue;

    void Start () {

        playerName = "Harry";
        playerHP = 100;
        enemyName = "Squawk-topus";
        enemyHP = 30;

        state = GetComponent<BattleScreenStates>();
        selection = GetComponent<ArrowSelection>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        enemyQuantity = GetComponent<EnemyQuantity>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        fightMessage = enemyName + " slithers hither!";
    }
	
	void Update () {
        print(state.curState);
        checkBattleOver();
        stateCheck();
        if (Input.GetKeyDown("space"))
            toggleState();
        // FOR TESTING TODO: REMOVE
        if (Input.GetKeyDown(KeyCode.J))
        {
            enemyQuantity.addAnEnemy();
            moreEnemies();
            toggleState();
        }
    }

    public void meleeAttack()
    {
        enemyHP -= 10 * buffMultiplier;
        fightMessage = "You attack " + selection.getArrowPos() +"! 10 HP";
    }

    public void skillUsed()
    {
        //enemyHP -= 15; // old
		e.SetHPCurrent(e.GetHPCurrent() - 15);
        activeTime.setSeconds(0);
        fightMessage = "You attack! 15 HP";
    }

    public void enemyAttacks()
    {
		p.SetHPCurrent(p.GetHPCurrent() - 25*e.GetAttack());
        //playerHP -= 25 * enemyBuffMultiplier; //old
        activeTime.setEnemySeconds(0);
        fightMessage = "Enemy Attacks! 25 HP";
    }

    void checkBattleOver()
    {
		if(p.GetHPCurrent() <= 0)
        {
            state.curState = BattleScreenStates.FightStates.LOSE;
			fightMessage = p.GetEntityName() + " fainted. Try again.";
            Invoke("LoseBattle", 5f);
        }
        if(currentMoveSelected && enemyQuantity.getNumberOfEnemies() > 1 && state.curState == BattleScreenStates.FightStates.NEUTRAL && !playerAttackFlag)
        {
            stateQueue.Add(BattleScreenStates.FightStates.PICKANENEMY);
            currentMoveSelected = false;
            toggleState();
        }
        else if(state.curState == BattleScreenStates.FightStates.PICKANENEMY)
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
            state.curState = BattleScreenStates.FightStates.WIN;
			fightMessage = e.GetEntityName() + " was defeated! " + p.GetEntityName() + " wins!";
            Invoke("WinBattle", 5f);
        }
    }

    void LoseBattle()
    {

    void stateTakesEffect()
    {
        if (state.curState == BattleScreenStates.FightStates.PLAYERTURN && playerAttackFlag == true)
        {
            meleeAttack();
            playerAttackFlag = false;
        }
        if (state.curState == BattleScreenStates.FightStates.ENEMYTURN && enemyAttackFlag == true)
        {
            enemyAttacks();
            enemyAttackFlag = false;
        }
        if(state.curState == BattleScreenStates.FightStates.LOSE)
            fightMessage = playerName + " fainted. Try again.";
        if (state.curState == BattleScreenStates.FightStates.WIN)
            fightMessage = enemyName + " was defeated! " + playerName + " wins!";
        if (state.curState == BattleScreenStates.FightStates.SECONDENEMYJOINS)
        {
            fightMessage = "Good grief! A " + enemyName + " joins in!";
            numEnemies = 2;
        }
        if (state.curState == BattleScreenStates.FightStates.THIRDENEMYJOINS)
        {
            fightMessage = "Just my luck! it's a " + enemyName + "!";
            numEnemies = 3;
        }
        if (state.curState == BattleScreenStates.FightStates.PICKANENEMY)
            fightMessage = "Select a target.";
    }

    void moreEnemies()
    {
        if (enemyQuantity.getNumberOfEnemies() == numEnemies)
            return;
        else if (enemyQuantity.getNumberOfEnemies() == 2)
        {
            stateQueue.Add(BattleScreenStates.FightStates.SECONDENEMYJOINS);
        }
        else if (enemyQuantity.getNumberOfEnemies() == 3)
        {
            stateQueue.Add(BattleScreenStates.FightStates.THIRDENEMYJOINS);
        }
    }

    // Leave battle
    void WinBattle()
    {
        // Renable player components. 
        GameObject temp = transform.parent.GetComponent<BattleHolderScript>().player;
        temp.GetComponent<PlayerMovement>().CmdPlayerToggle(true, null, temp);
        temp.GetComponent<PlayerMovement>().inBattle = false;
        Destroy(transform.parent.gameObject);
    }


	public int GetPlayerHP() {
		return p.GetHPCurrent();
	}

	public int GetPlayerHPMax() {
		return p.GetHPMax();
	}

	public int GetEnemyHP() {
		return e.GetHPCurrent();
	}

	public int GetEnemyHPMax() {
		return e.GetHPMax();
	}

    public string getFightMessage()
    {
        return fightMessage;
    }
}
