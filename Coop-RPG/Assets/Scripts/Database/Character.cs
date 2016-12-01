using System.Collections.Generic;
using System.Collections;
using System;
using SimpleFirebaseUnity;
using SimpleFirebaseUnity.MiniJSON;
using UnityEngine;
using AssemblyCSharp;

public class Character : MonoBehaviour
{

    static int debug_idx = 0;
    public int attack, magic, defense, hp, exp;
    string clName;

    //string itemJson;
    string acc1, acc2, weapon, armor, wType, acc1Type, acc2Type;
    //Skill[] sks;
    [SerializeField]
    TextMesh txt;


    void GetEXPHandler(Firebase sender, DataSnapshot snapshot)
    {
        DoDebug("Vals: " + snapshot.RawJson);

        //int temp = snapshot.RawValue as int;
        exp = int.Parse(snapshot.RawJson);
        DoDebug("EXP: " + exp);
    }

    void GetATKHandler(Firebase sender, DataSnapshot snapshot)
    {
        attack += int.Parse(snapshot.RawJson);

    }

    void GetMAGHandler(Firebase sender, DataSnapshot snapshot)
    {
        magic += int.Parse(snapshot.RawJson);

    }

    void GetHPHandler(Firebase sender, DataSnapshot snapshot)
    {
        hp = int.Parse(snapshot.RawJson);

    }

    void GetDEFHandler(Firebase sender, DataSnapshot snapshot)
    {
        defense += int.Parse(snapshot.RawJson);

    }

    void GetEXPHandlerBAD(Firebase sender, FirebaseError err)
    {
        exp = -1;
    }

    void GetATKHandlerBAD(Firebase sender, FirebaseError err)
    {
        attack = -1;
    }



    void GetHPHandlerBAD(Firebase sender, FirebaseError err)
    {
        hp = -1;
    }

    void GetClassName(Firebase sender, DataSnapshot snapshot)
    {
        DoDebug("CLASS: " + snapshot.RawJson);
        clName = snapshot.Value<string>();
    }

    void GetAcc1Name(Firebase sender, DataSnapshot snapshot)
    {
        acc1 = snapshot.Value<string>();
    }

    void GetAcc2Name(Firebase sender, DataSnapshot snapshot)
    {
        acc2 = snapshot.Value<string>();
    }

    void GetWeapName(Firebase sender, DataSnapshot snapshot)
    {
        weapon = snapshot.Value<string>();
    }

    void GetArmorName(Firebase sender, DataSnapshot snapshot)
    {
        armor = snapshot.Value<string>();
    }

    void DoDebug(string str)
    {
        Debug.Log(str);
        if (txt != null)
        {
            txt.text += (++debug_idx + ". " + str) + "\n";
        }
    }

    void GetWeaponType(Firebase sender, DataSnapshot snapshot)
    {
        DoDebug("HELP!!: " + snapshot.Value<string>());
        wType = snapshot.Value<string>();
    }

    void GetAcc1Type(Firebase sender, DataSnapshot snapshot)
    {
        DoDebug("A1Type: " + snapshot.Value<string>());
        acc1Type = snapshot.Value<string>();
    }

    void GetAcc2Type(Firebase sender, DataSnapshot snapshot)
    {
        acc2Type = snapshot.Value<string>();
    }


    //		public Character(string name) {
    void Start()
    {

        StartCoroutine(wait());

        //StartCoroutine (Run ());
        /*
        getVals ();


        addClassVals (clName);

        DoDebug( "HP: " + hp + "ATK: " + attack + "EXP: " + exp);
        DoDebug ("==== Wait for seconds 15f ======");
        new WaitForSeconds (15f);
        DoDebug( "HP: " + hp + "ATK: " + attack + "EXP: " + exp);
        */
    }

