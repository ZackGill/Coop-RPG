using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class CommonEnemyAi : MonoBehaviour
{
    /****************************************************************************
     * some of these commands may have to be under update tab, for the
     * cooldowntimer purposes.
     * 
     * I don't know much about jrpg, but tried playing
     * some of it and implementing basic understandings of it, which deals with
     * enemy AI using their skills whenever available, and when its not, just uses normal
     * attacks.
     * 
     * depending on how the battle system goes, can do damage calculations inside this
     * AI component as well, however, for now, just doing normal EnemyAI duties, which
     * involves choosing its own moves, then possibly sending it to the main battle
     * system if enemy does not miss. (in this case, mistake chance is hard coded 13%.)
     * 
     * Can turn these into enemy skill array, this is just a sample with one enemy type
     * skill and one enemy type normal attack. Enemy skill have cooldowns, normal does
     * not.
     * 
     * **************************************************************************/

	private Monster monster;
	private BattleLogic logic;
    //enemy stats assuming its a dragon type =p
    //private int sampleEnemySkill = 2;
    private int regularEnemySkill = -2;  			//indicate regular skill as -2 to avoid conflicts
    private int coolDownTimerAttack = 0;
	private int[] player = { 1, 2, 3, 4 };
	Skill[] enemySkills = new Skill[8];
	private int randomPlayer = Random.Range (1, 5);

    //assuming the case of enemy not having any healing skills of its own
	public int AI(Monster enemy)
    {
		//compare if random player is still alive or not, if so, keep attacking the same one, or be completely random and attack
		//random players.
		//just an example for tonignt so I don't forget
		int total = 0;
		enemySkills = enemy.getSkills();		//sets the enemy skills

		//loops through to add the values of all enemy skills together
		for (int i = 0; i < enemy.getSkills().Length; i++) {
			total += enemySkills [i].getValue ();
		}

		//it then will trigger a random range, as well as initializing current value pointer and the current chosen skill
		int ran = Random.Range (0, total);
		int current = 0;
		int chosenSkill = 0;

		/**********************************************************************************************************************
		* loops through once more, but this time it tries to find the chosen skills by checking each skill's value with
		* random generator. The chosen skills value will not change unless the current pointer's skill is less than equal
		* to the random generator's value or greater than equal to the current pointer.
		**********************************************************************************************************************/
		for (int i = 0; i < enemy.getSkills().Length; i++) {
			if (ran >= enemySkills [i].getValue () && current <= enemySkills [i].getValue ()) {
				chosenSkill = i;
				current = enemySkills [i].getValue ();
			}
		}

		if (coolDownTimerAttack == 0) {
			if (Random.Range (0.00f, 1.00f) > monster.getMistakeChance()) {
				
				//TODO: have cooldown timers for each skill instead of just one.
				coolDownTimerAttack = enemySkills[chosenSkill].getCooldown();
				return chosenSkill;
			} else {
				return -1;
			}
		} else {
			/**********************************************************
             * example enemy regular skill. If all of enemy's skill are
             * in cooldown, use regular skill.
             *********************************************************/
			if (Random.Range (0.00f, 1.00f) > monster.getMistakeChance()) { 
				return regularEnemySkill;
			} else {
				return -1;
			}
		}
    }

    void update()
    {
        if (coolDownTimerAttack != 0)
        {
            coolDownTimerAttack--;
        }
    }
}
