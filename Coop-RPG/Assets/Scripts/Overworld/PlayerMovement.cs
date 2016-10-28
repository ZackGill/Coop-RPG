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
        if(isLocalPlayer)
          Invoke("startPos", .1f); 
    }

    void startPos()
    {
        if (!isLocalPlayer)
            return;
        GenerateDungeon temp = GameObject.Find("DungeonGen").GetComponent<GenerateDungeon>();
        switch(Random.Range(0, 4))
        {
            case 0:
                transform.position = temp.spawnLocal.transform.position;
                break;
            case 1:
                transform.position = temp.spawnLocal1.transform.position;
                break;
            case 2:
                transform.position = temp.spawnLocal2.transform.position;
                break;
            case 3:
                transform.position = temp.spawnLocal3.transform.position;
                break;
        }

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
            Vector3 pos = transform.position;
            pos.z = -.5f;
            transform.position = pos;
            if (Camera.main == null)
                return;
            Camera.main.GetComponent<moveCamera>().player = gameObject;
        }
        float translation = Time.deltaTime * speed;
		int dir = 0;
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(new Vector3(translation, 0, 0));
			dir = 4;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(new Vector3(-translation, 0, 0));
			dir = 3;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(new Vector3(0, -translation, 0));
			dir = 2;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(new Vector3(0, translation, 0));
			dir = 1;
		}
		//GetComponent<Animator> ().SetInteger ("Dir", dir);
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
