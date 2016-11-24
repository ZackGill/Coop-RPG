using UnityEngine;
using System.Collections;

public class BattleHolderScript : MonoBehaviour {
    public GameObject player;
	// Use this for initialization
	void Start () {
        Invoke("die", 5f);
	}
	
    void die()
    {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
