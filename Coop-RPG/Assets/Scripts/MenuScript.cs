using UnityEngine;
using System.Collections;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;
    public class MenuScript : MonoBehaviour
    {

        public InputField LoginName, LoginPass, email;
        public Canvas loginCanvas, charactersCanvas, newCharCanvas;
        public CharacterInfo charToUse;
    public GameObject characterList;
    public GameObject characterButton;


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

    public void Play()
    {

    }

    public void checkCreate()
    {

        if (createAccCheck)
        {
            // Show character Creation stuff
        }
        else
        {
            error.text = err;
        }

    }

    bool loginCheck = false;
    string[] chars;

    bool createAccCheck = false;
    string err = "";
    public IEnumerator logIn()
    {

        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runAcc(LoginName.text, LoginPass.text));
        yield return new WaitForSeconds(5f);
        loginCheck = db.getLogOnOk();
        chars = db.getCharList();
        checkLogin();
    }

    public IEnumerator createAccount()
    {

        DatabaseManager db = new DatabaseManager();
        StartCoroutine(db.runCreateAcc(LoginName.text, LoginPass.text, email.text));
        yield return new WaitForSeconds(5f);
        createAccCheck = db.createAccGood();
        err = db.error;
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
