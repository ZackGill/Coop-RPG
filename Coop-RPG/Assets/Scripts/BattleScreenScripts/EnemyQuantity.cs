using UnityEngine;
using System.Collections;

public class EnemyQuantity : MonoBehaviour {

    int numberOfEnemies;

    // Use this for initialization
    void Start () {
        numberOfEnemies = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getNumberOfEnemies()
    {
        return numberOfEnemies;
    }
}
