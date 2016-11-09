using System.Collections.Generic;
using System.Collections;
using System;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ClassObj
	{
		int attack, defense, magic;
		//Skills

		void GetATK(Firebase sender, DataSnapshot snapshot) {
			attack = int.Parse (snapshot.RawJson);
		}

		void GetMAG(Firebase sender, DataSnapshot snapshot) {
			magic = int.Parse (snapshot.RawJson);
		}

		void GetDEF(Firebase sender, DataSnapshot snapshot) {
			defense = int.Parse (snapshot.RawJson);
		}


		public ClassObj (string name)
		{
			attack = defense = magic = 0;
			getVals (name);
		}

		private void getVals(string name) {
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Classes", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase inner = fb.Child(name).Child ("baseStats");
			Firebase aFB = inner.Child ("attack");
			Firebase mFB = inner.Child ("magic");
			Firebase dFB = inner.Child ("defense");

			aFB.OnGetSuccess += GetATK;
			mFB.OnGetSuccess += GetMAG;
			dFB.OnGetSuccess += GetDEF;
			aFB.GetValue ();
			mFB.GetValue ();
			dFB.GetValue ();
		}






		public int getAtk() {
			return attack;
		}

		public int getDef() {
			return defense;
		}

		public int getMag() {
			return magic;
		}


	}
}

