using UnityEngine;
using System.Collections;
public class moveCamera : MonoBehaviour
{
    private int speed = 30;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
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

        try
        {
            player = GameObject.Find("PlayerChar");
            transform.position = player.transform.position;
            transform.Translate(new Vector3(0, 0, -1));
        }
        catch { }

    }
}
