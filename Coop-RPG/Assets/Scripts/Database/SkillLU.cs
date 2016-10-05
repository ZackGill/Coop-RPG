using System;

namespace AssemblyCSharp
{
	public static class SkillLU
	{
		
		public static int getCD(String name) {
			int ret = 0;
			IFirebase fb, sk;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/SkillLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			sk = fb.Child (name);
			ret = int.Parse(sk.Child ("cooldown").ToString ());

			return ret;
		}

		public static int getValue(String name) {
			int ret = 0;
			IFirebase fb, sk;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/SkillLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			sk = fb.Child (name);
			ret = int.Parse(sk.Child ("value").ToString());

			return ret;
		}

		public static int getThreat(String name) {
			int ret = 0;
			IFirebase fb, sk;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/SkillLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			sk = fb.Child (name);
			ret = int.Parse(sk.Child ("threatGen").ToString());

			return ret;
		}

		public static String getType(String name) {
			String ret = "";
			IFirebase fb, sk;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/SkillLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			sk = fb.Child (name);
			ret = sk.Child ("type").ToString();

			return ret;
		}

		public static String getTargets(String name) {
			String ret = "";
			IFirebase fb, sk;
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/SkillLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			sk = fb.Child (name);
			ret = sk.Child ("targets").ToString();

			return ret;
		}



		/*
		IFirebase fb, skill;
		public string [] get (string name)
		{
			
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/SkillLU");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});

			skill = fb.Child (name);
			string[] ret = new string[5];
			ret [0] = skill.Child ("cooldown").ToString ();
			ret [1] = skill.Child ("targets").ToString ();
			ret [2] = skill.Child ("threatGen").ToString ();
			ret [3] = skill.Child ("type").ToString();
			ret [4] = skill.Child ("value").ToString ();

			return ret;

		}
		*/
	}
}

