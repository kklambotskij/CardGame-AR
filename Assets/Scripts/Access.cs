using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Access : NetworkBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    public List<HandController> hands = new List<HandController>();

    int playerNumber = -1;
    Text currentPlayerText;
    int timerTurn;
    public bool win;
    [SyncVar] public int amountOfPlayers;
    public HandController NextPlayer()
    {
        playerNumber = (playerNumber + 1) % hands.Count;
        return hands[playerNumber];
    }
#warning AddPlayer to the scene
    public void AddPlayer(string name, GameObject obj)
    {
        amountOfPlayers++;
        players.Add(obj);
        players[players.Count - 1].name = "Player" + amountOfPlayers;
        hands.Add(players[players.Count - 1].GetComponent<HandController>());//обращаемся к чужому скрипту чтобы менять там парметры
        hands[hands.Count - 1].playerName = name;
    }
	public void StartGame() 
	{
        amountOfPlayers = 0;
        win = false;
        timerTurn = -2;
        currentPlayerText = GameObject.Find("CurrentPlayerText").GetComponent<Text>();
		GiveTurn (0);
	}
	void GiveTurn(int hand)
	{
        if (hand >= hands.Count) 
		{
			hand = 0;
		}
		foreach (var item in hands) 
		{
			item.isMyTurn = false;
		}
		if (hand >= 0) {
			hands [hand].isMyTurn = true;
		}
		//Debug.Log (hand);
		playerNumber = hand;
        if (win)
        {
            ChangePlayer(-2);
            return;
        }
        ChangePlayer(playerNumber);
    }
    void GiveTurnTimer(int hand)
    {
        timerTurn = hand;
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown (KeyCode.Space)) {
			GiveTurn (playerNumber + 1);
        }
	}
    void ChangePlayer(int index)
    {
        if(index >= 0)
        {
            currentPlayerText.text = "Ходит игрок: " + hands[index].playerName;
            return;
        }
        if (index < -1)
        {
            currentPlayerText.text = "Победил игрок: " + hands[playerNumber].playerName;
            //win = false;
            return;
        }
        currentPlayerText.text = "";
    }
    public void EndTurn()
    {
        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].CheckForVictory() == true)
            {
                win = true;
                GiveTurn(i);
                //win = true;
            }
        }
        if (!win)
        {
            GiveTurnTimer(playerNumber + 1);
            StartCoroutine(Example());
        }
        //Invoke("GiveTurnTimer(playerNumber + 1)", 1);
    }
    IEnumerator Example()
    {
        print("debug");
        yield return new WaitForSeconds(0.01f);
        if(timerTurn > -2)
        {
            GiveTurn(timerTurn);
            timerTurn = -2;
        }
    }
}
