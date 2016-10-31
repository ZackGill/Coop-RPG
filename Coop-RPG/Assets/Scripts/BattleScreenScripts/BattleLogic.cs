using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// THIS WILL BE USED AS A MIDDLE GROUND BETWEEN THE USER INTERFACE AND BOB'S BATTLE SYSTEM, AS WELL
// AS INFORMATION ABOUT THE CHARACTER/ENEMIES. Essentially, this is the logic behind the UI.

public class BattleLogic : MonoBehaviour {
    // Flags so we don't attack more than once per turn.
    private bool playerAttackFlag = false;
    private bool enemyAttackFlag = false;
    // Information about the player.
    private string playerName;
    private float playerHP;
    private float playerSpeed;
    private float playerExperience;
    private int playerLevel;
    public bool currentMoveSelected = false;
    private float buffMultiplier = 1;
    // Information about the enemy.
    private string enemyName;
    private float enemyHP;
    private float enemySpeed;
    private float enemyExperienceHeld;
    private float enemyBuffMultiplier = 1;
    // Information about the battle itself.
    private string fightMessage;
    // So we can affect the state and timer when necessary.
    private BattleScreenStates state;
    private ActiveTime activeTime;
    private EnemyQuantity enemies;
    List<BattleScreenStates.FightStates> stateQueue;

    void Start () {

        playerName = "Harry";
        playerHP = 100;
        enemyName = "Squawk-topus";
        enemyHP = 30;

        state = GetComponent<BattleScreenStates>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        fightMessage = enemyName + " slithers hither!";
    }
	
	void Update () {
        //print(state.curState);
        checkBattleOver();
        stateCheck();
        if (Input.GetKeyDown("space"))
            toggleState();
    }

    public void meleeAttack()
    {
        enemyHP -= 10 * buffMultiplier;
        fightMessage = "You attack! 10 HP";
    }

    public void skillUsed()
    {
        enemyHP -= 15;
        fightMessage = "You attack! 15 HP";
    }

    public void enemyAttacks()
    {
        playerHP -= 25 * enemyBuffMultiplier;
        fightMessage = "Enemy Attacks! 25 HP";
    }

    void checkBattleOver()
    {
        if (playerHP <= 0)
            stateQueue.Add(BattleScreenStates.FightStates.LOSE);
        else if(enemyHP <= 0)
            stateQueue.Add(BattleScreenStates.FightStates.WIN);
    }

    void stateCheck()
    {
        if (activeTime.GetEnemyRatio() == 1)
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            activeTime.setEnemySeconds(0);
            enemyAttackFlag = true;
        }
        if (activeTime.GetRatio() == 1 && currentMoveSelected)
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
    }

    void moreEnemies()
    {
        if (enemies.getNumberOfEnemies() == 2)
        {

        }
        else if (enemies.getNumberOfEnemies() == 3)
        {

        }
    }

    public float getPlayerHP()
    {
        return playerHP;
    }

    public float getPlayerMaxHP()
    {
        return 100;
    }

    public float getEnemyHP()
    {
        return enemyHP;
    }

    public float getEnemyMaxHP()
    {
        return 100;
    }

    public string getFightMessage()
    {
        return fightMessage;
    }
}
