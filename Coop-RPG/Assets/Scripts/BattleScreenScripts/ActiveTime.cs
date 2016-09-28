using UnityEngine;
using System.Collections;

public class ActiveTime : MonoBehaviour {

    private float seconds;
    private float maxTime;
    private float enemySeconds;
    private float enemyMaxTime;

    void Start () {
        // maxTime is the amount of time a move needs to charge.
        seconds = 0;
        maxTime = 6;

        enemySeconds = 0;
        enemyMaxTime = 15;
	}
	
	void Update () {
        if (seconds < maxTime)
            seconds += Time.deltaTime;
        else
            seconds = maxTime;

        if (enemySeconds < enemyMaxTime)
            enemySeconds += Time.deltaTime;
        else
            enemySeconds = enemyMaxTime;
    }

    public float GetRatio()
    {
        return seconds / maxTime;
    }

    public float GetEnemyRatio()
    {
        return enemySeconds / enemyMaxTime;
    }

    public void setSeconds(float sec)
    {
        seconds = sec;
    }

    public void setEnemySeconds(float sec)
    {
        enemySeconds = sec;
    }
}
