using UnityEngine;
using System.Collections;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;
    public class MenuScript : MonoBehaviour
    {

        public InputField LoginName, LoginPass, createName, createPass;
        public Canvas createCanvas, loginCanvas, charactersCanvas, newCharCanvas;
        public CharacterInfo charToUse;

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

        public void createFailed()
        {


        }

        public void createWorks()
        {


        }

        public void loginFailed()
        {


        }

        public void loginGood()
        {

        }

    }
