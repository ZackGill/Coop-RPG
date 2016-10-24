using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class Loading : NetworkBehaviour {
    public NetworkLobbyManager server;
	// Use this for initialization
	void Start () {
        server = GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>();
        Invoke("Load", 5f);
	}
	

    void Load()
    {

        server.ServerChangeScene("genDungeon");

    }

	// Update is called once per frame
	void Update () {
	
	}
}
