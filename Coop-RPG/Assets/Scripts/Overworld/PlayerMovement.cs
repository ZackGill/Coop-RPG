using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class PlayerMovement : NetworkBehaviour
{
    public float speed = 2.25F;
    // Use this for initialization
    void Start()
    {

    }

    public override void OnStartLocalPlayer()
    {
        GameObject temp = GameObject.Find("DungeonGen");
        temp.GetComponent<GenerateDungeon>().spawnLocal = gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        else
        {
            if (Camera.main == null)
                return;
            Camera.main.GetComponent<moveCamera>().player = gameObject;
        }
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

    void FixedUpdate()
    {


    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene("BattleScreen");
        }
        if(coll.gameObject.tag == "Player")
        {
            return;
        }
    }
}
