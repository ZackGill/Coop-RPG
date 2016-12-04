using System;
using System.Collections.Generic;
using SimpleFirebaseUnity;
using UnityEngine;
namespace AssemblyCSharp
{

    public class Monster
    {
        bool isDead = false;
        float mistakeChance;
        int attack, magic, defense, hp, level, sightRange;
        bool bossTag = false;
        float moveSpeed;
        Skill[] skills;


        public Monster(int atk, int mag, int def, int hp, int lvl, bool bt, int sr, float mis, float ms)
        {
            attack = atk;
            magic = mag;
            defense = def;
            this.hp = hp;
            level = lvl;
            sightRange = sr;
            mistakeChance = mis;
            bossTag = bt;
            moveSpeed = ms;


        }

        public void setMS(float ms)
        {
            moveSpeed = ms;
        }

        public void setMistakeChance(float mis)
        {
            mistakeChance = mis;
        }

        public void setSightRange(int sr)
        {
            sightRange = sr;
        }

        public int getSightRange()
        {
            return sightRange;
        }

        public float getMS()
        {
            return moveSpeed;
        }

        public int getHP()
        {
            return hp;
        }

        public int getAttack()
        {
            return attack;
        }

        public int getMagic()
        {
            return magic;
        }

        public bool getBossTag()
        {
            return bossTag;
        }

        public int getLevel()
        {
            return level;
        }

        public float getMistakeChance()
        {
            return mistakeChance;
        }

        public int getDefense()
        {
            return defense;
        }

        public Skill[] getSkills()
        {
            return skills;
        }

        public void setSkills(Skill[] sk)
        {
            skills = sk;

        }
        public void setDead(bool dead)
        {
            isDead = dead;
        }
        public bool getDead()
        {
            return isDead;
        }
    }
}