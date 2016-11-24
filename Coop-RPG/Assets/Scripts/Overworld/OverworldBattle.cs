using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using AssemblyCSharp;
public class OverworldBattle : NetworkBehaviour {

    public struct BattleInfo {
        // Many of the following variables were needed for testing pre-firebase and should be removed.
        public int numEnemies;
        public int numPlayers;
        // Flags so we don't attack more than once per turn.
        public bool enemyAttackFlag;
        // Information about the player.
        public float player0HP;
        public float player1HP;
        public float player2HP;
        // Information about the enemy.
        public float enemyHP;
        public float enemy2HP;
        public float enemy3HP;
        // Information about the battle itself.
        public string fightMessage;
        // Store Enemy seconds and max seconds
        public float enemy0Sec;
        public float enemy0Max;
        public float enemy1Sec;
        public float enemy1Max;
        public float enemy2Sec;
        public float enemy2Max;
    }

    [SyncVar]
    public BattleInfo info;


    public Monster enemy0;
    public Monster enemy1;
    public Monster enemy2;

    public BattleLogic battle0;
    public BattleLogic battle1;
    public BattleLogic battle2;

    private bool event0added = false;
    private bool event1added = false;
    private bool event2added = false;



    // Use this for initialization
    void Start () {


    }

    // Update is called once per frame
    void Update () {
        if (battle0 != null && !event0added)
        {
            battle0.EventPlayerDamage += PlayerDamage;
            battle0.EventEnemyDamage += EnemyDamage;
            event0added = true;
        }
        if (battle1 != null && !event1added)
        {
            battle1.EventPlayerDamage += PlayerDamage;
            battle1.EventEnemyDamage += EnemyDamage;
            event1added = true;
        }
        if (battle2 != null && !event2added)
        {
            battle2.EventPlayerDamage += PlayerDamage;
            battle2.EventEnemyDamage += EnemyDamage;
            event2added = true;
        }
    }

    public void PlayerDamage(float amount, int playerNum)
    {
        switch (playerNum)
        {
            case 0:
                info.player0HP -= amount;
                break;
            case 1:
                info.player1HP -= amount;
                break;
            case 2:
                info.player2HP -= amount;
                break;
            default:
                break;
        }

    }

    // variables are damage done.
    public void EnemyDamage(float enemy1HP, float enemy2HP, float enemy3HP)
    {
        info.enemyHP -= enemy1HP;
        info.enemy2HP -= enemy2HP;
        info.enemy3HP -= enemy3HP;


    }


    // CMD and RPC to update the info of the non-synch vars.




}
