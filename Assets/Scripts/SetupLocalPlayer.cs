using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour {

    Access controller;
    Desk desk;

    [SyncVar] public string playerName;
    private void Start()
    {
        controller = GameObject.Find("Access").GetComponent<Access>();
        desk = GameObject.Find("Desk").GetComponent<Desk>();
        Initialize();
    }
    void Initialize()
    {
        GetComponent<HandController>().playerName = playerName;
        controller.AddPlayer(playerName, this.gameObject);
        if (isServer)
        {
            //controller.StartGame();
            //desk.StartGame();
        }
        if (isLocalPlayer)
        {
            GameObject.Find("Canvas").transform.Find("playerName").GetComponent<Text>().text = playerName;
        }
    }
}
