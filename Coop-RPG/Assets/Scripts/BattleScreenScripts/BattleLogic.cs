using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
// This is kind of the logic behind the GUI and the states that influence it. The logic for the damage and moves being done is in 
// BattleAttackHandler.
public class BattleLogic : MonoBehaviour
{
    // Many of the following variables were needed for testing pre-firebase and should be removed.
    public int numEnemies = 0;
    public int whichSkill = -1;
    // Flags so we don't attack more than once per turn.
    private bool playerAttackFlag = false;
    private bool enemyAttackFlag = false;
    // Information about the player.
    private string playerName;
    private float playerHP;
    private float playerExperience;
    private Skill[] playerSkills;
    private int playerLevel;
	private Player[] players;
    public bool currentMoveSelected = false;
    // Information about the enemy.
    private string enemyName;
    private float enemyHP;
    private float enemy2HP;
    private float enemy3HP;
	private Enemy[] enemies;
    // Information about the battle itself.
    private string fightMessage;
    // So we can affect the state and timer when necessary.
    private ArrowSelection selection;
    private Characters character;
    private BattleScreenStates state;
    private ActiveTime activeTime;
    private ActiveTime enemy2ActiveTime;
    private ActiveTime enemy3ActiveTime;
    private EnemyQuantity enemyQuantity;
    private BattleAttackHandler attack;
    List<BattleScreenStates.FightStates> stateQueue;

    void Start()
    {
        attack = GetComponent<BattleAttackHandler>();
        character = GetComponent<Characters>();
        state = GetComponent<BattleScreenStates>();
        selection = GetComponent<ArrowSelection>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        enemyQuantity = GetComponent<EnemyQuantity>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
		enemies = new Enemy[3];
		players = new Player[3];
		/*enemyName = "Squawk-topus";
        playerName = "Harry";
        playerHP = 97;
        enemyHP = 30;*/
		enemies [0].SetEntityName ("Squawk-topus");
		enemies [0].InitHP (30);
		players [0].SetEntityName ("Harry");
		players [0].InitHP (97);
		fightMessage = enemies[0].GetEntityName() + " slithers hither!";
        StartCoroutine(updateCharacter());
    }

    void Update()
    {
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
			lessEnemies();
            toggleState();
        }
    }

    IEnumerator updateCharacter()
    {
        yield return new WaitForSeconds(50);
        character = attack.getCharacter();
        //playerName = "Harry";
        //playerHP = character.getHP();
		players[0].SetHPCurrent(character.getHP());
        playerSkills = character.getSkills();
    }

    void checkBattleOver()
    {
		bool deadplayers = true;
		bool deadenemies = true;
		foreach(Player p in players){
			if (p.GetHPCurrent () > 0) {
				deadplayers = false;
			}
		}
		foreach (Enemy e in enemies) {
			if (e.GetHPCurrent () > 0) {
				deadenemies = false;
			}
		}
        if (deadplayers)
            stateQueue.Add(BattleScreenStates.FightStates.LOSE);
		else if(deadenemies)
			stateQueue.Add(BattleScreenStates.FightStates.WIN);
    }

    void stateCheck()
    {
        if (activeTime.GetEnemyRatio() == 1 || 
            (enemy2ActiveTime != null && enemy2ActiveTime.GetEnemyRatio() == 1) ||
            (enemy3ActiveTime != null && enemy3ActiveTime.GetEnemyRatio() == 1))
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            activeTime.setEnemySeconds(0);
            enemy2ActiveTime.setEnemySeconds(0);
            enemy3ActiveTime.setEnemySeconds(0);
            enemyAttackFlag = true;
        }
        if (currentMoveSelected && enemyQuantity.getNumberOfEnemies() > 1 && state.curState == BattleScreenStates.FightStates.NEUTRAL && !playerAttackFlag)
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
            //if (character.getSkills()[whichSkill].getType() == "heal")
            //    playerHP += attack.giveDamage(whichSkill);

            // WHICH ENEMY GETS HURT?
            if(selection.getArrowPos() == 0)
				enemies[0].DamageEntity(attack.giveDamage(whichSkill));
            if (selection.getArrowPos() == 1)
				enemies[1].DamageEntity(attack.giveDamage(whichSkill));
            if (selection.getArrowPos() == 2)
				enemies[2].DamageEntity(attack.giveDamage(whichSkill));

            fightMessage = attack.getFightMessage();
            playerAttackFlag = false;
            whichSkill = -1;
        }
        if (state.curState == BattleScreenStates.FightStates.ENEMYTURN && enemyAttackFlag == true)
        {
			players[0].DamageEntity(attack.enemyAttacks());
            fightMessage = attack.getFightMessage();
            enemyAttackFlag = false;
        }
        if (state.curState == BattleScreenStates.FightStates.LOSE)
			fightMessage = players[0].GetEntityName() + " fainted. Try again.";
        if (state.curState == BattleScreenStates.FightStates.WIN)
			fightMessage = enemies[0].GetEntityName() + " was defeated! " + players[0].GetEntityName() + " wins!";
        if (state.curState == BattleScreenStates.FightStates.SECONDENEMYJOINS)
        {
			fightMessage = "Good grief! A " + enemies[1].GetEntityName() + " joins in!";
            numEnemies = 2;
        }
        if (state.curState == BattleScreenStates.FightStates.THIRDENEMYJOINS)
        {
			fightMessage = "Just my luck! it's a " + enemies[2].GetEntityName() + "!";
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
            //enemy2HP = 30;
			enemies[1].InitHP(30);
			enemies [1].SetEntityName ("Sqwak-topus2");
            enemy2ActiveTime = transform.FindChild("EnemyPanel/Enemy2").GetComponent<ActiveTime>();
        }
        else if (enemyQuantity.getNumberOfEnemies() == 3)
        {
            stateQueue.Add(BattleScreenStates.FightStates.THIRDENEMYJOINS);
            //enemy3HP = 30;
			enemies[2].InitHP(30);
			enemies [2].SetEntityName ("Sqwak-topus3");
            enemy3ActiveTime = transform.FindChild("EnemyPanel/Enemy3").GetComponent<ActiveTime>();
        }
    }
		
	void lessEnemies() {
		if (enemyQuantity.getNumberOfEnemies() == numEnemies)
			return;
		else {
			foreach (Enemy e in enemies) {
				if (e.GetHPCurrent() == 0) {
					e.SetEntityName ("dead"); //error if set e to null possibly want 'new Enemy();'
				}
			}
			// this will change the order in the enemy lineup
			// however i think this will break everything if put in now
			/*if (enemies [0] == null) {
				if (enemies [2] != null) {
					enemies [0] = enemies [2];
				} else if (enemies [1] != null) {
					enemies [0] = enemies [1];
					enemies [1] = null;
				}
			} else if (enemies [1] == null && enemies [2] != null) {
				enemies [1] = enemies [2];
				enemies [2] = null;
			}*/
		}
	}

    public float getPlayerHP()
    {
        return playerHP;
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

    public Characters getCharacter()
    {
        return character;
    }
}