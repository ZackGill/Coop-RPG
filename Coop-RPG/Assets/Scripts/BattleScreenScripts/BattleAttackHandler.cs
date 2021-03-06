﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;

/// <summary>
///  This will be the logic between the fight itself, that is to say the battle, how much damage is being taken, what kind of moves
///  are being done, etc.
/// </summary>
public class BattleAttackHandler : MonoBehaviour
{

    private Characters character;

    private Monster enemy;
    private Monster enemy1;
    private Monster enemy2;
    private Monster enemy3;

    private string fightMessage;

    private float buffMultiplier = 1;
    private float enemyBuffMultiplier = 1;
    private int playerMeleeDamage = 9;

    private int targetPlayer = 0;

    private EnemyQuantity enemyQuantity;
    private BattleScreenStates state;
    private DatabaseBattle db;
    private ArrowSelection selection;
    private BattleLogic logic;
    private ActiveTime activeTime;


    System.Random ran = new System.Random();

    void Start()
    {
        db = GetComponent<DatabaseBattle>();
        selection = GetComponent<ArrowSelection>();
        logic = GetComponent<BattleLogic>();
        activeTime = transform.FindChild("PlayerInfo/ActiveTimeBar").GetComponent<ActiveTime>();
    }

    // TODO: Don't update this EVERY time...
    void Update()
    {

        // Maybe this should be done every time though.
        //activeTime.setMaxTime(200);

        if(character == null)
        {
            if (logic == null) {
                print("Null battle logic in Attack Handler");
                return;
                    }
            if (logic.infoDump == null) {
                print("Null info dump in Attack Handler");
                return;
            }
            if (logic.infoDump.player0 == null) {
                print("Null player0 in Attack handler");
                return;
            }

            switch (logic.playerNum)
            {
                case 0:
                    character = logic.infoDump.player0;
                    break;
                case 1:
                    character = logic.infoDump.player1;
                    break;
                case 2:
                    character = logic.infoDump.player2;
                    break;
            }
        }

        if (enemy == null)
            enemy = logic.infoDump.enemy0;
        if (enemy1 == null)
            enemy1 = logic.infoDump.enemy1;
        if (enemy2 == null)
            enemy2 = logic.infoDump.enemy2;

    }


    // For when the player's HP is being affected.
    public int enemyAttacks()
    {

        fightMessage = "Player " + targetPlayer + " was attacked! -5 HP";
        return 5;
    }


    // For when the enemy's HP is being affected.
    public int giveDamage(int whichSkill)
    {

        int damageDone = (int)(playerMeleeDamage * buffMultiplier);

        fightMessage = character.getName() + " attack " + selection.getArrowPos() + "! It does " + damageDone + " HP";
        if(whichSkill >= 0)
        {
            if(character == null)
            {
                print("Character null in battle attack handler");
                return (int)(playerMeleeDamage * buffMultiplier); ;
            }
            if (character.getSkills() == null)
            {
                print("Character skills null in battle attack handler");
                return (int)(playerMeleeDamage * buffMultiplier); ;
            }

            damageDone = character.getSkills()[whichSkill].getValue();
            fightMessage = character.getName() + " cast " + character.getSkills()[whichSkill].getName() + "! It does " + damageDone + " HP!";
        }
        return damageDone;
    }

    public Characters getCharacter()
    {
        return character;
    }


    public Monster getEnemy()
    {
        return enemy;
    }

    public Monster getEnemy2()
    {
        return enemy2;
    }

    public Monster getEnemy3()
    {
        return enemy3;
    }

    public int getTarget()
    {
        // Select a target player based on the number of players. Random for now.
        if (logic.getNumPlayers() == 1)
            targetPlayer = 0;
        else if (logic.getNumPlayers() == 2)
            targetPlayer = ran.Next(0, 2);
        else if (logic.getNumPlayers() == 3)
            targetPlayer = ran.Next(0, 3);

        return targetPlayer;
    }

    public void setTarget(int target)
    {
        targetPlayer = target;
    }

    public string getFightMessage()
    {
        return fightMessage;
    }
}
