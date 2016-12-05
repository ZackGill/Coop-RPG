using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace AssemblyCSharp{
public class LoadingScript : NetworkBehaviour {
		DatabaseManager db = new DatabaseManager();
		public Characters ch;
		public Monster mon;
		string monList;
		public Characters[] chList;
		public Monster[] monsterList;

        [SyncVar]
        public bool loading = true;

        static LoadingScript _instance;

        public static LoadingScript Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<LoadingScript>();
                }
                return _instance;
            }
        }


		static int debug_idx = 0;
		[SerializeField]
		TextMesh txt;
	// Use this for initialization
	void Start () {

            Invoke("help", 1f);
	}

    void help()
        {
            StartCoroutine(run());

        }

        // Update is called once per frame
        void Update () {
	
	}

		IEnumerator run() {
			int mean = 0;


            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            string[] charList = new string [players.Length];
            for(int i = 0; i < players.Length; i++)
            {
                charList[i] = players[i].GetComponent<PlayerMovement>().charName;
            }
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


            // Assign Monsters and Characters to everything

            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerMovement>().characterInfo = chList[i];
                players[i].GetComponent<PlayerMovement>().health = chList[i].getHP();
            }
            // Note: When Assigning monsters, going based on monster list on Firebase
            // Goes Patrol Charge, Boss, Wander Stalk.


                PatrolCharge_Overworld[] temp = GameObject.FindObjectsOfType<PatrolCharge_Overworld>();
                for(int a = 0; a < temp.Length; a++)
                {
                    temp[a].gameObject.GetComponent<MonsterStorage>().monster = monsterList[0];
                }


            WanderStalk_Overworld[] temp2 = GameObject.FindObjectsOfType<WanderStalk_Overworld>();
            for (int a = 0; a < temp.Length; a++)
            {
                temp[a].gameObject.GetComponent<MonsterStorage>().monster = monsterList[2];
            }


            TotallyABossOverworld[] temp3 = GameObject.FindObjectsOfType<TotallyABossOverworld>();
            for (int a = 0; a < temp.Length; a++)
            {
                temp[a].gameObject.GetComponent<MonsterStorage>().monster = monsterList[1];
            }


            //getMons (monList);
            // See if this runs once all done. It should.
            loading = false;
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
