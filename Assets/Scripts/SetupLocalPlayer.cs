using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    Desk desk;
    Access access;
    List<HandController> hands;

    [SyncVar]
    public string playerName = "player1";

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
        desk = new Desk();
        access = new Access();
        hands = access.GetListOfHands();

        access.StartGame();
        desk.StartGame();

        if (isClient)
        {
            for (int i = 0; i < hands.Count; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    desk.GiveCard(hands[i], 0, true);
                }

            }
        }
        if (isServer)
        {

        }
    }

}
