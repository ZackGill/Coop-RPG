using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using AssemblyCSharp;
public class OverworldBattle : NetworkBehaviour {
    [System.Serializable]
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

    [SyncVar(hook="OnInfo")]
    public BattleInfo info;

    public Monster enemy0;
    public Monster enemy1;
    public Monster enemy2;

    public BattleLogic battle0;
    public BattleLogic battle1;
    public BattleLogic battle2;

    public bool event0added = false;
    public bool event1added = false;
    public bool event2added = false;

    public void OnInfo(BattleInfo value)
    {
        Debug.Log("Hook called: OldPlayerHealth=" + info.player0HP + "New value=" + value.player0HP);
        info = value;
    }

    // Use this for initialization
    void Start () {


    }

    // Update is called once per frame
    void Update () {

    }
    [Command]
    public void CmdPlayerDamage(float amount, int playerNum)
    {
        BattleInfo temp = info;
        print("PlayerDamage");
        switch (playerNum)
        {
            case 0:
                temp.player0HP -= amount;
                break;
            case 1:
                temp.player1HP -= amount;
                break;
            case 2:
                temp.player2HP -= amount;
                break;
            default:
                break;

        }

        info = temp;

    }

    // variables are damage done.
    [Command]
    public void CmdEnemyDamage(float enemy1HP, float enemy2HP, float enemy3HP)
    {
        BattleInfo temp = info;

        print("EnemyDamage");
        temp.enemyHP -= enemy1HP;
        temp.enemy2HP -= enemy2HP;
        temp.enemy3HP -= enemy3HP;

        info = temp;
    }


    // CMD and RPC to update the info of the non-synch vars.




}
