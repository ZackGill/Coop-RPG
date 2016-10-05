using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    private float speed = 2.25F;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float translation = Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(translation, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-translation, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -translation, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, translation, 0));
        }
    }
}
