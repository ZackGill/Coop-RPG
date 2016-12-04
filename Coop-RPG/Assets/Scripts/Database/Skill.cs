using System;
using System.Collections.Generic;
using System.Collections;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;


public class Skill
{
		
    string name, target, type;
    int value, cooldown, threat;
    string skillJson;

    public Skill(string n, int cd, int val, int threatGen, string targs, string t)
    {
        this.name = n;
        this.cooldown = cd;
        this.value = val;
        this.threat = threatGen;
        this.target = targs;
        this.type = t;


    }

    public void applyPerk(string t, int val)
    {
        if (t.Equals("damage"))
        {
            value += val;
        }

        if (t.Equals("cooldown"))
        {
            cooldown -= val;
        }


    }

    public string toString()
    {
        return "" + name + ": Value: " + value + ", Type: " + type;
    }



		public String getName() {
			return name;
		}

		public String getType() {
			return type;
		}

		public String getTargets() {
			return target;
		}

		public int getValue() {
			return value;
		}

		public int getThreatGen() {
			return threat;
		}

		public int getCooldown() {
			return cooldown;
		}

	}


