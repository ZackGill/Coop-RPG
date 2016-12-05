using UnityEngine;
using System.Collections;
using AssemblyCSharp;
public class LoadingImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(!LoadingScript.Instance.loading)
        {
            Network.Destroy(gameObject);
        }
	}
}
