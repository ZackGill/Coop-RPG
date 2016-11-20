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
    private int sampleEnemySkill = 2;
    private int regularEnemySkill = 1;
    private int coolDownTimerAttack = 0;
	private int[] player = { 1, 2, 3, 4 };
	private int randomPlayer = Random.Range (1, 5);


    //assuming the case of enemy not having any healing skills of its own
	public int AI()
    {
		//compare if random player is still alive or not, if so, keep attacking the same one, or be completely random and attack
		//random players.
		//just an example for tonignt so I don't forget
		if (coolDownTimerAttack == 0) {
			if (Random.Range (0.00f, 1.00f) > monster.getMistakeChance()) {
				//add what ever it needs to do to attack here.
				return sampleEnemySkill;
				coolDownTimerAttack = 3;
			} else {
				return -1;
			}
		} else {
			/* *********************************************************
             * example enemy regular skill. If all of enemy's skill are
             * in cooldown, use regular skill.
             * ********************************************************/
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
