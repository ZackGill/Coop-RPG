using UnityEngine;
using System.Collections;

public class BattleAttackHandler : MonoBehaviour {

    private Characters character;
    private Enemy enemy1;
    private Enemy enemy2;
    private Enemy enemy3;

    private string fightMessage;
    private float buffMultiplier = 1;
    private float enemyBuffMultiplier = 1;
    private int playerMeleeDamage = 1;

    private EnemyQuantity enemyQuantity;
    private BattleScreenStates state;
    private DatabaseBattle db;
    private ArrowSelection selection;
    private BattleLogic logic;

    void Start () {
        character = GetComponent<Characters>();
        db = GetComponent<DatabaseBattle>();
        selection = GetComponent<ArrowSelection>();
        logic = GetComponent<BattleLogic>();
    }
	
    // TODO: Don't update this EVERY time...
	void Update () {

        character = db.getCharacter();
        playerMeleeDamage = character.getAttack() * 3;
    }

    public Characters getCharacter()
    {
        return character;
    }

    public void enemyAttacks()
    {
        fightMessage = "Enemy Attacks!" + 5 + " HP";
        takeDamage(5);
    }

    // The integer being passed in tells us which skillbutton is being pressed, which corresponds to the character's array of skills.
    public void playerHealsSelf(int whichSkill)
    {
        print(whichSkill - 1);
        int cureAmount = 0;

        if (character.getSkills()[whichSkill].getName().Equals("Heal"))
            cureAmount = 25;


        fightMessage = "You feel refreshed! + " + cureAmount + " HP";
        takeDamage(-(cureAmount));
    }
    // The integer being passed in tells us which skillbutton is being pressed, which corresponds to the character's array of skills.
    public void playerAttacksEnemy(int whichSkill)
    {
        int attackAmount = playerMeleeDamage;

        //if (character.getSkills()[whichSkill].getName().Equals("Photorealistic 4K Fireball"))
        //    attackAmount = 99999999;

        fightMessage = "You attack " + selection.getArrowPos() + "! It does " + attackAmount + " HP";
        giveDamage(5);
    }

    public void giveDamage(int damage)
    {
        float x = 100;
        x -= damage * (buffMultiplier);
        logic.setFightMessage(fightMessage);
    }

    public void takeDamage(int damage)
    {
        character.setHP(character.getHP() - (int)(damage * enemyBuffMultiplier));
        logic.setFightMessage(fightMessage);
    }
}
