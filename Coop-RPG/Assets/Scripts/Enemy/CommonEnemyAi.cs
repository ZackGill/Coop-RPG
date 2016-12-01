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

	//private Monster monster;
	//private BattleLogic logic;
    //enemy stats assuming its a dragon type =p
   	//private int sampleEnemySkill = 2;
    private int regularEnemySkill = -2;  			//indicate regular skill as -2 to avoid conflicts
    private int coolDownTimerAttack = 0;
	//private int[] playerThreat;
	Skill[] enemySkills = new Skill[8];
	int[][] coolDownList;

	public int AI(Monster enemy, int whichMonster)
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

		if (coolDownList[whichMonster][chosenSkill] == 0) {
			if (Random.Range (0.00f, 1.00f) > enemy.getMistakeChance()) {
				//TODO: have cooldown timers for each skill instead of just one.

				coolDownList[whichMonster][chosenSkill] = enemySkills[chosenSkill].getCooldown();
				return chosenSkill;
			} else {
				return -1;
			}
		} else {
			/**********************************************************
             * example enemy regular skill. If all of enemy's skill are
             * in cooldown, use regular skill.
             *********************************************************/
			if (Random.Range (0.00f, 1.00f) > enemy.getMistakeChance()) { 
				return regularEnemySkill;
			} else {
				return -1;
			}
		}
    }

	//this is to select players at complete random. use if you want to.
	public int playerSelect(int numberofPlayers){
		return Random.Range (1, numberofPlayers);
	}
	
	//TODO: add the playerthreat inside the param by which skill a player uses. 
	//	(suppose this can be done inside battle logic for simplicity, as to update each player's threat everytime it uses a skill on their turn).
	//	if a player leaves or is dead, simply remove them from the threat array on the battle logic side.
	//  int numberofPlayers --> represents total number of players currently in battle, this is to make sure if playerThreat is 0 for all players
	//			                it can use random generator to choose a player that way.
    //  If you want, you can also do it individually, for example, player0, player1, player2, but just add those to params, and loop over by number
    //  of players and replace with highest threat. If both player's threat happens to be equal, choose the player with lowest health
    //  wasn't sure how lists of Character Players were handled so just commented them inside the param
	public int playerSelectWithThreat(int numberofPlayers, int[] playerThreat /* add lists of players here... ex. Characters[] player*/){
		int highthreat = 0;
		int selectedplayer = -1;    //NOTE: you can also have global variable "currentTarget" to keep track of its current target
		for(int i = 0; i < numberofPlayers; i++){
			if(highthreat <= playerThreat[i] && selectedplayer != i){
               // if (highthreat == playerThreat[i])
               // {
                    //possible route....
                    //if(player[selectedplayer].getHP > player[i].getHP){
                    //  selectedplayer = i;
                    //  hightreat = playerThreat[i];
                    //}
               // }
              //  else
               // {
                    selectedplayer = i;
                    highthreat = playerThreat[i];
               // }
			}
		}
		
		if(selectedplayer == -1){
			return Random.Range(1, numberofPlayers);
		}else{
			return selectedplayer;
		}
	}

   	void update()
    {
        for (int j = 0; j < coolDownList.Length; j++)
        {
            for (int i = 0; i < coolDownList[j].Length; i++)
            {
                if (coolDownList[j][i] > 0)
                {
                    coolDownList[j][i]--;
                }
            }
        }
    }
}
