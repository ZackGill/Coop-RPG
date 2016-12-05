using System;
using UnityEngine;

namespace AssemblyCSharp{
    [Serializable]
	public class Characters
	{
		int attack, magic, defense, hp, exp, level;
		float threatLevel;
		string clName;
		Skill[] skills;
		string name;

		public Characters(string n, string cl, int atk, int mg, int def, int hp, int exp, int lvl) {
			clName = cl;
			attack = atk;
			magic = mg;
			defense = def;
			this.hp = hp;
			this.exp = exp;
			level = lvl;
			threatLevel = 0f;
			name = n;


		}

		public Characters() {
			clName = "";
			name = "";
			attack = 0;
			magic = 0;
			defense = 0;
			hp = 0;
			exp = 0;
			skills = null;
			level = 0;
			threatLevel = 0f;
		}

		public int getAttack() {
			return attack;
		}

		public void setAttack(int atk) {
			attack = atk;
		}

		public int getMagic() {
			return magic;
		}

		public void setMagic(int mg) {
			magic = mg;
		}
		public int getDefense() {
			return defense;
		}

		public void setDefense(int def) {
			defense = def;
		}

		public int getExp() {
			return exp;
		}

		public void setExp(int exp) {
			this.exp = exp;
		}

		public int getHP() {
			return hp;
		}

		public void setHP(int hp) {
			this.hp = hp;
		}

		public Skill[] getSkills() {
			return skills;
		}

		public void setSkills(Skill[] sk) {
			skills = sk;

		}

		public void setLevel(int lvl) {
			level = lvl;
		}

		public int getLevel() {
			return level;
		}

		public float getThreat() {
			return threatLevel;
		}

		public void setThreat(float t) {
			threatLevel = t;
		}

		public void addThreat(float toAdd) {
			threatLevel += toAdd;
		}

		public string getClass() {
			return clName;
		}

		public string getName() {
			return name;
		}

	}

}

