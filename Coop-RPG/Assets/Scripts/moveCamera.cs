using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class moveCamera : NetworkBehaviour
{
    private int speed = 30;
    public GameObject player;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1f);

    }
}
