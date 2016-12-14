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

        public float player0Threat;
        public float player1Threat;
        public float player2Threat;

    }

    [SyncVar(hook="OnInfo")]
    public BattleInfo info;

    public Monster enemy0 = null;
    public Monster enemy1 = null;
    public Monster enemy2 = null;

    public Characters player0 = null;
    public Characters player1 = null;
    public Characters player2 = null;



    public BattleLogic battle0;
    public BattleLogic battle1;
    public BattleLogic battle2;

   

    public void OnInfo(BattleInfo value)
    {
        info = value;
    }

    // Use this for initialization
    void Start () {

    }
    [SyncVar]
    public bool startMonster0Health = false;

    [SyncVar]
    public bool startMonster1Health = false;

    [SyncVar]
    public bool startMonster2Health = false;

    // Update is called once per frame
    void Update () {

        if(enemy0 != null && !startMonster0Health)
        {
            print("Set enemy0 health");
            BattleInfo temp = info;
            temp.enemyHP = enemy0.getHP();
            temp.fightMessage = "An enemy draws near!";
            info = temp;
            startMonster0Health = true;
        }

        if (enemy1 != null && !startMonster1Health)
        {
            print("Set enemy1 health");

            BattleInfo temp = info;
            temp.enemy2HP = enemy1.getHP();
            info = temp;
            startMonster1Health = true;
        }

        if (enemy2 != null && !startMonster2Health)
        {
            print("Set enemy2 health");

            BattleInfo temp = info;
            temp.enemy3HP = enemy2.getHP();
            info = temp;
            startMonster2Health = true;
        }
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
                if (temp.player0HP > player0.getHP())
                    temp.player0HP = player0.getHP();
                break;
            case 1:
                temp.player1HP -= amount;
                if (temp.player1HP > player1.getHP())
                    temp.player1HP = player1.getHP();
                break;
            case 2:
                temp.player2HP -= amount;
                if (temp.player2HP > player2.getHP())
                    temp.player2HP = player2.getHP();
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

    [Command]
    public void CmdAddPlayer(GameObject player)
    {
        print("Add player");
        BattleInfo temp = info;
        temp.numPlayers = temp.numPlayers + 1;

        if (player.GetComponent<PlayerMovement>().battle != null)
        {
            player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().infoDump = gameObject.GetComponent<OverworldBattle>();
            player.GetComponent<PlayerMovement>().battle.GetComponentInChildren<BattleLogic>().playerNum = temp.numPlayers - 1;
        }

        switch (temp.numPlayers - 1)
        {
            case 0:
                player0 = player.GetComponent<PlayerMovement>().characterInfo;
                temp.player0HP = player.GetComponent<PlayerMovement>().health;
                RpcUpdatePlayers(player, null, null);

                break;
            case 1:
                player1 = player.GetComponent<PlayerMovement>().characterInfo;
                temp.player1HP = player.GetComponent<PlayerMovement>().health;
                RpcUpdatePlayers(null, player, null);

                break;
            case 2:
                player2 = player.GetComponent<PlayerMovement>().characterInfo;
                temp.player2HP = player.GetComponent<PlayerMovement>().health;
                RpcUpdatePlayers(null, null, player);

                break;
        }
        info = temp;

    }
    public void OnDestroy()
    {
        if(battle0 != null)
        {
            Destroy(battle0.gameObject.transform.parent);
        }
        if (battle1 != null)
        {
            Destroy(battle1.gameObject.transform.parent);
        }
        if (battle2 != null)
        {
            Destroy(battle2.gameObject.transform.parent);
        }
    }
    [ClientRpc]
    public void RpcUpdatePlayers(GameObject char0, GameObject char1, GameObject char2)
    {
        if (char0 == null)
            print("Stuff");
        if(char0 != null)
            player0 = char0.GetComponent<PlayerMovement>().characterInfo;
        if(char1 != null)
            player1 = char1.GetComponent<PlayerMovement>().characterInfo;
        if(char2 != null)
            player2 = char2.GetComponent<PlayerMovement>().characterInfo;
    }

    // CMD and RPC to update the info of the non-synch vars.

    [Command]
    public void CmdFightMessage(string message)
    {

        print("fight message");
        BattleInfo temp = info;
        temp.fightMessage = message;
        info = temp;

    }

    [Command]
    public void CmdAttackFlag(bool flag)
    {
        print("flag");
        BattleInfo temp = info;
        temp.enemyAttackFlag = flag;
        info = temp;
    }


    [Command]
    public void CmdAddMonster(GameObject monsterHolder)
    {
        print("Add monster");
        BattleInfo temp = info;
        temp.numEnemies = temp.numEnemies + 1;
        switch (temp.numEnemies - 1)
        {
            case 0:
                enemy0 = monsterHolder.GetComponent<MonsterStorage>().monster;
                temp.enemyHP = monsterHolder.GetComponent<MonsterStorage>().monster.getHP();
                if (monsterHolder.GetComponent<MonsterStorage>().monster.getHP() <= 0)
                    temp.enemyHP = 20;
                temp.enemy0Max = 15f;
                RpcUpdateMonsters(monsterHolder, null, null);
                break;
            case 1:
                enemy1 = monsterHolder.GetComponent<MonsterStorage>().monster;
                temp.enemy1Max = 15f;
                if (monsterHolder.GetComponent<MonsterStorage>().monster.getHP() <= 0)
                    temp.enemy2HP = 20;
                temp.enemy2HP = monsterHolder.GetComponent<MonsterStorage>().monster.getHP();

                RpcUpdateMonsters(null, monsterHolder, null);
                break;
            case 2:
                enemy2 = monsterHolder.GetComponent<MonsterStorage>().monster;
                temp.enemy3HP = monsterHolder.GetComponent<MonsterStorage>().monster.getHP();
                temp.enemy2Max = 15f;
                if (monsterHolder.GetComponent<MonsterStorage>().monster.getHP() <= 0)
                    temp.enemy3HP = 20;
                RpcUpdateMonsters(null, null, monsterHolder);
                break;
        }
        info = temp;




    }

    [ClientRpc]
    public void RpcUpdateMonsters(GameObject monsterHolder0, GameObject monsterHolder1, GameObject monsterHolder2)
    {
        print("Update Monsters RPC");
        if (monsterHolder0 == null)
            print("Mon0 is null RPCUpdate");
        if(monsterHolder0 != null)
         enemy0 = monsterHolder0.GetComponent<MonsterStorage>().monster; ;
        if(monsterHolder1 != null)
            enemy1 = monsterHolder1.GetComponent<MonsterStorage>().monster; ;
        if(monsterHolder2 != null)
            enemy2 = monsterHolder2.GetComponent<MonsterStorage>().monster; ;
    }

    [Command]
    public void CmdEnemyTimes(float one, float two, float three)
    {
        BattleInfo temp = info;
        temp.enemy0Sec = one;
        temp.enemy1Sec = two;
        temp.enemy2Sec = three;
        info = temp;
    }

    [Command]
    public void CmdEnemyMaxTimes(float one, float two, float three)
    {
        BattleInfo temp = info;
        temp.enemy0Max = one;
        temp.enemy1Max = two;
        temp.enemy2Max = three;
        info = temp;
    }

    [Command]
    public void CmdRemoveEnemy()
    {
        BattleInfo temp = info;
        if(temp.numEnemies == 3)
        {
            enemy2 = null;
            temp.numEnemies--;
        }
        else if(temp.numEnemies == 2)
        {
            enemy1 = null;
            temp.numEnemies--;
        }
        else if (temp.numEnemies == 1)
        {
            enemy0 = null;
            temp.numEnemies--;
        }
        info = temp;
    }

    [Command]
    public void CmdRemovePlayer()
    {
        BattleInfo temp = info;
        if (temp.numPlayers == 3)
        {
            player2 = null;
            temp.numPlayers--;
        }
        else if (temp.numPlayers == 2)
        {
            player1 = null;
            temp.numPlayers--;
        }
        else if (temp.numPlayers == 1)
        {
            player2 = null;
            temp.numPlayers--;
        }
        info = temp;
    }
}