    IEnumerator wait()
    {
        DatabaseManager db = new DatabaseManager();
        Characters test = null;
        StartCoroutine(db.runChar("Example"));
        yield return new WaitForSeconds(35f);
        test = db.getChar();
        DoDebug("-AFTER-\nAttack: " + test.getAttack() + "\nDefense: " + test.getDefense());

    }


    private void getVals()
    {
        FirebaseQueue q = new FirebaseQueue();
        Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/Characters", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
        Firebase chara;
        chara = fb.Child("Example");
        Firebase expFB;
        expFB = chara.Child("EXP");
        expFB.OnGetSuccess += GetEXPHandler;
        expFB.OnGetFailed += GetEXPHandlerBAD;
        expFB.GetValue ();
        q.AddQueueGet(expFB);

        Firebase atkFB;
        atkFB = chara.Child("stats").Child("attack");
        atkFB.OnGetSuccess += GetATKHandler;
        atkFB.OnGetFailed += GetATKHandlerBAD;
        atkFB.GetValue ();
        q.AddQueueGet(atkFB);

        Firebase magFB;
        magFB = chara.Child("stats").Child("magic");
        magFB.OnGetSuccess += GetMAGHandler;

        magFB.GetValue ();
        q.AddQueueGet(magFB);

        Firebase defFB;
        defFB = chara.Child("stats").Child("defense");
        defFB.OnGetSuccess += GetDEFHandler;
        //defFB.GetValue ();
        q.AddQueueGet(defFB);

        Firebase hpFB;
        hpFB = chara.Child("HP");
        hpFB.OnGetSuccess += GetHPHandler;
        hpFB.OnGetFailed += GetHPHandlerBAD;
        hpFB.GetValue ("print=pretty");
        q.AddQueueGet(hpFB);

        Firebase classFB;
        classFB = chara.Child("class");
        classFB.OnGetSuccess += GetClassName;

        //classFB.GetValue ("print=pretty");
        q.AddQueueGet(classFB);

        Firebase itemFB = chara.Child("equipment");
        Firebase acc1FB = itemFB.Child("acc1");
        acc1FB.OnGetSuccess += GetAcc1Name;
        //acc1FB.GetValue ();
        q.AddQueueGet(acc1FB);
        Firebase acc2FB = itemFB.Child("acc2");
        acc2FB.OnGetSuccess += GetAcc2Name;
        //acc2FB.GetValue ();
        q.AddQueueGet(acc2FB);
        Firebase weapFB = itemFB.Child("weapon");
        weapFB.OnGetSuccess += GetWeapName;
        //weapFB.GetValue ();
        q.AddQueueGet(weapFB);
        Firebase arFB = itemFB.Child("armor");
        arFB.OnGetSuccess += GetArmorName;
        //arFB.GetValue ();
        q.AddQueueGet(arFB);

    }

