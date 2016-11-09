using UnityEngine;
using System.Collections;

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

    void Start()
    {
        character = GetComponent<Characters>();
        db = GetComponent<DatabaseBattle>();
        selection = GetComponent<ArrowSelection>();
        logic = GetComponent<BattleLogic>();
    }

    // TODO: Don't update this EVERY time...
    void Update()
    {
        character = db.getCharacter();
    }

    public int enemyAttacks()
    {
        fightMessage = "You are attacked! -5 HP";
        return 5;
    }

    public int giveDamage(int whichSkill)
    {

        print(whichSkill);

        int damageDone = (int)(playerMeleeDamage * buffMultiplier);

        Skill[] skills = new Skill[8];
        if(character.getSkills().Length > 0)
            skills = character.getSkills();

        fightMessage = "You attack " + selection.getArrowPos() + "! It does " + damageDone + " HP";
        if(whichSkill >= 0)
        {
            damageDone = skills[whichSkill].getValues();
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
