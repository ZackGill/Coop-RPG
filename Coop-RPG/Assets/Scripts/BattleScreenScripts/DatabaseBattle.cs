using UnityEngine;
using System.Collections;
using AssemblyCSharp;
public class DatabaseBattle : MonoBehaviour {

    private Characters character;
    DatabaseManager db = new DatabaseManager();

    // Use this for initialization
    void Start () {

        character = GetComponent<Characters>();
        StartCoroutine(wait());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator wait()
    {
        character = null;
        StartCoroutine(db.runChar("Lex"));
        yield return new WaitForSeconds(35f);
        character = db.getChar();
    }

    public Characters getCharacter()
    {
        return character;
    }
}
