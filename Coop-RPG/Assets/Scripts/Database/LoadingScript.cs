using UnityEngine;
using System.Collections;

namespace AssemblyCSharp{
public class LoadingScript : MonoBehaviour {
		DatabaseManager db = new DatabaseManager();
		Characters ch;
		Monster mon;
		string monList;
		Characters[] chList;
		Monster[] monsterList;

		static int debug_idx = 0;
		[SerializeField]
		TextMesh txt;
	// Use this for initialization
	void Start () {
			StartCoroutine (run ());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

		IEnumerator run() {
			int mean = 0;
			string[] charList = { "Lex", "Example" };
			chList = new Characters[charList.Length];

			for (int i = 0; i < charList.Length; i++) {
				DoDebug (charList [i]);
				yield return StartCoroutine(loadChar (charList[i]));
				chList [i] = ch;
				mean += ch.getLevel ();
				DoDebug ("--------------------");
			}
			mean /= chList.Length;
			DoDebug ("mean: " + mean);

			mean = 1;
			yield return StartCoroutine (getMonList(mean));
			DoDebug ("monlist: " + monList);
			string[] mList = monList.Split (';');
			monsterList = new Monster[mList.Length];
			for (int i = 0; i < mList.Length; i++) {
				DoDebug (mList [i]);
				yield return  StartCoroutine (loadMon (mList [i]));
				monsterList [i] = mon;
			}
			//getMons (monList);
			yield return new WaitForSeconds (1f);
		}

		private IEnumerator loadChar(string charName) {
			yield return StartCoroutine (db.runChar (charName));
			ch = db.getChar ();

		}

		private IEnumerator loadMon(string monName) {
			yield return StartCoroutine (db.runMon (monName));
			mon = db.getMon ();
		}

		private IEnumerator getMonList(int mean) { 
			if (mean <= 5) {
				yield return StartCoroutine (db.runMonList ("1-5"));
			
				monList = db.getMonsterList ();

			} else if (mean < 11) {
				yield return StartCoroutine (db.runMonList ("6-10"));

				monList = db.getMonsterList ();

			} else {

				yield return StartCoroutine (db.runMonList ("11+"));

				monList = db.getMonsterList ();

			}

		}

		private IEnumerator getMons(string list) {
			
			string[] mons = list.Split (';');
			monsterList = new Monster[mons.Length];

			for (int i = 0; i < mons.Length; i++) {
				
				StartCoroutine(getMonster (mons [i]));
				monsterList [i] = mon;
			}
			yield return new WaitForSeconds (3f);


		}

		IEnumerator getMonster(string name) {
			yield return StartCoroutine (db.runMon (name));
			mon = db.getMon ();
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
