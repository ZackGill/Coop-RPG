using System.Collections.Generic;
using System.Collections;
using System;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Account : MonoBehaviour
	{

        string uName, email, password;


		/*
		string name, email, password;
		IFirebase fb, acc;
		public Account (string name, string email, string password)
		{
			fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Accounts");
			fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {
				
			}, (FirebaseError e) => {
				
			});

			acc = fb.Child (name);
			acc.SetJsonValue ("{\"characters\" : \"NONE\", \"email\" : \"" + 
				email + "\", \"password\" : \"" + password + "\"}");


			this.name = name;
			this.email = email;
			this.password = password;
		}

		public string getChars() {
			string ret = acc.Child ("characters").ToString();

			return ret;
		}
		*/
	}
}

