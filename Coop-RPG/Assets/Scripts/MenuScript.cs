using UnityEngine;
using System.Collections;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine.UI;
namespace AssemblyCSHarp
{
    public class MenuScript : MonoBehaviour
    {

        public InputField LoginName, LoginPass, createName, createPass;
        public Canvas createCanvas, loginCanvas, charactersCanvas, newCharCanvas;
        public CharacterInfo charToUse;

        static int debug_idx = 0;
        string userName, password;

        // Use this for initialization
        void Start()
        {
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
}