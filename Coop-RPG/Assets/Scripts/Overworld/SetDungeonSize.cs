using UnityEngine;
using System.Collections;

public static class Dungeon
{
    public static int Size;
}

public class SetDungeonSize : MonoBehaviour {

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
