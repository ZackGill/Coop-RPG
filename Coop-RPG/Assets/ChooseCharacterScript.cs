using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using SimpleFirebaseUnity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChooseCharacterScript : MonoBehaviour {
    public CharacterInfo character;
    public Text charName;
    // Use this for initialization
    void Start() {
        character = GameObject.Find("CharacterInfo").GetComponent<CharacterInfo>();
    }

    // Update is called once per frame
    void Update() {

    }

    void play()
    {
        SceneManager.LoadScene("Lobby");
    }

    public IEnumerator chooseChar()
    {
        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runChar(charName.text));
        yield return new WaitForSeconds(20f);
        Characters temp = db.getChar();
        character.character = temp;
        character.charName = charName.text;
        play();
    }

    public void Click()
    {
        print("MouseDown");
        StartCoroutine(chooseChar());
    }
}
