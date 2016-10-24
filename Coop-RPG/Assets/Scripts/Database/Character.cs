using System;

namespace AssemblyCSharp
{
	public class Character
	{
		int attack, magic, defense, hp, exp;
		Skill[] sks;
		IFirebase fb, chara, inner;

		public Character (String name) {
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Characters");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});

			chara = fb.Child (name);
			hp = int.Parse(chara.Child ("hp").ToString());
			exp = int.Parse(chara.Child ("exp").ToString());
			String rpgclass = chara.Child("class").ToString();
			inner = chara.Child ("stats");
			attack = int.Parse(inner.Child ("attack").ToString());
			defense = int.Parse(inner.Child ("defense").ToString());
			magic = int.Parse(inner.Child ("magic").ToString());

			String skillNames = chara.Child ("skills").ToString();
			String perkNames = chara.Child ("perks").ToString();

			string[] skills = skillNames.Split (new char[] {','});
			string[] perks = perkNames.Split (new char[] {','});

			Skill a = new Skill (skills [0], perks [0]);

			sks = new Skill[1];
			sks [0] = a;


			inner = chara.Child ("equipment");
			String acc1, acc2, weapon, armor;

			weapon = inner.Child ("weapon").ToString();
			armor = inner.Child ("armor").ToString();
			acc1 = inner.Child ("acc1").ToString();
			acc2 = inner.Child ("acc2").ToString();

			addWeapon (weapon);
			addAcc (acc1);
			addAcc (acc2);

			defense += ItemLU.getArmor (armor);



			addClassVals (rpgclass);

		}

		private void addClassVals(String clName) {
			attack += ClassLU.getAttack (clName);
			magic += ClassLU.getMagic (clName);
			defense += ClassLU.getDefense (clName);

		}

		private void addWeapon(String name) {
			String wType = ItemLU.getWeapType (name);
			int wVal = ItemLU.getWeapValue (name);

			if (wType.Equals("magic")) {
				magic += wVal;
			}

			if (wType.Equals ("attack")) {
				attack += wVal;
			}


		}

		private void addAcc(String name) {
			String accStat = ItemLU.getAccStat (name);
			int accVal = ItemLU.getAccValue (name);

			if (accStat.Equals ("attack")) {
				attack += accVal;
			}

			if (accStat.Equals ("magic")) {
				magic += accVal;
			}

			if (accStat.Equals ("defense")) {
				defense += accVal;
			}

		}







	/*	public Character (string cName, int exp, int hp, int atk, int def, int mag, 
			string rpgclass, string weapon, string armor, string acc, string perks, string skills)
		{
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Characters");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

			}, (FirebaseError e) => {

			});
			chara = fb.Child (cName);
			chara.SetJsonValue("{\"EXP\" : " + exp + ", \"HP\" : "+ hp + ", \"class\" : \"" + 
				rpgclass + "\", \"stats\" : {\"attack\" : " + atk + ", \"defense\" : " +
				def + ", \"magic\" : " + mag + "}, \"skills\" : \"" + skills + "\", \"perks\" : \"" +
				perks + "\", \"equipment\" : {\"weapon\" : \"" + weapon + "\", \"armor\" : \"" +
				armor + "\", \"acceosry\" : \"" + acc + "\"}}");


		}*/
	}
}

