using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    [SyncVar]
    string playerName = "player1";

    void OnServerInitialized()
    {
        //вызывате при инициализации сервера
    }
    void OnConnectedToServer()
    {
        
    }
    void Initialize()
    {
        /* 1) поменять имя игрока Вадим
         * 2) Раздать карты игроку Ваня
         * 3) Скрыть всех игрков кроме него Вадим 
         * 4) Вызвать функции старта игры других скриптов Ваня
         */
        if (isClient)
        {

        }
        if (isServer)
        {

        }
    }

}
