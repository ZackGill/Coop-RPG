using System.Collections.Generic;
using System.Collections;
using System;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;
namespace AssemblyCSharp
{
	public class DatabaseManager
	{
		public Coroutine co { get; private set; }
		string tempJson;
		string skillList;
		string perkList;
		string[] charList;
		string equipJson;
		string className;
		string[] classDesc;
		string clPerkJson;
		int attackToAdd = 0;
		int defenseToAdd = 0;
		int magicToAdd = 0;
		Characters ch = null;
		Monster mon = null;
		bool logOnOk = false;
		string monList;
		string[] clPerkList;


		public bool isDone = false;



		static int debug_idx = 0;
		[SerializeField]
		TextMesh txt;

		public DatabaseManager() {

		}

		/*
		void Start() {
			string yoo = "Example";
			//StartCoroutine (runChar (yoo));
		}
		*/

		private void checkLogOn(string pass) {
			string save = tempJson;
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			string[] sp = tempJson.Split (',');
			foreach (string s in sp) {
				string[] t = s.Split (':');
				if (t [0].Equals ("\"password\"")) {
					string test = t [1].Substring (1, t [1].Length - 2);
					if (test.Equals (pass)) {
						logOnOk = true;
						tempJson = save;
						return;
					}

				}
			}

			logOnOk = false;

			tempJson = save;
		}

		public bool getLogOnOk() {
			return logOnOk;
		}


		void getAccJson(string aName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Accounts", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase acc = fb.Child (aName);
			acc.OnGetSuccess += GetJson;
			acc.GetValue ();
		}

		void getClassDescJson() {
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/ClassDesc", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			fb.OnGetSuccess += GetJson;
			fb.GetValue ();
		}

		private void setClassDesc() {
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			string[] ret = tempJson.Split (',');
			classDesc = ret;
		}

		public string[] getClassDesc() {
			return classDesc;
		}

		public IEnumerator runClassDesc() {
			getClassDescJson ();
			yield return new WaitForSeconds(3f);
			setCharList();
		}

		public void setCharList() {
			tempJson = tempJson.Substring(1, tempJson.Length-2);
			string characters;
			string[] list = tempJson.Split (',');
			characters = list[0].Split (':')[1];
			characters = characters.Substring (1, characters.Length-2);
			//Actual list of chars
			string[] cList = characters.Split (';');
			charList = cList;
		}
			
		public string[] getCharList() {
			return charList;
		}

		private Skill parseSkillJson(string skillName, string skillJson) {
			DoDebug("SkillJSON: " + skillJson);
			int cd, val, threat;
			string targs, type;
			cd = val = threat = 0;
			targs = type = "";
			skillJson = skillJson.Substring (1, skillJson.Length-2);
			string[] list = skillJson.Split (',');
			Skill sk;
			foreach (string s in list) {
				DoDebug ("TEST" + s);
				string[] sp = s.Split (':');
				if (String.Equals(sp[0], "\"cooldown\"")) {
					cd = int.Parse (sp [1]);

				}
				if (sp [0].Equals ("\"value\"")) {
					val = int.Parse (sp [1]);

				}
				if (sp [0].Equals ("\"threatGen\"")) {
					threat = int.Parse (sp [1]);
				
				}
				if (sp [0].Equals ("\"targets\"")) {
					string temp = sp [1].Substring (1);
					targs = temp.Substring (0, 3);
				
				}

				if (sp [0].Equals ("\"type\"")) {
					
					type = sp[1].Substring(1, sp[1].Length-2);
		
				}

			}
			sk = new Skill(skillName, cd, val, threat, targs, type);
			return sk;
		}

		void getSkills(ref Characters ch, string sList, string pList) {
			string[] skillList = sList.Split(';');
			Skill[] temp = new Skill[skillList.Length];


		}

		void parsePerk(ref Skill sk, string perkJson) {
			string type = "";
			int value = 0;
			perkJson = perkJson.Substring (1);
			string[] list = perkJson.Split (',');
			foreach (string s in list) {

				string[] sp = s.Split (':');
				if (String.Equals(sp[0], "\"type\"")) {
					type = sp [1].Substring (1, sp [1].Length - 2);

				}
				if (sp [0].Equals ("\"value\"")) {
					value = int.Parse (sp [1].Substring(0,1));

				}
		

			}
			sk.applyPerk (type, value);
		}

