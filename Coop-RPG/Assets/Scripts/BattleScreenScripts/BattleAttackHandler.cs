using UnityEngine;
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
        character = new Characters("okay", 10, 10, 10, 10, 10);
        enemy = new Monster(1, 1, 1, 1, 1, false, 1, 1, 1);
        enemy2 = new Monster(1, 1, 1, 1, 1, false, 1, 1, 1);
        enemy3 = new Monster(1, 1, 1, 1, 1, false, 1, 1, 1);
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

        character = db.getCharacter();

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
        print(whichSkill);

        int damageDone = (int)(playerMeleeDamage * buffMultiplier);

        fightMessage = "You attack " + selection.getArrowPos() + "! It does " + damageDone + " HP";
        if(whichSkill >= 0)
        {
            damageDone = character.getSkills()[whichSkill].getValues();
            fightMessage = "You cast " + character.getSkills()[whichSkill].getName() + "! It does " + damageDone + " HP!";
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
