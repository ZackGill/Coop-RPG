using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleSystem : MonoBehaviour {

	List<CombatMove> moveQueue = new List<CombatMove>();
	List<Player> players = new List<Player>();
	List<Enemy> enemies = new List<Enemy>();

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
			var timeSort = from move in moveQueue
			               orderby move.GetSpeed()
			               select move;
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
			if (move.GetMoveType() == "Run") {
				int playerRun = Random.Range(1, 101) % 10;
				int enemyRun = 0;
				foreach (Enemy enemy in enemies) {
					enemyRun += enemy.runVal;
				}
				if (playerRun >= enemyRun) {
					print ("Run Success");
				} else {
					print ("Run Fail");
				}
			}
		}
	}
}
