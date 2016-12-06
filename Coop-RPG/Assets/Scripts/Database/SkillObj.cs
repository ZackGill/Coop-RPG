using System;
using UnityEngine;
namespace AssemblyCSharp
{
	[System.Serializable]
	public class SkillObj
	{
		int cooldown, value, threatGen;
		string targets, type;

		public SkillObj (int cd, int val, int threat, string targs, string t)
		{
			this.cooldown = cd;
			this.value = val;
			this.threatGen = threat;
			this.targets = targs;
			this.type = t;
			
			
		}

		public static SkillObj fromJson(string json) {
			return JsonUtility.FromJson<SkillObj> (json);
		}

		public int getCD() {
			return cooldown;
		}

		public int getValue() {
			return value;
		}
	}
}

