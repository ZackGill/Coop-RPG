using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Characters
	{
		int attack, magic, defense, hp, exp;
		string clName;
		Skill[] skills;

		public Characters(string cl, int atk, int mg, int def, int hp, int exp) {
			clName = cl;
			attack = atk;
			magic = mg;
			defense = def;
			this.hp = hp;
			this.exp = exp;


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

        public string getClassName()
        {
            return clName;
        }

        public void setClassName(string temp)
        {
            clName = temp;
        }
	}
}

