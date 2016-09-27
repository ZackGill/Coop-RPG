using UnityEngine;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour {

	List<CombatMove> moveQueue = List<BattleMove>();
	List<Player> players = List<Player>();
	List<Enemy> enemies = List<Enemy>();

	// Use this for initialization
	void Start () {
		while (enemies != null || players != null) {
			// players choose moves
			foreach (Player player in players) {
				moveQueue.Add (player.ChooseMove ());
			}
			// enemies choose moves
			foreach (Enemy enemy in enemies) {
				moveQueue.Add (enemy.ChooseMove ());
			}
			// sort and execute
			//moveQueue.Sort (); //C# has built in sort function for list
			ExecuteMoves();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//render every tick
	}

	// Goes down list and perfroms each move
	void ExecuteMoves(List<CombatMove> moves) {
		foreach (CombatMove move in moves) {
			// execute
		}
	}
}