    private void addClassVals(string cName)
    {
        //DoDebug("FUG: " + ClassLU.getAttack(cName));
        FirebaseQueue q = new FirebaseQueue();
        Firebase cl = Firebase.CreateNew("coop-rpg.firebaseio.com/Classes", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
        //DoDebug("CLASS ATK: " + ClassLU.getAttack(cName));
        Firebase atk = cl.Child(cName).Child("baseStats").Child("attack");
        atk.OnGetSuccess += GetATKHandler;
        //atk.GetValue ();
        q.AddQueueGet(atk);
        Firebase def = cl.Child(cName).Child("baseStats").Child("defense");
        def.OnGetSuccess += GetDEFHandler;
        //def.GetValue ();
        q.AddQueueGet(def);
        Firebase mag = cl.Child(cName).Child("baseStats").Child("magic");
        mag.OnGetSuccess += GetMAGHandler;
        mag.GetValue();
        q.AddQueueGet(mag);
        DoDebug("fguATK: " + attack + "\n" + "MAG: " + magic + "\n" + "DEF: " + defense);
    }

    private void addItemVals()
    {
        Firebase iLU = Firebase.CreateNew("coop-rpg.firebaseio.com/ItemLU", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");
        FirebaseQueue q = new FirebaseQueue();
        //Weapon
        Firebase wCheck = iLU.Child("weapons").Child(weapon);

        Firebase wTypeFB = wCheck.Child("type");
        wTypeFB.OnGetSuccess += GetWeaponType;


        Firebase wNum = wCheck.Child("value");
        //q.AddQueueGet (wTypeFB);
        wTypeFB.GetValue();


        //Armor
        Firebase arCheck = iLU.Child("armor").Child(armor).Child("value");
        arCheck.OnGetSuccess += GetDEFHandler;
        arCheck.GetValue();
        //q.AddQueueGet(arCheck);
        //Accs
        Firebase acc1FB = iLU.Child("accesory").Child(acc1);
        Firebase acc2FB = iLU.Child("accesory").Child(acc2);
        Firebase acc1T = acc1FB.Child("stat");
        Firebase acc2T = acc2FB.Child("stat");

        //getAccTypes (acc1FB, acc2FB);
        acc1T.OnGetSuccess += GetAcc1Type;
        //a1.GetValue ();
        acc2T.OnGetSuccess += GetAcc2Type;

        //q.AddQueueGet (acc1FB);
        //q.AddQueueGet (acc2FB);
        acc1T.GetValue();
        acc2T.GetValue();






    }

    private void addIVals()
    {
        FirebaseQueue q = new FirebaseQueue();
        Firebase fb = Firebase.CreateNew("coop-rpg.firebaseio.com/ItemLU", "nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1");

        Firebase weap = fb.Child("weapons").Child(weapon).Child("value");
        if (wType.Equals("attack"))
        {

            weap.OnGetSuccess += GetATKHandler;
            //wNum.GetValue ();		
        }
        if (wType.Equals("magic"))
        {
            weap.OnGetSuccess += GetMAGHandler;
            //wNum.GetValue ();
        }

        q.AddQueueGet(weap);

        Firebase a1 = fb.Child("accesory").Child(acc1).Child("value");
        Firebase a2 = fb.Child("accesory").Child(acc2).Child("value");

        if (acc1Type.Equals("attack"))
        {
            a1.OnGetSuccess += GetATKHandler;
            //acc1Val.GetValue ();
        }

        if (acc1Type.Equals("magic"))
        {
            a1.OnGetSuccess += GetMAGHandler;
            //acc1Val.GetValue ();
        }

        if (acc1Type.Equals("defense"))
        {
            a1.OnGetSuccess += GetDEFHandler;
            //acc1Val.GetValue ();
        }
        q.AddQueueGet(a1);

        if (acc2Type.Equals("attack"))
        {
            a2.OnGetSuccess += GetATKHandler;
            //acc2Val.GetValue ();
        }

        if (acc2Type.Equals("magic"))
        {
            a2.OnGetSuccess += GetMAGHandler;
            //acc2Val.GetValue ();
        }

        if (acc2Type.Equals("defense"))
        {
            a2.OnGetSuccess += GetDEFHandler;
            //acc2Val.GetValue ();
        }

        q.AddQueueGet(a2);
    }

    private void getAccTypes(Firebase a1, Firebase a2)
    {
        a1.OnGetSuccess += GetAcc1Type;
        //a1.GetValue ();
        a2.OnGetSuccess += GetAcc2Type;
        //a2.GetValue ();
    }

    IEnumerator Run()
    {
        getVals();

        DoDebug("WAITING" + weapon);
        yield return new WaitForSeconds(2f);
        DoDebug("DONE");

        addClassVals(clName);

        DoDebug("WAITING" + weapon);
        yield return new WaitForSeconds(3f);
        DoDebug("DONE");

        addItemVals();


        DoDebug("fwaitATK: " + attack + "\n" + "MAG: " + magic + "\n" + "DEF: " + defense);
        yield return new WaitForSeconds(3f);
        DoDebug("finalATK: " + attack + "\n" + "MAG: " + magic + "\n" + "DEF: " + defense);

        addIVals();

        DoDebug("fwaitATK: " + attack + "\n" + "MAG: " + magic + "\n" + "DEF: " + defense);
        yield return new WaitForSeconds(3f);
        DoDebug("finalATK: " + attack + "\n" + "MAG: " + magic + "\n" + "DEF: " + defense);

    }




    /*
        int attack, magic, defense, hp, exp;
        Skill[] sks;
        IFirebase fb, chara, inner;

        public Character (String name) {
            fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Characters");
            fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

            }, (FirebaseError e) => {

            });

            chara = fb.Child (name);
            hp = int.Parse(chara.Child ("hp").ToString());
            exp = int.Parse(chara.Child ("exp").ToString());
            String rpgclass = chara.Child("class").ToString();
            inner = chara.Child ("stats");
            attack = int.Parse(inner.Child ("attack").ToString());
            defense = int.Parse(inner.Child ("defense").ToString());
            magic = int.Parse(inner.Child ("magic").ToString());

            String skillNames = chara.Child ("skills").ToString();
            String perkNames = chara.Child ("perks").ToString();

            string[] skills = skillNames.Split (new char[] {','});
            string[] perks = perkNames.Split (new char[] {','});

            Skill a = new Skill (skills [0], perks [0]);

            sks = new Skill[1];
            sks [0] = a;


            inner = chara.Child ("equipment");
            String acc1, acc2, weapon, armor;

            weapon = inner.Child ("weapon").ToString();
            armor = inner.Child ("armor").ToString();
            acc1 = inner.Child ("acc1").ToString();
            acc2 = inner.Child ("acc2").ToString();

            addWeapon (weapon);
            addAcc (acc1);
            addAcc (acc2);

            defense += ItemLU.getArmor (armor);



            addClassVals (rpgclass);

        }

        private void addClassVals(String clName) {
            attack += ClassLU.getAttack (clName);
            magic += ClassLU.getMagic (clName);
            defense += ClassLU.getDefense (clName);

        }

        private void addWeapon(String name) {
            String wType = ItemLU.getWeapType (name);
            int wVal = ItemLU.getWeapValue (name);

            if (wType.Equals("magic")) {
                magic += wVal;
            }

            if (wType.Equals ("attack")) {
                attack += wVal;
            }


        }

        private void addAcc(String name) {
            String accStat = ItemLU.getAccStat (name);
            int accVal = ItemLU.getAccValue (name);

            if (accStat.Equals ("attack")) {
                attack += accVal;
            }

            if (accStat.Equals ("magic")) {
                magic += accVal;
            }

            if (accStat.Equals ("defense")) {
                defense += accVal;
            }

        }







    /*	public Character (string cName, int exp, int hp, int atk, int def, int mag, 
            string rpgclass, string weapon, string armor, string acc, string perks, string skills)
        {
            fb = Firebase.CreateNew ("https://coop-rpg.firebaseio.com/Characters");
            fb.AuthWithCustomToken ("nofP6v645gh35aA1jlQGOc4ueceuDZqEIXu7qMs1", (AuthData auth) => {

            }, (FirebaseError e) => {

            });
            chara = fb.Child (cName);
            chara.SetJsonValue("{\"EXP\" : " + exp + ", \"HP\" : "+ hp + ", \"class\" : \"" + 
                rpgclass + "\", \"stats\" : {\"attack\" : " + atk + ", \"defense\" : " +
                def + ", \"magic\" : " + mag + "}, \"skills\" : \"" + skills + "\", \"perks\" : \"" +
                perks + "\", \"equipment\" : {\"weapon\" : \"" + weapon + "\", \"armor\" : \"" +
                armor + "\", \"acceosry\" : \"" + acc + "\"}}");


        }*/
}


