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
    private Enemy enemy1;
    private Enemy enemy2;
    private Enemy enemy3;

    private string fightMessage;

    private float buffMultiplier = 1;
    private float enemyBuffMultiplier = 1;
    private int playerMeleeDamage = 9;

    private EnemyQuantity enemyQuantity;
    private BattleScreenStates state;
    private DatabaseBattle db;
    private ArrowSelection selection;
    private BattleLogic logic;
    private ActiveTime activeTime;
    Skill[] skills = new Skill[8];

    void Start()
    {
        character = new Characters("okay", 10, 10, 10, 10, 10);
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
        //skills = character.getSkills();
    }


    // For when the player's HP is being affected.
	public int enemyAttacks(int enemySkill)
    {
		print(enemySkill);
		int damageDone = (int)logic.getEnemyHP () / 2;  //get the max hp of enemy that the player is fighting, this will be the regular skill / attack.
		fightMessage = "You are attacked! -5 HP";

		if(enemySkill >= 0)
		{
			damageDone = skills[enemySkill].getValue();
			fightMessage = "You cast " + skills[enemySkill].getName() + "! It does " + damageDone + " HP!";
		}
		return damageDone;
    }


    // For when the enemy's HP is being affected.
    public int giveDamage(int whichSkill)
    {

        print(whichSkill);

        int damageDone = (int)(playerMeleeDamage * buffMultiplier);

        fightMessage = "You attack " + selection.getArrowPos() + "! It does " + damageDone + " HP";
        if(whichSkill >= 0)
        {
            damageDone = skills[whichSkill].getValue();
            fightMessage = "You cast " + skills[whichSkill].getName() + "! It does " + damageDone + " HP!";
        }
        return damageDone;

    }

    public Characters getCharacter()
    {
        return character;
    }

    public string getFightMessage()
    {
        return fightMessage;
    }
}
