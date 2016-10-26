using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class BattleLogic : MonoBehaviour {

	// Information about the player
	//private Player p;

    private string playerName;
    private float playerHP;
    private float playerSpeed;
    private float playerExperience;
    private int playerLevel;
    public bool currentMoveSelected = false;
    private float buffMultiplier = 1;
    // Information about the enemy.
	//private Enemy e;
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
    List<BattleScreenStates.FightStates> stateQueue;

    void Start () {
		//initialize player here
        playerName = "Harry";
        playerHP = 100;
		//initialize enemy here
        enemyName = "Spookeroni";
        enemyHP = 30;

        state = GetComponent<BattleScreenStates>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        fightMessage = enemyName + " draws near!";
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
		//e.damage(10*p.GetAttack());
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
		//p.damage(25*e.GetAttack());
        playerHP -= 25 * enemyBuffMultiplier;
        fightMessage = "Enemy Attacks! 25 HP";
    }

    void checkBattleOver()
    {

	if (playerHP <= 0) //if(p.GetHpCurr <= 0)
 		stateQueue.Add(BattleScreenStates.FightStates.LOSE);
        else if(enemyHP <= 0)
            stateQueue.Add(BattleScreenStates.FightStates.WIN);
     }
	void stateCheck()
	{
		if (activeTime.GetEnemyRatio() == 1){
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
        {
            fightMessage = enemyName + " was defeated! " + playerName + " wins!";
            SceneManager.LoadScene("genDungeon");
        }
    }

	// these methods will be pulled from the player/enemy classes
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
