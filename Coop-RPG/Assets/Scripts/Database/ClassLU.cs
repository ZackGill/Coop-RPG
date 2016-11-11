using System;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
namespace AssemblyCSharp
{
	public static class ClassLU
	{
		public static int getAttack(string name) {
			int ret = 0;
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Classes", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase inner = fb.Child ("baseStats").Child ("attack");
			/*inner.OnGetSuccess += (async (Firebase sender, DataSnapshot snap)=>{
				ret = int.Parse(snap.RawValue);

			};*/
				
			inner.OnGetSuccess += (Firebase sender, DataSnapshot snapshot)=>{
				ret = int.Parse(snapshot.RawJson);
				};
			inner.GetValue ();
			return ret;
		}

		public static int getMagic(string name) {
			int ret = 0;
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Classes", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase inner = fb.Child ("baseStats").Child ("magic");
			/*inner.OnGetSuccess += (async (Firebase sender, DataSnapshot snap)=>{
				ret = int.Parse(snap.RawValue);

			};*/

			inner.OnGetSuccess += (Firebase sender, DataSnapshot snapshot)=>{
				ret = int.Parse(snapshot.RawJson);
			};
			inner.GetValue ();
			return ret;
		}

		public static int getDefense(string name) {
			int ret = 0;
			Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Classes", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			Firebase inner = fb.Child ("baseStats").Child ("defense");
			/*inner.OnGetSuccess += (async (Firebase sender, DataSnapshot snap)=>{
				ret = int.Parse(snap.RawValue);

			};*/

			inner.OnGetSuccess += (Firebase sender, DataSnapshot snapshot)=>{
				ret = int.Parse(snapshot.RawJson);
			};
			inner.GetValue ();
			return ret;
		}



	

		/*
		public static int getAttack(String name) {
			int ret = 0;
			IFirebase fb, stat, cl;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Classes");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			cl = fb.Child (name);
			stat = cl.Child ("baseStats");
			ret = int.Parse(stat.Child ("attack").ToString());
			return ret;
		}

		public static int getMagic(String name) {
			int ret = 0;
			IFirebase fb, stat, cl;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Classes");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			cl = fb.Child (name);
			stat = cl.Child ("baseStats");
			ret = int.Parse(stat.Child ("magic").ToString());
			return ret;
		}

		public static int getDefense(String name) {
			int ret = 0;
			IFirebase fb, stat, cl;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Classes");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			cl = fb.Child (name);
			stat = cl.Child ("baseStats");
			ret = int.Parse(stat.Child ("defense").ToString());
			return ret;
		}

		public static String getPerks(String name, int level) {
			String ret = "";
			IFirebase fb, stat, cl;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Classes");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			cl = fb.Child (name);
			stat = cl.Child ("perks");
			ret = stat.Child (level.ToString()).ToString ();

			return ret;
		}
		*/
	}
}

