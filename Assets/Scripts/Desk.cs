using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State 
{
	Start, Flop, Bet
};
public class Desk : MonoBehaviour
{
    //RenderMaster renderMaster = new RenderMaster();

    public State gameState = State.Start;
    public GameObject player1;
    public GameObject AI;
    HandController hand1;
    HandController AIHand;
    public Deck localDeck;
    public Deck discardPile;
    float time = 1;
    public int countCards;
    public int fishy = 21;
    TablePattern pokerPattern;
    // Use this for initialization

    void StartGame()
    {
        localDeck = new Deck(Game.UNO, "DrawPile", this.gameObject);
        //localDeck = new Deck(Game.UNO, "UNO", this.gameObject);
        discardPile = new Deck(Game.None, "DiscardPile", this.gameObject);
        player1 = GameObject.Find("Hand");
        AI = GameObject.Find("AI");
        hand1 = player1.GetComponent<HandController>(); //обращаемся к чужому скрипту чтобы менять там парметры
        AIHand = AI.GetComponent<HandController>();
        localDeck.Shuffle();
        localDeck.Render();
        //pokerPattern = new TablePattern();
        //pokerPattern.AddCard(new Card("2", Suit.Clubs));
        //pokerPattern.Render();
        UNOPrepare();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GiveCard(hand1, 0, true);
        }
        if (Input.GetMouseButtonDown(2))
        {
            GiveCard(AIHand, 0, true);
        }
    }

    void FlipDeck()
    {
        foreach (var item in localDeck.Cards)
        {
            item.gameobj.transform.rotation *= Quaternion.AngleAxis(-180, new Vector3(1, 0, 0));
        }
    }

    void UNOPrepare()
    {
        //int discardCount = discardPile.Cards.Count;
        int playerCount = hand1.Cards.Count;
        int AICount = AIHand.Cards.Count;
        for (int i = 0; i < playerCount; i++)
        {
            hand1.GiveCard(localDeck, 0, true);
        }
        for (int i = 0; i < AICount; i++)
        {
            AIHand.GiveCard(localDeck, 0, true);
        }
        //DeckReset();
        GiveCard(discardPile, 0, true);
        for (int i = 0; i < 1; i++)
        {
            GiveCard(hand1, 0, true);
            GiveCard(AIHand, 0, true);
        }
    }
#if false
    /*
    public void RenderDeck(Deck deck, int shift = 0, bool flip = false)
    {
        
        for (int i = 0; i < deck.Cards.Count; i++)
        {
            GameObject handCard;
            if (!deck.Cards[i].isOnScreen)
            {
                GameObject handCard = (GameObject)Instantiate(Resources.Load("FPC/PlayingCards_" + deck.Cards[i].value + deck.Cards[i].GetSuit()));
                handCard.transform.rotation *= Quaternion.AngleAxis(flip ? -30 : -210, new Vector3(1, 0, 0)); //повернул на -90 по оси X
                handCard.transform.localScale += new Vector3(60, 60, 0); //увеличил в 60 раз
                handCard.transform.position = new Vector3(21 - shift, 0 + 0.05f * i, 50);
                if (handCard.GetComponent<Collider>() == null)
                {
                    handCard.AddComponent<BoxCollider>();
                }
                if (!flip) handCard.gameObject.transform.SetParent(transform);
				handCard.name = String.Concat(deck.name, "_", i);
                deck.Cards[i].gameobj = handCard;
                deck.Cards[i].isOnScreen = true;
            }
            else
            {
                deck.Cards[i].gameobj.name = String.Concat(deck.name, "_", i);
                deck.Cards[i].gameobj.transform.position = new Vector3(21 - shift, 0 + 0.05f * i, 50);
            }
        }
        */
    }
#endif
    int RussianBlackJack()
    {
        int counter = 0;
        foreach (var item in hand1.Cards)
        {
            switch (item.value)
            {
                case "J":
                    counter += 2;
                    break;
                case "Q":
                    counter += 3;
                    break;
                case "K":
                    counter += 4;
                    break;
                case "A":
                    counter += 11;
                    break;
                default:
                    counter += Convert.ToInt32(item.value);
                    break;
            }
        }

        if (counter > 21)
        {
            return -1;
        }
        if (counter < 21)
        {
            return 0;
        }

        return 1;
    }

	public void Flop()
	{

        UNOPrepare();
        /*
		if (gameState == State.Start)
		{
			GiveCard(pokerPattern, 0, true);
			GiveCard(pokerPattern, 0, true);
			GiveCard(pokerPattern, 0, true);
			gameState = State.Flop;
		}
        */
	}

    public void CheckforVictory()
    {
        if (localDeck.game == Game.BlackJack)
        {
            switch (RussianBlackJack())
            {
                case -1:
                    Debug.Log("Defeat");
                    break;
                case 1:
                    Debug.Log("Victory");
                    break;
                default:
                    Debug.Log("Continue or stop");
                    break;
            }
        }
    }

    public int GiveCard(TablePattern patt, int index, bool remove)
	{
		if (localDeck.Cards.Count > 0)
		{
			localDeck.Cards[index].isOnScreen = false;
			patt.AddCard(localDeck.Cards[index]);
			if (remove)
			{
				Destroy(localDeck.Cards[index].gameobj);
				localDeck.Cards.RemoveAt(index);
                localDeck.Render();
            }
			return 0;
		}
		return -1;
	}

    public int GiveCard(HandController hand, int index, bool remove)
    {
        if (localDeck.Cards.Count > 0)
        {
            localDeck.Cards[index].isOnScreen = false;
            hand.AddCard(localDeck.Cards[index]);
            if (remove)
            {
                Destroy(localDeck.Cards[index].gameobj);
                localDeck.Cards.RemoveAt(index);
                localDeck.Render();
            }
            return 0;
        }
        return -1;
    }

    public int GiveCard(Deck deck, int index, bool remove)
    {
        if (localDeck.Cards.Count > 0)
        {
            localDeck.Cards[index].isOnScreen = false;
            deck.AddCard(new Card(localDeck.Cards[index]));
            if (remove)
            {
                Destroy(localDeck.Cards[index].gameobj);
                localDeck.Cards.RemoveAt(index);
                localDeck.Render();
            }
            deck.Render(5, true);
            return 0;
        }
        return -1;
    }

    public void DeckReset()
    {
        Debug.Log("New Deck");
        switch (localDeck.game)
        {
            case Game.Poker:
                localDeck = new Deck(Game.Poker, localDeck.name, gameObject);
                break;
            case Game.BlackJack:
                localDeck = new Deck(Game.BlackJack, localDeck.name, gameObject);
                break;
            case Game.UNO:
                localDeck = new Deck(Game.UNO, localDeck.name, gameObject);
                break;
            default:
                localDeck = new Deck(Game.None, localDeck.name, gameObject);
                break;
        }
		localDeck.Shuffle ();
        localDeck.Render();
    }
    // FixedUpdate is called once per 0.02 sec

    void FixedUpdate()
    {

    }
}
