﻿using UnityEngine;
using System.Collections;

public class Entity {

	int level;
	string charClass;
	int attack, defense, magic;
	int hpMax, hpCurrent;
	string entityName;
	CombatMove[] moveList = new CombatMove[3];
	//Skill[] skills = new Skill[3];
	//Perk[] perks = new Perk[3];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetLevel() {
		return level;
	}

	public void SetLevel(int level) {
		this.level = level;
	}

	public string GetCharClass() {
		return charClass;
	}

	public void SetCharClass(string charClass) {
		this.charClass = charClass;
	}

	public int GetAttack() {
		return attack;
	}

	public void SetAttack(int attack) {
		this.attack = attack;
	}
		
	public int GetDefense() {
		return defense;
	}

	public void SetDefense(int defense) {
		this.defense = defense;
	}

	public int GetMagic() {
		return magic;
	}

	public void SetMagic(int magic) {
		this.magic = magic;
	}

	public int GetHPMax() {
		return hpMax;
	}

	public void SetHPMax(int hpmax) {
		this.hpMax = hpmax;
	}

	public int GetHPCurrent() {
		return hpCurrent;
	}

	public void SetHPCurrent(int hpcurrent) {
		this.hpCurrent = hpcurrent;
	}

	public string GetEntityName() {
		return entityName;
	}

	public void SetEntityName(string entityName) {
		this.entityName = entityName;
	}

	public CombatMove[] GetMoveList() {
		return moveList;
	}

	public void SetMoveList(CombatMove move, int slot) {
		this.moveList[slot] = move;
	}
		
	public void SetMoveList(CombatMove[] moves) {
		this.moveList = moves;
	}
	/*
	public Skill[] GetSkills() {
		return skills;
	}

	public void SetSkills(Skill skill, int slot) {
		this.skills[slot] = skill;
	}

	public void SetSkills(Skills[] skills) {
		this.skills = skills;
	}

	public Perk[] GetPerks() {
		return perks;
	}

	public void SetPerks(Perk perk, int slot) {
		this.perks[slot] = perk;
	}

	public void SetPerks(Perk[] perks) {
		this.perks = perks;
	}
	*/
}
