using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour {

    Access controller;
    Desk desk;

    [SyncVar] string playerName;
    private void Start()
    {
        controller = GameObject.Find("Access").GetComponent<Access>();
        desk = GameObject.Find("Desk").GetComponent<Desk>();
        if (isServer)
        {
            playerName = "player" + (controller.players.Count + 1).ToString(); //player1, player2 ...
            name = playerName;
        }
        if (isLocalPlayer)
        {
            //GetComponent<HandController>().enabled = true;
            Initialize();
        }
        else
        {
            //GetComponent<HandController>().enabled = false;
        }
    }
    void Initialize()
    {
        GetComponent<HandController>().playerName = playerName;
        if (isClient)
        {
            controller.AddPlayer(playerName);
        }
        if (isServer)
        {
            controller.StartGame();
            desk.StartGame();
        }
        GameObject.Find("Canvas").transform.Find("playerName").GetComponent<Text>().text = playerName;
        /* 
         * 2) Раздать карты игроку Ваня
         * 3) Скрыть всех игрков кроме него Вадим 
         * 4) Вызвать функции старта игры других скриптов Ваня
         */
    }

}
