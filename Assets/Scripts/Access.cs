using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Access : MonoBehaviour 
{
	List <GameObject> players = new List<GameObject>();
	List <HandController> hands = new List<HandController>();
	int playerNumber = -1;
    Text currentPlayerText;
    int timerTurn;
    public bool win;
    public int amountOfPlayers;

   public HandController NextPlayer()
    {
        playerNumber = (playerNumber + 1) % hands.Count;
        return hands[playerNumber];
    }

	// Use this for initialization
	void Start () 
	{
        amountOfPlayers = 2;
        win = false;
        timerTurn = -2;
        currentPlayerText = GameObject.Find("CurrentPlayerText").GetComponent<Text>();
        for (int i = 0; i < amountOfPlayers; i++)
        {
            //players.Add(GameObject.Find("Player" + i));
            //hands.Add(players[i].GetComponent<HandController>());
        }
        players.Add(GameObject.Find("Hand"));
		players.Add(GameObject.Find ("AI"));
		hands.Add(players[0].GetComponent<HandController>()); //обращаемся к чужому скрипту чтобы менять там парметры
		hands.Add(players[1].GetComponent<HandController>());
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
