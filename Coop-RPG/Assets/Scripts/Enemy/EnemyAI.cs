using UnityEngine;
using System.Collections;

public class EnemyAI {

	private int totalPlayerHealth = 70;
	private int healthPercentage;
    private float mistakeChance = 0.13f;
    private int enemyHP = 30;
    private int bossTag = 0;
    private int level = 2;

    //enemy stats
    private int attack = 5, defense = 7, magic = 4;
    private float enemymodifier = 0.50f;
    private string type = "attack", type2 = "magic";
    private int coolDownTimerAttack = 0;
    private int coolDownTimerMagic = 0;

    //player defense stat to calculate what their defense is
    //might not need this but just for the heck of it for now.
    private int playerDefense = 2;
	
    //assuming the case of enemy not having any healing skills of its own
    public void AI()
    {
        healthPercentage = (int)(totalPlayerHealth / 100) * 100;

        //just an example for tonignt so I don't forget
        if (type == "attack" && coolDownTimerAttack == 0)
        {
            //might have to change this calculation a bit, works for smaller numbers, but gets bigger as the attack and level gets larger
            int enemyDamage = (level * 2) + Mathf.CeilToInt((level * enemymodifier) * attack);

            //increase the damage even more depending on chosen player's defense, if low, increased damage, if high, decreased damage.
            int damageModifier = Mathf.CeilToInt((attack - defense) * 0.5f);
            enemyDamage *= damageModifier;

            if (Random.Range(0.00f, 1.00f) > mistakeChance)
            {
                //add what ever it needs to do to attack here.
                Debug.Log(enemyDamage);
                coolDownTimerAttack = 2;
            }
            else
            {
                Debug.Log("Enemy Missed!");
            }
            
        }

        if(type2 == "magic" && coolDownTimerMagic == 0)
        {
            //might have to change this calculation a bit, works for smaller numbers, but gets bigger as the attack and level gets larger
            //we could also do enemyHP/2 and impelement level + type in as well instead of this.
            int enemyDamage = (level * 2) + Mathf.CeilToInt((level * enemymodifier) * magic);

            //increase the damage even more depending on chosen player's defense, if low, increased damage, if high, decreased damage.
            int damageModifier = Mathf.CeilToInt((magic - defense) * 0.5f);
            enemyDamage *= damageModifier;

            if (Random.Range(0.00f, 1.00f) > mistakeChance)
            {
                Debug.Log(enemyDamage);
                coolDownTimerMagic = 3;
            }
            else
            {
                Debug.Log("Enemy Missed!");
            }
        }
        else
        {
            //for every tick, update coolDownTimer
            coolDownTimerAttack--;
            coolDownTimerMagic--;

        }

        //if the specific is a boss, the damage may increase by putting the boss modifier in to increase damage done.


        /* ***********************************************************
         * this method could possibly be used for another AI method.
         * which alternates its skill choices as the healthpercentage
         * of players changes.
         * ***********************************************************/

      /*  if(healthPercentage >= 75)
        {
            if(Random.Range(0.00f, 1.00f) > mistakeChance)
            {
                
                Debug.Log("-10 HP");
            }else
            {
                Debug.Log("Enemy Missed!");
            }
        }else if(healthPercentage < 75 && healthPercentage >= 25)
        {
            if (Random.Range(0.00f, 1.00f) > mistakeChance)
            {
                Debug.Log("-10 HP");
            }
            else
            {
                Debug.Log("Enemy Missed!");
            }
        }
        else if(healthPercentage < 25)
        {
            if (Random.Range(0.00f, 1.00f) > mistakeChance)
            {
                Debug.Log("-10 HP");
            }
            else
            {
                Debug.Log("Enemy Missed!");
            }
        }*/
    }
	
    /************************************************************************************************************************************
     * depending on how the battle system is impelemnted, could also calculate amount of damage any enemy ai can take by calculating
     * the player damage, with the enemy defense, and send the result back to subtract from current enemy hp.
     * *********************************************************************************************************************************/
    
}
