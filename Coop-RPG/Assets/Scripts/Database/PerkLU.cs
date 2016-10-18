using System;

namespace AssemblyCSharp
{
	public static class PerkLU
	{
		
		public static String getType(String name) {
			String ret = "";
			IFirebase fb, p;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/PerkLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});

			p = fb.Child (name);
			ret = p.Child ("type").ToString ();

			return ret;


		}

		public static int getValue(String name) {
			int ret = 0;
			IFirebase fb, p;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/PerkLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});

			p = fb.Child (name);
			ret = int.Parse(p.Child ("value").ToString ());

			return ret;


		}



		/*
		IFirebase fb, perk;
		public string[] get (string name)
		{
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/PerkLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});

			perk = fb.Child (name);
			string[] ret = new string[2];
			ret [0] = perk.Child ("type").ToString();
			ret [1] = perk.Child ("value").ToString ();

			return ret;


		}
*/
	}
}

