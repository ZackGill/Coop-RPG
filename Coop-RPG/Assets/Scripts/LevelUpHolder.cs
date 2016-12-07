using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LevelUpHolder : MonoBehaviour {
    public GameObject perkOptionFab;

    public Text victoryText;

    public Button perkOption;
    public Button perk2Option;
    public Button perk3Option;

	// Use this for initialization
	void Start () {
        victoryText.text = "You Cleared the Dungeon! Did you Level Up?";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
