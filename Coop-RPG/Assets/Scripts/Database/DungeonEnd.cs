using UnityEngine;
using System.Collections;

namespace AssemblyCSharp{
public class DungeonEnd : MonoBehaviour {

		static int debug_idx = 0;
		[SerializeField]
		TextMesh txt;


		int dungeonExp = 0;
		DatabaseManager db = new DatabaseManager();
		string[] clPList;
		Characters[] cList;
		string tempStat;
		string tempPerk;

	// Use this for initialization
	void Start () {
			StartCoroutine(run ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		IEnumerator run() {
			foreach (Characters ch in cList) {
				DoDebug (ch.getName ());
				//Checks if player leveled to know how to use Database manager
				if (charLeveled (ch)) {
					//HERE WE NEED TO SET tempStat AND tempPerk
					//INFO FROM UI
					int newExp = ch.getExp() + dungeonExp;
					int newLevel = checkLevel (newExp);
					yield return StartCoroutine (db.runUpdateChar (ch.getName(), newExp, newLevel, tempStat, tempPerk));
				} else {
					yield return StartCoroutine (db.runUpdateChar(ch.getName(), ch.getExp() + dungeonExp, 0, "", ""));
				}

			}
		}


		IEnumerator getClassPerks(string clName) {
			yield return StartCoroutine (db.runClPerks (clName));
			clPList = db.getClassPerkList();
		}

		string perksForLevel(int level) {
			return clPList [level];
		}
			

		bool charLeveled(Characters ch) {
			int exp = ch.getExp ();
			exp += dungeonExp;
			return (checkLevel (exp) > ch.getLevel ());
		}

		int checkLevel(int exp) {
			if (exp < 100) {
				return 1;
			} else if (exp < 250) {
				return 2;
			} else if (exp < 500) {
				return 3;
			} else if (exp < 800) {
				return 4;
			} else {
				return 5;
			}
			return 0;
		}

		void DoDebug(string str)
		{
			Debug.Log(str);
			if (txt != null)
			{
				txt.text += (++debug_idx + ". " + str) + "\n";
			}
		}

}
}
