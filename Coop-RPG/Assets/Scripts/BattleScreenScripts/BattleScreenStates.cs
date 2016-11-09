using UnityEngine;
using System.Collections;

public class BattleScreenStates : MonoBehaviour {


    // These states will represent "whose turn" it is.
    public enum FightStates
    {
        BEGINNING,
        PLAYERTURN,
        NEUTRAL,
        ENEMYTURN,
        SECONDENEMYJOINS,
        THIRDENEMYJOINS,
        PICKANENEMY,
        LOSE,
        WIN
    }

    public FightStates curState;

	void Start () {
        curState = FightStates.BEGINNING;
	}
	
	void Update () {

        //switch (curState)
        //{
        //    case (FightStates.BEGINNING): // A message or description about the foe should appear.
        //        break;
        //    case (FightStates.PLAYERTURN): // The player uses his move.
        //        break;
        //    case (FightStates.ENEMYTURN): // The enemy attacks.
        //        break;
        //    case (FightStates.LOSE): // You and your friends are dead. Game over.
        //        break;
        //    case (FightStates.WIN): // Message congratulating the players.
        //        break;
        //}
	}
}
