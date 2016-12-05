using UnityEngine;
using System.Collections;

namespace AssemblyCSharp{
public class DungeonEnd : MonoBehaviour {
		int dungeonExp = 0;
		DatabaseManager db = new DatabaseManager();
		string[] clPList;
		Characters[] cList;
		string tempStat;
		string tempPerk;

	// Use this for initialization
	void Start () {
			foreach (Characters ch in cList) {
				if (charLeveled (ch)) {
					int newExp = ch.getExp() + dungeonExp;
					int newLevel = checkLevel (newExp);
					StartCoroutine (db.runUpdateChar (ch.getName(), newExp, newLevel, tempStat, tempPerk));
				} else {
					StartCoroutine (db.runUpdateChar(ch.getName(), ch.getExp() + dungeonExp, 0, "", ""));
				}

			}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		IEnumerator getClassPerks(string clName) {
			yield return StartCoroutine (db.runClPerks (clName));
			clPList = db.getClassPerkList();
		}

		string perksForLevel(int level) {
			return clPList [level];
		}

		void levelUp(Characters ch, string stat, string perk) {
			
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

}
}
