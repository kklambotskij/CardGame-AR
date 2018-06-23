using UnityEngine;
public enum State 
{
	Start, Flop, Bet
};
public class Desk : MonoBehaviour
{
    //RenderMaster renderMaster = new RenderMaster();

    public State gameState = State.Start;

    /* Use turnController.hands or .players instead
    public GameObject player1;
    public GameObject AI;
    HandController hand1;
    HandController AIHand;
    */
    Access turnController;

    public Deck localDeck;
    public Deck discardPile;
    public int countCards;
    public int fishy = 21;
    TablePattern pokerPattern;
    // Use this for initialization

    public void Start()
    {
        turnController = GameObject.Find("Access").GetComponent<Access>();
        localDeck = new Deck(Game.UNO, "DrawPile", gameObject);
        discardPile = new Deck(Game.None, "DiscardPile", gameObject);
        localDeck.Shuffle();
        localDeck.Render();
        //pokerPattern = new TablePattern();
        //pokerPattern.AddCard(new Card("2", Suit.Clubs));
        //pokerPattern.Render();
        turnController.StartGame();
        UNOPrepare();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && (turnController.hands.Count > 0))
        {
            GiveCard(turnController.hands[0], 0, true);
        }
        if (Input.GetMouseButtonDown(2) && (turnController.hands.Count > 1))
        {
            GiveCard(turnController.hands[1], 0, true);
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
        for (int i = 0; i < turnController.hands.Count; i++)
        {
            int playerCount = turnController.hands[i].Cards.Count;
            for (int j = 0; j < playerCount; j++)
            {
                turnController.hands[i].GiveCard(localDeck, 0, true);
            }
        }
        GiveCard(discardPile, 0, true);
        for (int i = 0; i < turnController.hands.Count; i++)
        {
            for (int j = 0; j < countCards; j++)
            {
                GiveCard(turnController.hands[i], 0, true);
            }
        }
    }

#if false
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
#endif
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
#if false
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
#endif
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
