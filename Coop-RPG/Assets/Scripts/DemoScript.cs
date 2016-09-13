using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour {

    public Text hello;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        hello.text = "Hello World";

        print("This is console printing");
	}
}