		void parseCharInfo(int mode, ref Characters ch, string charName) {
			string acc1, acc2, weapon, armor, wType, acc1Type, acc2Type;
			int attack, magic, defense, hp, exp, lvl;
			attack = magic = defense = hp = exp = lvl = 0;

			string clName;
			Skill[] skills;

			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			char[] chArr = new char[2];
			chArr [0] = '{';
			chArr [1] = '}';
			string[] splJson = tempJson.Split (chArr);
			equipJson = splJson [1];
			string statJson = splJson [3];
			for (int i = 0; i < 3; i++) {
				DoDebug (splJson [i]);
			}



			string[] fBlock = splJson [0].Split (',');
			foreach (string s in fBlock) {
				string[] sp = s.Split (':');
				if (String.Equals(sp[0], "\"HP\"")) {
					hp = int.Parse (sp [1]);

				}
				if (sp [0].Equals ("\"EXP\"")) {
					exp = int.Parse (sp [1]);

				}

				if (sp [0].Equals ("\"LVL\"")) {
					lvl = int.Parse (sp [1]);

				}

				if (sp [0].Equals ("\"class\"")) {
					className = sp [1].Substring (1, sp [1].Length - 2);
					DoDebug (className);
				

				}
			}

		

			string[] nBlock = splJson [2].Split (',');
			foreach (string s in nBlock) {
				string[] sp = s.Split (':');
				DoDebug ("!!!!!!HERE!!!!!!  " + sp [0]);
				if (String.Equals(sp[0], "\"perks\"")) {
					perkList = sp [1].Substring (1, sp [1].Length - 2);

				}
				if (sp [0].Equals ("\"skills\"")) {

					skillList = sp [1].Substring (1, sp [1].Length - 2);
				}
			}

			if (mode == 0) {
				return;
			}

			string[] sBlock = statJson.Split (',');
			foreach (string s in sBlock) {
				string[] sp = s.Split (':');
				if (sp [0].Equals ("\"attack\"")) {
					attack = int.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"magic\"")) {
					magic = int.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"defense\"")) {
					defense = int.Parse (sp [1]);
				}
			}

