using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class DatabaseBattle : MonoBehaviour {

    private Characters character;
    private Monster enemy;
    DatabaseManager db = new DatabaseManager();

    // Use this for initialization
    void Start () {

        character = GetComponent<Characters>();
        enemy = new Monster(5, 1, 1, 1, 1, false, 1, 1, 1);
        StartCoroutine(wait());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator wait()
    {
        character = null;
        StartCoroutine(db.runChar("Lex"));
        //StartCoroutine(db.runMon("Squawktopus"));
        yield return new WaitForSeconds(35f);
        character = db.getChar();
    }

    public Characters getCharacter()
    {
        return character;
    }


}
