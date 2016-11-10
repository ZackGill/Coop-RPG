using System;
using System.Collections.Generic;
using SimpleFirebaseUnity;
namespace AssemblyCSharp
{
	public class Monster
	{
<<<<<<< HEAD
		Dictionary<string, string> vals = new Dictionary<string, string> (); //need to cast vars as ints when we pull them out

		public void getVals() {
			//get all values in one call and assign to dict
			//FirebaseQueue q = new FirebaseQueue ();
			//Firebase fb = Firebase.CreateNew ("coop-rpg.firebaseio.com/Enemies", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
			//Firebase enemy;
			//enemy = fb.Child ("exampleMon");
			//vals.OnGetSuccess += GetEnemyHandler;
			//vals.OnGetFailed += GetEnemyHandlerBAD;
		//	q.AddQueueGet(enemy);
		}
			
		/*
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
=======
		float mistakeChance;
		int attack, magic, defense, hp, level, sightRange;
		bool bossTag = false;
		float moveSpeed;

		Skill[] skills;
		

		public Monster(int atk, int mag, int def, int hp, int lvl, bool bt, int sr, float mis, float ms) {
			attack = atk;
			magic = mag;
			defense = def;
			this.hp = hp;
			level = lvl;
			sightRange = sr;
			mistakeChance = mis;
			bossTag = bt;
			moveSpeed = ms;
>>>>>>> refs/remotes/origin/Cameron
		

		}

		public void setMS(float ms) {
			moveSpeed = ms;
		}

		public void setMistakeChance(float mis) {
			mistakeChance = mis;
		}

		public void setSightRange(int sr) {
			sightRange = sr;
		}

		public int getSightRange() {
			return sightRange;
		}

		public float getMS() {
			return moveSpeed;
		}

		public int getHP() {
			return hp;
		}

		public int getAttack() {
			return attack;
		}

		public int getMagic() {
			return magic;
		}

		public bool getBossTag() {
			return bossTag;
		}

		public int getLevel() {
			return level;
		}

		public float getMistakeChance() {
			return mistakeChance;
		}

		public int getDefense() {
			return defense;
		}

		public Skill[] getSkills() {
			return skills;
		}

		public void setSkills(Skill[] sk) {
			skills = sk;

		}
	}
}