			DoDebug ("ATTACK: " + attack);
			ch = new Characters (charName, className, attack, magic, defense, hp, exp, lvl);
		}

		void parseWeaponInfo() {
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			string[] info = tempJson.Split (',');
			string type = info [0].Split (':')[1];
			type = type.Substring (1, type.Length - 2);
			int val = int.Parse(info[1].Split(':')[1]);

			if (type.Equals ("attack")) {
				attackToAdd += val;
			}

			if (type.Equals ("magic")) {
				magicToAdd += val;
			}


		}

		void parseArmorInfo() {
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			int stat = int.Parse (tempJson.Split(':')[1]);

			defenseToAdd += stat;
		}

		void parseAccInfo() {
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			string[] list = tempJson.Split (',');
			string stat = list [0].Split (':') [1];
			stat = stat.Substring (1, stat.Length - 2);
			int val = int.Parse(list [1].Split (':') [1]);

			if (stat.Equals ("attack")) {
				attackToAdd += val;
			}

			if (stat.Equals ("magic")) {
				magicToAdd += val;
			}

			if (stat.Equals ("defense")) {
				defenseToAdd += val;
			}
		}

		void parseClassInfo() {
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			char[] chArr = new char[2];
			chArr [0] = '{';
			chArr [1] = '}';
			string[] list = tempJson.Split (chArr);
			string statJson = list [1];

			string[] sBlock = statJson.Split (',');
			foreach (string s in sBlock) {
				string[] sp = s.Split (':');
				if (sp [0].Equals ("\"attack\"")) {
					DoDebug (sp [1]);
					attackToAdd += int.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"magic\"")) {
					DoDebug (sp [1]);
					magicToAdd += int.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"defense\"")) {
					DoDebug (sp [1]);
					defenseToAdd += int.Parse (sp [1]);
				}
			}

			clPerkJson = list [2].Split (':')[1];

		}

		void getClassInfo() {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Classes", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase cl = fb.Child (className);
			cl.OnGetSuccess += GetJson;
			cl.GetValue ();
		}


		void getCharInfo(string cName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Characters", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase chara = fb.Child (cName);
			chara.OnGetSuccess += GetJson;
			chara.GetValue ();
		}



		void getSkillInfo(string skillName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/SkillLU", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase skill = fb.Child (skillName);
			skill.OnGetSuccess += GetJson;
			skill.GetValue ();
		}

		void getSPerkInfo(string perkName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/PerkLU", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase perk = fb.Child (perkName);
			perk.OnGetSuccess += GetJson;
			perk.GetValue ();
		}

		void getAccInfo(string accName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/ItemLU/accesory", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase acc = fb.Child (accName);
			acc.OnGetSuccess += GetJson;
			acc.GetValue ();

		}

		void getArmorInfo(string arName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/ItemLU/armor", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase arm = fb.Child (arName);
			arm.OnGetSuccess += GetJson;
			arm.GetValue ();
		}

		void getWeapInfo(string wName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/ItemLU/weapons", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase wep = fb.Child (wName);
			wep.OnGetSuccess += GetJson;
			wep.GetValue ();
		}


		void DoDebug(string str)
		{
			Debug.Log(str);
			if (txt != null)
			{
				txt.text += (++debug_idx + ". " + str) + "\n";
			}
		}

		public IEnumerable runAcc(string accName, string pass) {
			getAccJson (accName);
			DoDebug("WAITING");
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			checkLogOn (pass);
			setCharList ();


		}

		public IEnumerator runChar(string charName) {
			getCharInfo (charName);
			//skillList = "Spin-Slash";
			//WAIT

			Characters x = null;
			DoDebug("WAITING");
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseCharInfo (0, ref x, charName);

			getClassInfo ();
			DoDebug("WAITING");
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");

			parseClassInfo ();
			string[] e = equipJson.Split (',');
			string temp = e [0].Split (':') [1];
			temp = temp.Substring (1, temp.Length - 2);
			getAccInfo (temp);
			DoDebug("WAITING ON " + temp);
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseAccInfo ();

			temp = e [1].Split (':') [1];
			temp = temp.Substring (1, temp.Length - 2);
			getAccInfo (temp);
			DoDebug("WAITING ON " + temp);
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseAccInfo ();

			temp = e [2].Split (':') [1];
			temp = temp.Substring (1, temp.Length - 2);
			getArmorInfo (temp);
			DoDebug("WAITING ON " + temp);
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseArmorInfo ();

			temp = e [3].Split (':') [1];
			temp = temp.Substring (1, temp.Length - 2);
			getWeapInfo (temp);
			DoDebug("WAITING ON " + temp);
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseWeaponInfo();
			/*
			getClassInfo ();
			DoDebug("WAITING ON " + className);
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseClassInfo();
*/
	

			DoDebug("ATTACK: " + attackToAdd + "\nDefense: " + defenseToAdd);




			DoDebug ("SKILL LIST: " + skillList);
			DoDebug ("PERK LIST: " + perkList);

			string[] sList = skillList.Split (';');
			DoDebug ("sListLen: " + sList.Length);
			//split skill string
			//make temp skill array
			Skill[] sTemp = new Skill[sList.Length];
			DoDebug ("sTEmplen: " + sTemp.Length);
			for (int i = 0; i < sTemp.Length; i++) {
				DoDebug ("HELLO");
			////getInfo
				getSkillInfo(sList[i]);
				DoDebug("WAITING ON SKILL: " + sList[i]);
				yield return new WaitForSeconds (2f);
				DoDebug("DONE");
			////WAIT
			////parse info

				sTemp[i] = parseSkillJson(sList[i], tempJson);
				DoDebug (sTemp [i].toString ());
			////add skill to array
			//end of loop
			}
			string[] pList = perkList.Split (';');
			//split perk string
			/// for i=0->perkList.length
			for (int i = 0;i<pList.Length;i++) {
			/// getInfo
				getSPerkInfo(pList[i]);
			/// WAIT
				DoDebug("WAITING ON PERK: " + pList[i]);
				yield return new WaitForSeconds (2f);
				DoDebug("DONE");

			/// parseInfo
				parsePerk(ref sTemp[i],tempJson);
				DoDebug (sTemp [i].toString ());
			/// call skill addperk function with parsed info
			/// end loop
			}


			getCharInfo (charName);
			DoDebug("WAITING ON CHAR INFO");
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseCharInfo (1, ref ch, charName);

			ch.setSkills (sTemp);
			ch.setAttack (ch.getAttack () + attackToAdd);
			ch.setMagic (ch.getMagic () + magicToAdd);
			ch.setDefense (ch.getDefense () + defenseToAdd);

			DoDebug("ATTACK: " + ch.getAttack() + "\nDefense: " + ch.getDefense());

			isDone = true;

			yield return ch;

		}

		void GetJson(Firebase sender, DataSnapshot snapshot) {
			tempJson = snapshot.RawJson;
		}

		public Characters getChar() {
			return ch;
		}

		private void getMonJson(string mName) {
			DoDebug ("GETTING JSON FOR " + mName);
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Enemies", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase chara = fb.Child (mName);
			chara.OnGetSuccess += GetJson;
			chara.GetValue ();
		}

		private void parseMonJson(ref Monster mon) {
			float mistakeChance, moveSpeed;
			int attack, magic, defense, hp, level, sightRange;
			bool bossTag = false;
			attack = magic = defense = hp = level = sightRange = 0;
			mistakeChance = 0.0f;
		    moveSpeed = 0.0f;

			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			char[] chArr = new char[2];
			chArr [0] = '{';
			chArr [1] = '}';
			string[] splJson = tempJson.Split (chArr);
			string aiJson = splJson [1];
			string statJson = splJson [3];
			for (int i = 0; i < 3; i++) {
				DoDebug (splJson [i]);
			}
				

			string[] fBlock = splJson [2].Split (',');
			foreach (string s in fBlock) {
				string[] sp = s.Split (':');
				if (String.Equals(sp[0], "\"HP\"")) {
					hp = int.Parse (sp [1]);
					DoDebug ("HP: " + hp);

				}
				if (sp [0].Equals ("\"bossTag\"")) {
					bossTag = bool.Parse (sp [1]);
					DoDebug ("BOSS: " + bossTag);

				}
				if (sp [0].Equals ("\"level\"")) {
					level = int.Parse (sp [1]);
					DoDebug (className);
				}

				if (sp [0].Equals ("\"perks\"")) {
					perkList = sp [1].Substring(1, sp[1].Length-2);
				}

				if (sp [0].Equals ("\"skills\"")) {
					skillList = sp [1].Substring(1, sp[1].Length-2);
				}
			}

			string[] sBlock = statJson.Split (',');
			foreach (string s in sBlock) {
				string[] sp = s.Split (':');
				if (sp [0].Equals ("\"attack\"")) {
					attack = int.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"magic\"")) {
					magic = int.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"defense\"")) {
					defense = int.Parse (sp [1]);
				}
			}

			string[] aiBlock = aiJson.Split (',');
			foreach (string s in aiBlock) {
				string[] sp = s.Split (':');
				if (sp [0].Equals ("\"sightrange\"")) {
					sightRange = int.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"mistakeChance\"")) {
					mistakeChance = float.Parse (sp [1]);
				}
				if (sp [0].Equals ("\"moveSpeed\"")) {
					moveSpeed = float.Parse (sp [1]);
				}
			}




			mon = new Monster (attack, magic, defense, hp, level, bossTag, sightRange, mistakeChance, moveSpeed);

		}

		public IEnumerator runMon(string name) {
			getMonJson (name);

			yield return new WaitForSeconds (3f);

			parseMonJson (ref mon);





			DoDebug ("SKILL LIST: " + skillList);


			string[] sList = skillList.Split (';');
			DoDebug ("sListLen: " + sList.Length);
			//split skill string
			//make temp skill array
			Skill[] sTemp = new Skill[sList.Length];
			DoDebug ("sTEmplen: " + sTemp.Length);
			for (int i = 0; i < sTemp.Length; i++) {
				DoDebug ("HELLO");
				////getInfo
				getSkillInfo(sList[i]);
				DoDebug("WAITING ON SKILL: " + sList[i]);
				yield return new WaitForSeconds (3f);
				DoDebug("DONE");
				////WAIT
				////parse info
				sTemp[i] = parseSkillJson(sList[i], tempJson);
				DoDebug (sTemp [i].toString ());
				////add skill to array
				//end of loop
			}
			string[] pList = perkList.Split (';');
			//split perk string
			/// for i=0->perkList.length
			for (int i = 0;i<pList.Length;i++) {
				/// getInfo
				getSPerkInfo(pList[i]);
				/// WAIT
				DoDebug("WAITING ON PERK: " + pList[i]);
				yield return new WaitForSeconds (3f);
				DoDebug("DONE");

				/// parseInfo
				parsePerk(ref sTemp[i],tempJson);
				DoDebug (sTemp [i].toString ());
				/// call skill addperk function with parsed info
				/// end loop
			}

			mon.setSkills (sTemp);
			DoDebug (mon.getAttack().ToString());
			DoDebug (mon.getMagic().ToString());
			DoDebug (mon.getSkills () [0].toString ());



		}

		public Monster getMon() {
			return mon;
		}

		void newChar(string name, string clName)
		{
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/Characters", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			fb.OnSetFailed += createFailed;
			fb.OnSetSuccess += createSuccess;
			fb.Child(name, true).SetValue("{ \"EXP\": \"1\", \"HP\": \"1\", \"class\": \"" + 
				clName + "\", \"perks\": \"Spin-Slash1\", \"skills\":"+
				" \"Spin-Slash\"}", true);




			Firebase temp = Firebase.CreateNew("coop-rpg.firebaseio.com/Characters/" + name, "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			temp.Child("equipment", true).SetValue("{ \"acc1\": \"NONE\", \"acc2\": \"NONE\", \"armor\": \"rag\", \"weapon\": \"stick\"}", true);

			Firebase temp2 = Firebase.CreateNew("coop-rpg.firebaseio.com/Characters/" + name, "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");


			temp2.Child("stats", true).SetValue("{ \"attack\": \"1\", \"defense\": \"1\", \"magic\": \"1\"}", true);


		}

		public IEnumerator runCreateChar(string name, string clName, string aName)
		{
			newChar(name, clName);
			DoDebug("WAITING CHAR CREATE");
			accCharListJson(aName);
			yield return new WaitForSeconds(5f);
			string list = accCharList ();
			list = list + ";" + name;
			updateCharList (aName, list);
			yield return new WaitForSeconds (5f);


		}

		public IEnumerator runCreateAcc(string name, string pass, string email)
		{
			newAcc(name, pass, email);
			DoDebug("WAITING ACC");
			yield return new WaitForSeconds(3f);
		}

		void newAcc(string name, string pass, string email)
		{
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/Accounts", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			fb.OnSetFailed += createFailed;
			fb.OnSetSuccess += createSuccess;
			fb.Child(name, true).SetValue("{ \"characters\": \"NONE\", \"email\": \"" + email + "\", \"password\": \"" + pass + "\"}", true);
		}

		void createFailed(Firebase sender, FirebaseError err)
		{
			
			DoDebug("" + err.Message);
		
		}


		void createSuccess(Firebase sender, DataSnapshot data)
		{
			DoDebug("Made the thing");
	
		}

		void accCharListJson(string name) {
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/Accounts/" + name, "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			fb.OnGetSuccess += GetJson;
			fb.GetValue ();
		}

		string accCharList() {
			string ret = "";
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			string[] spl = tempJson.Split (',');
			ret = spl [0].Split (':') [1];
			ret = ret.Substring (1, ret.Length - 2);


			return ret;
		}

		void updateCharList(string aName, string list) {
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/Accounts/" + aName, "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase cList = fb.Child ("characters");
			cList.OnSetSuccess += createSuccess;
			cList.SetValue (list);
			DoDebug (list);

		}


		void getMonsterListJson(string cat) {
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/EnemyByLvl", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase monList = fb.Child (cat);
			monList.OnGetSuccess += GetJson;
			monList.GetValue ();
		}

		public string getMonsterList() {
			return monList;
		}

		public IEnumerator runMonList(string cat) {
			getMonsterListJson(cat);
			yield return new WaitForSeconds(5f);
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			DoDebug (tempJson);
			monList = tempJson;

		}

		void getClassPerks(string clName) {
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/Classes/" + clName + "/perks", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			fb.OnGetSuccess += GetJson;
			fb.GetValue ();
		}

		public string[] getClassPerkList() {

			return clPerkList;
		}

		public IEnumerator runClPerks(string clName) {
			getClassPerks (clName);
			yield return new WaitForSeconds (2f);
			tempJson = tempJson.Substring (1, tempJson.Length - 2);
			clPerkList = tempJson.Split (',');

		}

		public IEnumerator runUpdateChar(string cName, int newExp, int newLevel, string stat, string perk) {
			Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/Characters/" + cName, "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase exp = fb.Child ("EXP");
			exp.OnSetSuccess += createSuccess;
			exp.SetValue (newExp);
			if (newLevel != 0) {
				Firebase lvl = fb.Child ("LVL");
				lvl.OnSetSuccess += createSuccess;
				lvl.SetValue (newLevel);
			
				Firebase stats = fb.Child ("stats");
				Firebase mod = stats.Child (stat);
				mod.OnSetSuccess += createSuccess;
				mod.OnGetSuccess += GetJson;
				mod.GetValue ();
				yield return new WaitForSeconds (3f);
				int temp = int.Parse (tempJson);
				mod.SetValue (temp + 1);
				Firebase perkFB = fb.Child ("perks");
				perkFB.OnSetSuccess += createSuccess;
				perkFB.OnGetSuccess += GetJson;
				perkFB.GetValue ();
				yield return new WaitForSeconds (3f);
				tempJson = tempJson.Substring (1, tempJson.Length - 2);
				perkFB.SetValue (tempJson + ";" + perk);
			}



		}

	}


}

