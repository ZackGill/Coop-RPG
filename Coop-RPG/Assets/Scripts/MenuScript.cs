using UnityEngine;
using System.Collections;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
    public class MenuScript : MonoBehaviour
    {

    public CharacterInfo character;

    public InputField LoginName, LoginPass, email;
        public GameObject loginCanvas, charactersCanvas, newCharCanvas;
        public CharacterInfo charToUse;
    public GameObject characterList;
    public GameObject characterButton;


    public InputField charName;
    public Dropdown classesSel;

    public Text error;

        static int debug_idx = 0;
        string userName, password;

        Firebase fire;

        // Use this for initialization
        void Start()
        {
        fire = Firebase.CreateNew("coop-rpg.firebaseio.com/Accounts", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
        // TODO: Firebase stuff here, mainly to set it up so once login or create is clicked, can do firebase stuff.
        // Also call character list stuff. Do as much as possible at a time, elminate waiting latter.
    }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void checkLogin()
    {
        // Do Character list stuff here if true
        if (loginCheck)
        {
            charactersCanvas.gameObject.SetActive(true);
            for(int i = 0; i < chars.Length; i++)
            {
                GameObject temp = (GameObject)Instantiate(characterButton, characterList.transform);
                temp.GetComponentInChildren<Text>().text = chars[i];
            }
        }
        else
        {
            error.text = "Error finding account. Did you type your password in correctly?";
        }

    }

    public void checkCreate()
    {

        if (createAccCheck)
        {
            StartCoroutine(createCharScreen());
        }
        else
        {
            error.text = err;
        }

    }

    bool loginCheck = false;
    string[] chars;
    string[] classes;
    bool createAccCheck = false;
    string err = "";
    public IEnumerator logIn()
    {

        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runAcc(LoginName.text, LoginPass.text));
        yield return new WaitForSeconds(7f);
        loginCheck = db.getLogOnOk();
        chars = db.getCharList();
        checkLogin();
    }

    public IEnumerator createAccount()
    {

        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runCreateAcc(LoginName.text, LoginPass.text, email.text));
        yield return new WaitForSeconds(5f);
        createAccCheck = db.createGood;
        err = db.error;
        checkCreate();
    }

        public void CreateClicked()
      {
        if (LoginName.text == null || LoginPass.text == null || LoginName.text.Length <= 0 || LoginPass.text.Length <= 0)
        {
            error.text = "Please Enter a Username and password";
            return;
        }
        email.text = "mail@mail.mail";
        error.text = "";
        StartCoroutine(createAccount());
    }

    public void playGo()
    {

        SceneManager.LoadScene("Lobby");
    }

    public void play()
    {
        StartCoroutine(chooseChar());
    }

    public IEnumerator chooseChar()
    {
        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runChar(charName.text));
        yield return new WaitForSeconds(20f);
        Characters temp = db.getChar();
        character.character = temp;
        character.charName = charName.text;
        playGo();
    }

    public void charScreen()
    {
        newCharCanvas.SetActive(true);
        loginCanvas.SetActive(false);
        foreach(string s in classes)
        {
            classesSel.options.Add(new Dropdown.OptionData(s));
        }

    }

    public void createAndPlay()
    {
        if(charName.text == null)
        {
            error.text = "Please enter in a name";
            return;
        }
        error.text = "";
        StartCoroutine(createChar());
    }

    public IEnumerator createChar()
    {
        print("Before getting the drop down");
        print(classesSel.value + "");
        string temp = classesSel.options[classesSel.value].text;
        temp = temp.Substring(1, temp.IndexOf('\"', 1)-1);
        print(temp);
        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runCreateChar(charName.text, temp, LoginName.text));
        yield return new WaitForSeconds(5f);
        play();
        // Put error checks here, will do later.
    }

    public IEnumerator createCharScreen()
    {
        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runClassDesc());
        yield return new WaitForSeconds(5f);
        classes = db.getClassDesc();
        charScreen();
    }

    public void LoginClicked()
    {
        if(LoginName.text == null || LoginPass.text == null || LoginName.text.Length == 0 || LoginPass.text.Length == 0)
        {
            error.text = "Please Enter Username and Password";
            return;
        }
        error.text = "";
        StartCoroutine(logIn());
    }

    }
