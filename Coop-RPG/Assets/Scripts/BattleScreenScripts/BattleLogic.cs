using UnityEngine;
using System.Collections;

public class BattleLogic : MonoBehaviour {

    // Information about the player
	//List<Player> players = new List<Player>();
	//List<Enemy> enemies = new List<Enemy>();
	private Player p;
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
    private string playerFightMessage;
    private string enemyFightMessage;

    // So we can affect the state and timer when necessary.
    private BattleScreenStates state;
    private ActiveTime activeTime;

    void Start () {
		// initialize
		p = new Player();
		//initialize enemy here
		e = new Enemy();
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
		e.SetHPCurrent(e.GetHPCurrent() - 10*p.GetAttack());
        //e.hpCurrent -= 10 * p.attack; // old
        activeTime.setSeconds(0);
        playerFightMessage = "You attack! 10 HP";
    }

    public void skillUsed()
    {
        //enemyHP -= 15; // old
		e.SetHPCurrent(e.GetHPCurrent() - 15);
        activeTime.setSeconds(0);
        playerFightMessage = "You attack! 15 HP";
    }

    public void enemyAttacks()
    {
		p.SetHPCurrent(p.GetHPCurrent() - 25*e.GetAttack());
        //playerHP -= 25 * enemyBuffMultiplier; //old
        activeTime.setEnemySeconds(0);
        enemyFightMessage = "Enemy Attacks! 25 HP";
    }

    void checkBattleOver()
    {
		if(p.GetHPCurrent() <= 0)
        {
            state.curState = BattleScreenStates.FightStates.LOSE;
			playerFightMessage = p.GetEntityName() + " fainted. Try again.";
        }
		else if(e.GetHPCurrent() <= 0)
        {
            state.curState = BattleScreenStates.FightStates.WIN;
			playerFightMessage = e.GetEntityName() + " was defeated! " + p.GetEntityName() + " wins!";
        }
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

    public string getPlayerFightMessage()
    {
        return playerFightMessage;
    }

    public string getEnemyFightMessage()
    {
        return enemyFightMessage;
    }
}
