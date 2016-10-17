using UnityEngine;
using System.Collections;

public class BattleLogic : MonoBehaviour {

    // Information about the player
	//private Player p;
    private string playerName;
    private float playerHP;
    private float playerSpeed;
    private float playerExperience;
    private int playerLevel;
    private float buffMultiplier = 1;

    // Information about the enemy.
	//private Enemy e;
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

    void Start () {
		//initialize player here
        playerName = "Harry";
        playerHP = 100;
		//initialize enemy here
        enemyName = "Spookeroni";
        enemyHP = 30;

        state = GetComponent<BattleScreenStates>();
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        playerFightMessage = enemyName + " draws near!";
    }
	
	void Update () {
        checkBattleOver();
    }

    public void meleeAttack()
    {
		//e.damage(10*p.GetAttack());
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
		//p.damage(25*e.GetAttack());
        playerHP -= 25 * enemyBuffMultiplier;
        activeTime.setEnemySeconds(0);
        enemyFightMessage = "Enemy Attacks! 25 HP";
    }

    void checkBattleOver()
    {
		if (playerHP <= 0) //if(p.GetHpCurr <= 0)
        {
            state.curState = BattleScreenStates.FightStates.LOSE;
            playerFightMessage = playerName + " fainted. Try again.";
        }
		else if(enemyHP <= 0) //if(e.GetHpCurr <= 0)
        {
            state.curState = BattleScreenStates.FightStates.WIN;
            playerFightMessage = enemyName + " was defeated! " + playerName + " wins!";
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

    public string getPlayerFightMessage()
    {
        return playerFightMessage;
    }

    public string getEnemyFightMessage()
    {
        return enemyFightMessage;
    }
}
