using UnityEngine;
using System.Collections;

public class ArrowSelection : MonoBehaviour {

    private int arrowPos = 1;
    private EnemyQuantity enemyQuantity;
    private BattleLogic battleLogic;
    private BattleScreenStates state;

    private SpriteRenderer arrow;
    private SpriteRenderer enemy1;
    private SpriteRenderer enemy2;
    private SpriteRenderer enemy3;

    // Use this for initialization
    void Start () {
        enemyQuantity = GetComponent<EnemyQuantity>();
        battleLogic = GetComponent<BattleLogic>();
        state = GetComponent<BattleScreenStates>();
        arrow = transform.FindChild("EnemyPanel/selectionarrow").GetComponent<SpriteRenderer>();
        enemy1 = transform.FindChild("EnemyPanel/Enemy").GetComponent<SpriteRenderer>();
        enemy2 = transform.FindChild("EnemyPanel/Enemy2").GetComponent<SpriteRenderer>();
        enemy3 = transform.FindChild("EnemyPanel/Enemy3").GetComponent<SpriteRenderer>();
        arrow.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (state.curState == BattleScreenStates.FightStates.PICKANENEMY)
            selectEnemy();
        else
            arrow.enabled = false;
    }

    void selectEnemy()
    {
        int numEnemies = battleLogic.numEnemies;
        arrow.enabled = true;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            arrowPos--;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            arrowPos++;
        if (arrowPos == numEnemies)
            arrowPos = 0;
        if (arrowPos == -1)
            arrowPos = numEnemies - 1;
       
        drawArrow();
    }

    void drawArrow()
    {

        Vector3 center1 = enemy1.bounds.center;
        Vector3 center2 = enemy2.bounds.center;
        Vector3 center3 = enemy3.bounds.center;
        Vector3 position = this.transform.position;
        if(arrowPos == 0)
            position = new Vector3(center2.x, center2.y + 2.5f, center2.z);
        if (arrowPos == 1)
            position = new Vector3(center1.x, center1.y + 2.5f, center1.z);
        if (arrowPos == 2)
            position = new Vector3(center3.x, center3.y + 2.5f, center3.z);
        arrow.transform.position = position;
    }

    public int getArrowPos()
    {
        return arrowPos;
    }
}
