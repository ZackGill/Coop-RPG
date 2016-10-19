using System;

namespace AssemblyCSharp
{
	public class Monster
	{
		IFirebase fb, mon, sub;
		double AImistake;
		bool bossTag;
		int level, atk, def, mag, AIsight, hp;
///		Perk[] perks;
///		Skill[] skills;
		public Monster (string name)
		{
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Enemies");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});

			mon = fb.Child (name);
		
			hp = int.Parse(mon.Child("HP").ToString());
			level = int.Parse (mon.Child ("level").ToString ());
			bossTag = bool.Parse (mon.Child ("bossTag").ToString ());
			sub = mon.Child("Stats");
			atk = int.Parse(sub.Child("attack").ToString());
			def = int.Parse(sub.Child("defense").ToString());
			mag = int.Parse(sub.Child("magic").ToString());
			sub = mon.Child("AI");
			AIsight = int.Parse(sub.Child("sightrange").ToString());
			AImistake = double.Parse (sub.Child ("mistakeChance").ToString ());

			///perks
			/// skills


		}
	}
}

