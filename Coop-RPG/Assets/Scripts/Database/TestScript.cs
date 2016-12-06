/*
Copyright 2015 Google Inc. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
﻿using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	/*
	IFirebase firebase;
	IFirebase child;
	IFirebase test;
	// Use this for initialization
	void Start () {
		firebase = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Accounts");
		firebase.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {
			Debug.Log ("auth success!!" + auth.Uid);
		}, (FirebaseError e) => {
			Debug.Log ("auth failure!!");
		});

		string f = firebase.Child ("Accounts").ToString();
		Debug.Log(f);

		firebase.ChildAdded += (object sender, FirebaseChangedEventArgs e) => {
			Debug.Log ("Child added!");
		};

		firebase.ChildRemoved += (object sender, FirebaseChangedEventArgs e) => {
			Debug.Log ("Child removed!");
		};
			

		test = firebase.Child("accBUILD");
		test.SetJsonValue ("{\"characters\" : \"TheDoo222d\", \"email\" : \"mail@mail.mail\", \"password\" : \"FUG\"}");
		child = test.Child ("characters");
		Debug.Log (child.ToString ());

	}

	// Update is called once per frame
	void Update () {
	
	}
*/
}
