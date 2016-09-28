using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using UnityEngine.UI;

public class DemoScript : MonoBehaviour {
    public Text hello;
    // Use this for initialization
    void Start () {
    }
    // Update is called once per frame
    void Update () {
        hello.text = "It is " + DateTime.Now.ToString("h:mm:ss tt");
        print("This is console printing");
    }
}