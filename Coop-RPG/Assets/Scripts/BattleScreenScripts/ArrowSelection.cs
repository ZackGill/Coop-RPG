using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Threading;
using UnityEngine.UI;

public class ArrowSelection : MonoBehaviour
{

    private int whichSkill;
    private int arrowPos = 1;
    private Characters character;
    private Monster[] enemies;
    private EnemyQuantity enemyQuantity;
    private BattleLogic battleLogic;
    private BattleScreenStates state;

    private Image playerImage;
    private SpriteRenderer arrow;
    private SpriteRenderer enemy1;
    private SpriteRenderer enemy2;
    private SpriteRenderer enemy3;

    // Use this for initialization
    void Start()
    {
        enemyQuantity = GetComponent<EnemyQuantity>();
        battleLogic = GetComponent<BattleLogic>();
        character = battleLogic.getCharacter();
        whichSkill = battleLogic.whichSkill;
        state = GetComponent<BattleScreenStates>();
        arrow = transform.FindChild("EnemyPanel/selectionarrow").GetComponent<SpriteRenderer>();
        enemy1 = transform.FindChild("EnemyPanel/Enemy").GetComponent<SpriteRenderer>();
        enemy2 = transform.FindChild("EnemyPanel/Enemy2").GetComponent<SpriteRenderer>();
        enemy3 = transform.FindChild("EnemyPanel/Enemy3").GetComponent<SpriteRenderer>();
        playerImage = transform.FindChild("PlayerInfo/PlayerImage").GetComponent<Image>();
        arrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        character = battleLogic.getCharacter();
        whichSkill = battleLogic.whichSkill;

        if (state.curState == BattleScreenStates.FightStates.PICKANENEMY)
        {
            if ((whichSkill >= 0 && character.getSkills()[whichSkill].getType() == "heal"))
            {
                selectPlayer();
            }
            else
            {
                selectEnemy();
            }
        }
        else
            arrow.enabled = false;
        enemies = battleLogic.getEnemies();
    }


    void selectEnemy()
    {
        int numEnemies = battleLogic.numEnemies;
        if (arrowPos == -1)
            arrowPos = numEnemies - 1;
        if (arrowPos == numEnemies)
            arrowPos = 0;
        if (arrowPos == 2 && enemies[2].getDead())
            arrowPos--;
        if (arrowPos == 1 && enemies[0].getDead())
            arrowPos--;
        if (arrowPos == 0 && enemies[1].getDead())
            arrowPos--;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            arrowPos--;
            if (numEnemies == 3 && arrowPos == 2 && enemies[2].getDead())
                arrowPos--;
            if (arrowPos == 1 && enemies[0].getDead())
                arrowPos--;
            if (numEnemies == 2 && arrowPos == 0 && enemies[1].getDead())
                arrowPos--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            arrowPos++;

        if (arrowPos == numEnemies)
            arrowPos = 0;
        if (arrowPos == -1)
            arrowPos = numEnemies - 1;

            if (numEnemies == 2 && arrowPos == 0 && enemies[1].getDead())
                arrowPos++;
            if (arrowPos == 1 && enemies[0].getDead())
                arrowPos++;
            if (numEnemies == 3 && arrowPos == 2 && enemies[2].getDead())
                arrowPos++;
        }

        drawArrow();
        arrow.enabled = true;
    }

    void selectPlayer()
    {
        Vector3 position = this.transform.position;
        position = new Vector3(playerImage.transform.position.x, playerImage.transform.position.y + 2.5f, playerImage.transform.position.z);
        arrow.transform.position = position;
        arrow.enabled = true;
    }

    void drawArrow()
    {

        Vector3 center1 = enemy1.bounds.center;
        Vector3 center2 = enemy2.bounds.center;
        Vector3 center3 = enemy3.bounds.center;
        Vector3 position = this.transform.position;
        if (arrowPos == 0)
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