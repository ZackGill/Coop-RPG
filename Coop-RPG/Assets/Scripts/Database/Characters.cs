using System;

namespace AssemblyCSharp
{
	public class Characters
	{
		IFirebase fb, chara, sub;
		string[] charas;
		public Characters (string cList)
		{
		///	charas = cList.Split (",");
		}

		public Characters (int index) {
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Characters");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});

			string cName = charas[index];
			chara = fb.Child(cName);

			int exp, hp, atk, def, mag;
			string rpgclass, weapon, armor, acc, perks, skills;

			exp = int.Parse(chara.Child("EXP").ToString());
			hp = int.Parse(chara.Child("HP").ToString());
		///	rpgclass = int.Parse(chara.Child("class").ToString());
			sub = chara.Child("Stats");
			atk = int.Parse(sub.Child("attack").ToString());
			def = int.Parse(sub.Child("defense").ToString());
			mag = int.Parse(sub.Child("magic").ToString());
			sub = chara.Child("equipment");
			weapon = sub.Child("weapon").ToString();
			armor = sub.Child("armor").ToString();
			acc = sub.Child("accesory").ToString();



		

			/// Character(cName, exp, hp, atk, def, mag, rpgclass, weapon,
			///	armor, acc, perks, skills);




		}
	}
}

