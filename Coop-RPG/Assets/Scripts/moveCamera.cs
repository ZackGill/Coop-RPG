using UnityEngine;
using System.Collections;
public class moveCamera : MonoBehaviour
{
    private int speed = 30;
    GameObject player;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("PlayerChar");
        transform.position = player.transform.position;
        transform.Translate(new Vector3(0, 0, -1));
    }
}
