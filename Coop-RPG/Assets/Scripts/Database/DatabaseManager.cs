using System.Collections.Generic;
using System.Collections;
using System;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;
namespace AssemblyCSharp
{
	public class DatabaseManager : MonoBehaviour
	{
		string tempJson;
		string skillList;
		string perkList;
		string charList;
		string equipJson;
		string className;
		string clPerkJson;
		int attackToAdd = 0;
		int defenseToAdd = 0;
		int magicToAdd = 0;

		static int debug_idx = 0;
		[SerializeField]
		TextMesh txt;

		void Start() {
			StartCoroutine (runChar ());
		}

		void getAccJson(string aName) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Accounts", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase acc = fb.Child (aName);
			acc.OnGetSuccess += GetJson;
			acc.GetValue ();
		}

		 void getCharList() {
			tempJson = tempJson.Substring(1);
			string characters;
			string[] list = tempJson.Split (',');
			characters = list[0].Split (':')[1];
			characters = characters.Substring (1, characters.Length-2);
			//Actual list of chars
			string[] cList = characters.Split (';');
			charList = cList [1];

		}
			

		private Skill parseSkillJson(string skillName, string skillJson) {
			int cd, val, threat;
			string targs, type;
			cd = val = threat = 0;
			targs = type = "";
			skillJson = skillJson.Substring (1);
			string[] list = skillJson.Split (',');
			Skill sk;
			foreach (string s in list) {

				string[] sp = s.Split (':');
				if (String.Equals(sp[0], "\"cooldown\"")) {
					cd = int.Parse (sp [1]);

				}
				if (sp [0].Equals ("\"value\"")) {
					val = int.Parse (sp [1].Substring(0,1));

				}
				if (sp [0].Equals ("\"threatGen\"")) {
					threat = int.Parse (sp [1]);
				
				}
				if (sp [0].Equals ("\"targets\"")) {
					string temp = sp [1].Substring (1);
					targs = temp.Substring (0, 3);
				
				}

				if (sp [0].Equals ("\"type\"")) {
					string temp = sp [1].Substring (1);
					type = temp.Substring (0, 3);
		
				}

			}
			sk = new Skill(skillName, cd, val, threat, targs, type);
			return sk;
		}

		void getSkills(ref Character ch, string sList, string pList) {
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

		void parseCharInfo(int mode, ref Characters ch) {
			string acc1, acc2, weapon, armor, wType, acc1Type, acc2Type;
			int attack, magic, defense, hp, exp;
			attack = magic = defense = hp = exp = 0;

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
				if (sp [0].Equals ("\"class\"")) {
					className = sp [1].Substring (1, sp [1].Length - 2);
					DoDebug (className);
				

				}
			}

		

			string[] nBlock = splJson [2].Split (',');
			foreach (string s in nBlock) {
				string[] sp = s.Split (':');
				if (String.Equals(sp[0], ",\"perks\"")) {
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
			ch = new Characters (className, attack, magic, defense, hp, exp);
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

		IEnumerable runAcc() {
			getAccJson ("fug");
			DoDebug("WAITING");
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			getCharList ();

		}

		IEnumerator runChar() {
			getCharInfo ("Example");
			//skillList = "Spin-Slash";
			//WAIT

			Characters x = null;
			DoDebug("WAITING");
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseCharInfo (0, ref x);

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




			DoDebug (skillList);

			int ind = 0;
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
			perkList = "Spin-Slash1";
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

			Characters chara = null;
			getCharInfo ("Example");
			DoDebug("WAITING ON CHAR INFO");
			yield return new WaitForSeconds (2f);
			DoDebug("DONE");
			parseCharInfo (1, ref chara);

			chara.setSkills (sTemp);
			chara.setAttack (chara.getAttack () + attackToAdd);
			chara.setMagic (chara.getMagic () + magicToAdd);
			chara.setDefense (chara.getDefense () + defenseToAdd);

			DoDebug("ATTACK: " + chara.getAttack() + "\nDefense: " + chara.getDefense());

		

		}

		void GetJson(Firebase sender, DataSnapshot snapshot) {
			tempJson = snapshot.RawJson;
		}



	}


}

