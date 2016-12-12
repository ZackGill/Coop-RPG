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


        if (Input.GetKey(KeyCode.Equals))
        {
            speed -= 1;
            if (speed < 10) speed = 10;
            else
                Camera.main.orthographicSize /= (float)1.05;
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            speed += 1;
            if (speed > 100) speed = 100;
            else
                Camera.main.orthographicSize *= (float)1.05;
        }

    }
}
