using UnityEngine;
using System.Collections;

public static class Dungeon
{
    public static int Size;
}

public class SetDungeonSize : MonoBehaviour {
    public void makeRandom()
    {
        Dungeon.Size = UnityEngine.Random.Range(1, 3);
    }
	public void makeSmall()
    {
        Dungeon.Size = 1;
    }
    
    public void makeMedium()
    {
        Dungeon.Size = 2;
    }

    public void makeLarge()
    {
        Dungeon.Size = 3;
    }
}
