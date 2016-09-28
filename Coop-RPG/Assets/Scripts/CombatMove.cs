using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CombatMove : MonoBehaviour {

	Entity _caster;
	List<Entity> _targets = new List<Entity>();
	string _moveType;
	int _speed, _cooldown;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Getters
	Entity GetCaster() {
		return _caster;
	}
		
	List<Entity> GetTargets() {
		return _targets;
	}

	public string GetMoveType() {
		return _moveType;
	}

	int GetSpeed() {
		return _speed;
	}

	int GetCooldown() {
		return _cooldown;
	}

	// Setters
	void SetCaster(Entity c) {
		_caster = c;
	}

	void SetTargets(List<Entity> tar) {
		foreach (Entity e in tar) {
			_targets.Add (e);
		}
	}

	void SetMoveType(string str) {
		_moveType = str;
	}

	void SetSpeed(int s) {
		_speed = s;
	}

	void SetCooldown(int cd) {
		_cooldown = cd;
	}
}
