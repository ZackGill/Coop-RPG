using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private float playerSpeed;
    private float playerExperience;
    private Skill[] playerSkills;
    private int playerLevel;
    public bool currentMoveSelected = false;
    // Information about the enemy.
    private string enemyName;
    private float enemyHP;
    private float enemySpeed;
    private float enemyExperienceHeld;
    // Information about the battle itself.
    private string fightMessage;
    // So we can affect the state and timer when necessary.
    private ArrowSelection selection;
    private Characters character;
    private BattleScreenStates state;
    private ActiveTime activeTime;
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
        enemyName = "Squawk-topus";
        playerName = "Harry";
        playerHP = 1;
        enemyHP = 30;
        fightMessage = enemyName + " slithers hither!";
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
            toggleState();
        }
    }

    IEnumerator updateCharacter()
    {
        yield return new WaitForSeconds(50);
        character = attack.getCharacter();
        playerName = "Harry";
        playerHP = character.getHP();
        playerSkills = character.getSkills();
    }

    void checkBattleOver()
    {
        if (playerHP <= 0)
            stateQueue.Add(BattleScreenStates.FightStates.LOSE);
        else if (enemyHP <= 0)
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
            if (character.getSkills()[whichSkill].getType() == "heal")
                playerHP += attack.giveDamage(whichSkill);
            enemyHP -= attack.giveDamage(whichSkill);
            fightMessage = attack.getFightMessage();
            playerAttackFlag = false;
            whichSkill = -1;
        }
        if (state.curState == BattleScreenStates.FightStates.ENEMYTURN && enemyAttackFlag == true)
        {
            playerHP -= attack.enemyAttacks();
            fightMessage = attack.getFightMessage();
            enemyAttackFlag = false;
        }
        if (state.curState == BattleScreenStates.FightStates.LOSE)
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

    public Characters getCharacter()
    {
        return character;
    }
}