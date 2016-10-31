using UnityEngine;
using System.Collections;

public class Player : Entity {
	public int xp;

	// Use this for initialization
	void Start () {
		base.SetEntityName("Harry");
		base.SetAttack(1);
		base.SetDefense(1);
		base.SetHPMax(100);
		base.SetHPCurrent(100);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public CombatMove ChooseMove()
    {
        return new CombatMove();
    }

	public int GetXP() {
		return xp;
	}

	public void SetXP(int xp) {
		this.xp = xp;
	}
}
