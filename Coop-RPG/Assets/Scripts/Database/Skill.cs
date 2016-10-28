using System;
using System.Collections.Generic;
using System.Collections;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Skill : MonoBehaviour
	{
		static int debug_idx = 0;
		string name, target, type;
		int value, cooldown, threat;
		string skillJson;

		[SerializeField]
		TextMesh txt;

		/*
		public Skill(string name) {
			this.name = name;


		
		}
		*/

		void Start() {
			StartCoroutine (Run ());
		}

		void DoDebug(string str)
		{
			Debug.Log(str);
			if (txt != null)
			{
				txt.text += (++debug_idx + ". " + str) + "\n";
			}
		}
		void GetSkill(Firebase sender, DataSnapshot snapshot) {
			skillJson = snapshot.RawJson;
		}

		void getJson(string name) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/SkillLU", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase skill = fb.Child (name);
			skill.OnGetSuccess += GetSkill;
			skill.GetValue ();

		}

		void parseJson() {
			int cd, val, threat;
			string targs, type;
			skillJson = skillJson.Substring (1);
			string[] list = skillJson.Split (',');
			foreach (string s in list) {
				DoDebug (s);
				string[] sp = s.Split (':');
				if (String.Equals(sp[0], "\"cooldown\"")) {
					cd = int.Parse (sp [1]);
					DoDebug ("CD FUG: " + cd);
				}
				if (sp [0].Equals ("\"value\"")) {
					val = int.Parse (sp [1].Substring(0,1));
					DoDebug ("VAL: " + val);
				}
				if (sp [0].Equals ("\"threatGen\"")) {
					threat = int.Parse (sp [1]);
					DoDebug ("THR: " + threat);
				}
				if (sp [0].Equals ("\"targets\"")) {
					string temp = sp [1].Substring (1);
					targs = temp.Substring (0, 3);
					DoDebug ("TRGS: " + targs);
				}

				if (sp [0].Equals ("\"type\"")) {
					string temp = sp [1].Substring (1);
					type = temp.Substring (0, 3);
					DoDebug ("TYPE: " + type);
				}
			}
		}

		IEnumerator Run() {
			getJson ("Spin-Slash");

			DoDebug("WAITING");
			yield return new WaitForSeconds (4f);
			DoDebug("DONE");

			DoDebug (skillJson);
			parseJson ();
		}

		/*
		String targets, type, name;
		int value, threatGen, cooldown;
		public Skill (String name, String p)
		{
			this.name = name;
			targets = SkillLU.getTargets (name);
			type = SkillLU.getType (name);
			value = SkillLU.getValue (name);
			threatGen = SkillLU.getThreat (name);
			cooldown = SkillLU.getCD (name);

	
			String pType = PerkLU.getType (p);
			int pVal = PerkLU.getValue (p);

			if (pType.Equals ("damage")) {
				value += pVal;
				value++;
			}
			
		}


		public String getName() {
			return name;
		}

		public String getType() {
			return type;
		}

		public String getTargets() {
			return targets;
		}

		public int getValue() {
			return value;
		}

		public int getThreatGen() {
			return threatGen;
		}

		public int getCooldown() {
			return cooldown;
		}
		*/
	}
}

