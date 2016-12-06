using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LevelUpHolder : MonoBehaviour {
    public GameObject perkOptionFab;

    public Text victoryText;

    public GameObject perkOption;
    public GameObject perk2Option;
    public GameObject perk3Option;

	// Use this for initialization
	void Start () {
        victoryText.text = "You Cleared the Dungeon! Did you Level Up?";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
