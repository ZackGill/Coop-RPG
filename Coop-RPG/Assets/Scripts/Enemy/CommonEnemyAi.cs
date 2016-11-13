using UnityEngine;
using System.Collections;

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

    private float mistakeChance = 0.13f;

    //enemy stats assuming its a dragon type =p
    private string sampleEnemySkill = "Fire Ball";
    private string regularEnemySkill = "Tail swing";
    private int coolDownTimerAttack = 0;

    //assuming the case of enemy not having any healing skills of its own
    public void AI()
    {
        //just an example for tonignt so I don't forget
        if (coolDownTimerAttack == 0)
        {

            if (Random.Range(0.00f, 1.00f) > mistakeChance)
            {
                //add what ever it needs to do to attack here.
                Debug.Log(sampleEnemySkill);
                coolDownTimerAttack = 3;
            }
            else
            {
                Debug.Log("Enemy Missed!");
            }

        }
        else
        {
            /* *********************************************************
             * example enemy regular skill. If all of enemy's skill are
             * in cooldown, use regular skill.
             * ********************************************************/
            if (Random.Range(0.00f, 1.00f) > mistakeChance)
            { 
                Debug.Log(regularEnemySkill);
            }
            else
            {
                Debug.Log("Enemy Missed!");
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
