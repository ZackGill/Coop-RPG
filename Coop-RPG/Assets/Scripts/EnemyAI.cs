using UnityEngine;
using System.Collections;

public class EnemyAI {

	private int totalPlayerHealth;
	private int healthPercentage;
	private BaseAbility chosenAbility;

	//this is a very very basic part of the Enemy AI for sprint 1... can go on further on this class if we need to make it advanced later on
	public BaseAbility EnemyAbility(){
		//totalPlayerHealth = ***get the player health***
		//healthPercentage = (int)(totalPlayerHealth / 100) * 100;

		//if(healthPercentage >= 75)
		//	return chosenAbility = ChooseAbilityAtSeventyFivePercent()
		//else if(healthPercentage < 75 && healthPercentage >= 25)
		// return chosenAbility = ***chosen skill***
		//else if(healthPercentage < 25)
		// return chosenAbility = ***chosen skill***
		return chosenAbility;
	}

	private BaseAbility ChooseAbilityAtSeventyFivePercent(){
        //return chosenAbility = ***chosen skill***;
        return new BaseAbility();
	}
}
