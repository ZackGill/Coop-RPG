using UnityEngine;
using System.Collections;

namespace AssemblyCSharp{
public class DungeonEnd : MonoBehaviour {
		public int dungeonExp = 0;
		DatabaseManager db = new DatabaseManager();
		public string[] clPList;
		public Characters[] cList;
		public string tempStat;
		public string tempPerk;

	// Use this for initialization
	void Start () {

    }

    public void updateChars()
        {
            foreach (Characters ch in cList)
            {
                if (charLeveled(ch))
                {
                    int newExp = ch.getExp() + dungeonExp;
                    int newLevel = checkLevel(newExp);
                    StartCoroutine(db.runUpdateChar(ch.getName(), newExp, newLevel, tempStat, tempPerk));
                }
                else
                {
                    StartCoroutine(db.runUpdateChar(ch.getName(), ch.getExp() + dungeonExp, checkLevel(ch.getExp() + dungeonExp), "", ""));
                }

            }
        }
	
	// Update is called once per frame
	void Update () {
	
	}
		public IEnumerator getClassPerks(string clName) {
			yield return StartCoroutine (db.runClPerks (clName));
			clPList = db.getClassPerkList();
		}

		public string perksForLevel(int level) {
			return clPList [level];
		}


		public void levelUp(Characters ch, string stat, string perk) {
            StartCoroutine(db.runUpdateChar(ch.getName(), ch.getExp() + dungeonExp, checkLevel(ch.getExp() + dungeonExp), stat, perk));
        }

        public bool charLeveled(Characters ch) {
			int exp = ch.getExp ();
			exp += dungeonExp;
			return (checkLevel (exp) > ch.getLevel ());
		}

		public int checkLevel(int exp) {
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
