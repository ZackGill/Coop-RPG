using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

// This is kind of the logic behind the GUI and the states that influence it. The logic for the damage and moves being done is in 
// BattleAttackHandler.
public class BattleLogic : MonoBehaviour
{
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

    void Start()
    {
        attack = GetComponent<BattleAttackHandler>();
        character = GetComponent<Characters>();
        state = GetComponent<BattleScreenStates>();
        selection = GetComponent<ArrowSelection>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        enemies = new Monster[3];

        // TEMP STUFF FOR TESTING
        playerMaxHP = 100;
        playerHP = 97;
        enemies[0] = new Monster(5, 1, 1, 1, 1, false, 1, 1, 1);
        enemyHP = enemies[0].getHP();

        fightMessage = "Squawktopus slithers hither!";
        StartCoroutine(updateCharacter());
        numEnemies = 1;
        numPlayers = 1;
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
            numEnemies++;
            if (numEnemies > 3)
                numEnemies = 3;
            moreEnemies();
            toggleState();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            numPlayers++;
            morePlayers();
            toggleState();
        }
    }

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
                playerHP += attack.giveDamage(whichSkill);

            //WHICH ENEMY GETS HURT?
            else if(selection.getArrowPos() == 0)
                enemy2HP -= attack.giveDamage(whichSkill);
            else if (selection.getArrowPos() == 1)
                enemyHP -= attack.giveDamage(whichSkill);
            else if (selection.getArrowPos() == 2)
                enemy3HP -= attack.giveDamage(whichSkill);

            playerAttackFlag = false;
            currentMoveSelected = false;
            fightMessage = attack.getFightMessage();
            whichSkill = -1;
        }

        if (state.curState == BattleScreenStates.FightStates.ENEMYTURN && enemyAttackFlag == true)
        {
            // Check Target. Random for now. Refer to BattleAttackHandler.cs
            if(attack.getTarget() == 0)
                playerHP -= attack.enemyAttacks();
            else
                print(attack.enemyAttacks());

            enemyAttackFlag = false;
            fightMessage = attack.getFightMessage();
        }
        if (state.curState == BattleScreenStates.FightStates.LOSE)
            fightMessage = playerName + " fainted. Try again.";
        if (state.curState == BattleScreenStates.FightStates.WIN)
            fightMessage = "Enemy was defeated! " + playerName + " wins!";
        if (state.curState == BattleScreenStates.FightStates.SECONDENEMYJOINS)
        {
            fightMessage = "Good grief! A new enemy joins in!";
        }
        if (state.curState == BattleScreenStates.FightStates.THIRDENEMYJOINS)
        {
            fightMessage = "Just my luck! it's another enemy!";
        }
        if (state.curState == BattleScreenStates.FightStates.FRIENDJOINS)
        {
            fightMessage = "Good news! Help has arrived!";
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