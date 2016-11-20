using System;

namespace AssemblyCSharp
{
	public class Characters
	{
		int attack, magic, defense, hp, exp, level;
		string clName;
		Skill[] skills;

		public Characters(string cl, int atk, int mg, int def, int hp, int exp, int lvl) {
			clName = cl;
			attack = atk;
			magic = mg;
			defense = def;
			this.hp = hp;
			this.exp = exp;
			level = lvl;


		}

		public Characters() {
			clName = "";
			attack = 0;
			magic = 0;
			defense = 0;
			hp = 0;
			exp = 0;
			skills = null;
			level = 0;

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


	}
}

