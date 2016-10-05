using System;

namespace AssemblyCSharp
{
	public class Skill
	{
		String targets, type;
		int value, threatGen, cooldown;
		public Skill (String name, String p)
		{

			targets = SkillLU.getTargets (name);
			type = SkillLU.getType (name);
			value = SkillLU.getValue (name);
			threatGen = SkillLU.getThreat (name);
			cooldown = SkillLU.getCD (name);

	
			String pType = PerkLU.getType (p);
			int pVal = PerkLU.getValue (p);

			if (pType.Equals ("damage")) {
				value++;
			}
			
		}

		public String getType() {
			return type;
		}

		public String getTargets() {
			return targets;
		}

		public int getValue() {
			return value;
		}

		public int getThreatGen() {
			return threatGen;
		}

		public int getCooldown() {
			return cooldown;
		}
	}
}

