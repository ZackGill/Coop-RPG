using UnityEngine;
using System.Collections;
using AssemblyCSharp;
public class CharacterInfo : MonoBehaviour {

    /*
     * Script used to hold data on a selected character. This is the character that will be used in the game itself.
     */

    public Characters character;
    public string charName;


   /* public int attack, magic, defense, hp, exp;
    public string clName;
    public Sprite charSprite;
    //string itemJson;
    public string acc1, acc2, weapon, armor, wType, acc1Type, acc2Type;
    public Skill[] sks;*/
   // [SerializeField]
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
