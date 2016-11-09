using UnityEngine;
using System.Collections;

public class Enemy : Entity {

    public int runVal, sightRange, bossTag;
	public double mistakeChance;

	// Use this for initialization
	void Start () {
		base.SetEntityName("Spookeroni");
		base.SetHPMax(30);
		base.SetHPCurrent(30);
	}

	public Enemy() {

	}

	public Enemy(string enemyName) {
		base.SetEntityName (enemyName);
	}

	public Enemy(string entityName, double mistakeChance, int sightRange, int hp, int bossTag, int level){
		base.SetEntityName (entityName);
		SetMistakeChance(mistakeChance);
		SetSightRange (sightRange);
		base.SetHPMax (hp);
		base.SetHPCurrent (hp);
		SetBossTag (bossTag);
		base.SetLevel (level);
		//base.SetPerks (perks);
		//base.SetMoveList (skills);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public CombatMove ChooseMove()
    {
        //return base.moveList[0];
		return new CombatMove();
    }

	public int GetRunVal() {
		return runVal;
	}

	public void SetRunVal(int runVal) {
		this.runVal = runVal;
	}

	public int GetSightRange() {
		return sightRange;
	}

	public void SetSightRange(int sightRange) {
		this.sightRange = sightRange;
	}

	public int GetBossTag() {
		return bossTag;
	}

	public void SetBossTag(int bossTag) {
		this.bossTag = bossTag;
	}

	public double GetMistakeChance() {
		return mistakeChance;
	}

	public void SetMistakeChance(double mistakeChance) {
		this.mistakeChance = mistakeChance;
	}
}
