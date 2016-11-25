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
    public bool currentMoveSelected = false;
    // Information about the enemy.
    private string enemyName;
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
    private EnemyQuantity enemyQuantity;
    private BattleAttackHandler attack;
    List<BattleScreenStates.FightStates> stateQueue;

    // Used by Zack to test Mulitplayer synching. Using this so don't have to try skills and all,
    // since their use isn't completed in the battle code by Lex and Bob, don't want to spend hours porting it
    // to the prefab and solving merge issues if I'm just going to have to do it again. 
    [Command]
    public void CmdTest()
    {
        if (infoDump == null)
            return;
        infoDump.CmdPlayerDamage(10, 0);
        infoDump.CmdEnemyDamage(5, 0, 0);
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
        //character = GetComponent<Characters>();
        state = GetComponent<BattleScreenStates>();
        selection = GetComponent<ArrowSelection>();
        stateQueue = new List<BattleScreenStates.FightStates>();
        enemyQuantity = GetComponent<EnemyQuantity>();
        stateQueue.Add(BattleScreenStates.FightStates.BEGINNING);
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
        enemyName = "Squawk-topus";
        playerName = "Harry";
        playerHP = 97;


        enemyHP = 30;
        fightMessage = enemyName + " slithers hither!";
        StartCoroutine(updateCharacter());

        Invoke("CmdTest", 5f);
    }

    void Update() {
        if (playerNum >= 0 || playerNum < 3)
        {
            setEnemyHP();
            setPlayerHP();
            if(infoDump != null)
            infoDump.info.fightMessage = fightMessage;
        }
       // print(state.curState);
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
        if (activeTime.GetEnemyRatio() == 1 || 
            (enemy2ActiveTime != null && enemy2ActiveTime.GetEnemyRatio() == 1) ||
            (enemy3ActiveTime != null && enemy3ActiveTime.GetEnemyRatio() == 1))
        {
            stateQueue.Add(BattleScreenStates.FightStates.ENEMYTURN);
            activeTime.setEnemySeconds(0);
            enemy2ActiveTime.setEnemySeconds(0);
            enemy3ActiveTime.setEnemySeconds(0);
            enemyAttackFlag = true;
            if (infoDump != null) ;
            infoDump.info.enemyAttackFlag = true;
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
            if (selection.getArrowPos() == 0)
            {
                if (EventEnemyDamage != null)
                    EventEnemyDamage(0, attack.giveDamage(whichSkill), 0);
            }
            if (selection.getArrowPos() == 1)
            {
                if(EventEnemyDamage != null)
                EventEnemyDamage(attack.giveDamage(whichSkill), 0, 0);
            }
            if (selection.getArrowPos() == 2)
            {
                if(EventEnemyDamage != null)
                EventEnemyDamage(0, 0, attack.giveDamage(whichSkill));

            }
            fightMessage = attack.getFightMessage();
            if(infoDump != null)
            infoDump.info.fightMessage = fightMessage;
            playerAttackFlag = false;
            whichSkill = -1;
        }
        if (state.curState == BattleScreenStates.FightStates.ENEMYTURN && enemyAttackFlag == true)
        {
            if(EventPlayerDamage != null)
                EventPlayerDamage(attack.enemyAttacks(), playerNum);
            fightMessage = attack.getFightMessage();
            if(infoDump != null)
            infoDump.info.fightMessage = fightMessage;

            enemyAttackFlag = false;
            if(infoDump != null)
            infoDump.info.enemyAttackFlag = false;
        }
        if (state.curState == BattleScreenStates.FightStates.LOSE)
            fightMessage = playerName + " fainted. Try again.";
        if (state.curState == BattleScreenStates.FightStates.WIN)
            fightMessage = enemyName + " was defeated! " + playerName + " wins!";
        if (state.curState == BattleScreenStates.FightStates.SECONDENEMYJOINS)
        {
            fightMessage = "Good grief! A " + enemyName + " joins in!";
            numEnemies = 2;
            if (infoDump != null)
            {
                infoDump.info.numEnemies = 2;
                infoDump.info.fightMessage = fightMessage;
            }
        }
        if (state.curState == BattleScreenStates.FightStates.THIRDENEMYJOINS)
        {
            fightMessage = "Just my luck! it's a " + enemyName + "!";
            numEnemies = 3;
            if (infoDump != null)
            {
                infoDump.info.numEnemies = 3;
                infoDump.info.fightMessage = fightMessage;
            }

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
            enemy2HP = 30;
            enemy2ActiveTime = transform.FindChild("EnemyPanel/Enemy2").GetComponent<ActiveTime>();
        }
        else if (enemyQuantity.getNumberOfEnemies() == 3)
        {
            stateQueue.Add(BattleScreenStates.FightStates.THIRDENEMYJOINS);
            enemy3HP = 30;
            enemy2ActiveTime = transform.FindChild("EnemyPanel/Enemy3").GetComponent<ActiveTime>();
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