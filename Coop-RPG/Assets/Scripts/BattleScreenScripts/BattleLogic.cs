using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// THIS WILL BE USED AS A MIDDLE GROUND BETWEEN THE USER INTERFACE AND BOB'S BATTLE SYSTEM, AS WELL
// AS INFORMATION ABOUT THE CHARACTER/ENEMIES. Essentially, this is the logic behind the UI.

public class BattleLogic : MonoBehaviour {

    // Information about the player
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
    private string playerFightMessage;
    private string enemyFightMessage;

    // So we can affect the state and timer when necessary.
    private BattleScreenStates state;
    private ActiveTime activeTime;
    List<BattleScreenStates.FightStates> states = new List<BattleScreenStates.FightStates>();

    void Start () {

        playerName = "Harry";
        playerHP = 100;
        enemyName = "Spookeroni";
        enemyHP = 30;

        state = GetComponent<BattleScreenStates>();
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        playerFightMessage = enemyName + " draws near!";
    }
	
	void Update () {
        checkBattleOver();
        if (Input.GetKeyDown("space"))
            toggleState();
    }

    public void meleeAttack()
    {
        enemyHP -= 10 * buffMultiplier;
        activeTime.setSeconds(0);
        playerFightMessage = "You attack! 10 HP";
    }

    public void skillUsed()
    {
        enemyHP -= 15;
        activeTime.setSeconds(0);
        playerFightMessage = "You attack! 15 HP";
    }

    public void enemyAttacks()
    {
        playerHP -= 25 * enemyBuffMultiplier;
        activeTime.setEnemySeconds(0);
        enemyFightMessage = "Enemy Attacks! 25 HP";
    }

    void checkBattleOver()
    {
        if (playerHP <= 0)
        {
            state.curState = BattleScreenStates.FightStates.LOSE;
            playerFightMessage = playerName + " fainted. Try again.";
        }
        else if(enemyHP <= 0)
        {
            state.curState = BattleScreenStates.FightStates.WIN;
            playerFightMessage = enemyName + " was defeated! " + playerName + " wins!";
        }
    }

    void stateCheck()
    {
        if (activeTime.GetEnemyRatio() == 1)
        {
            state.curState = BattleScreenStates.FightStates.ENEMYTURN;
        }
        if (activeTime.GetRatio() == 1 && currentMoveSelected)
        {
            state.curState = BattleScreenStates.FightStates.PLAYERTURN;
        }
        if (!nextState)
            return;
    }

    void toggleState()
    {

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

    public string getPlayerFightMessage()
    {
        return playerFightMessage;
    }

    public string getEnemyFightMessage()
    {
        return enemyFightMessage;
    }
}
