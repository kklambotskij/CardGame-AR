using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandController : MonoBehaviour
{
    //RenderMaster renderMaster = new RenderMaster();
    int cardChosen = -1;
	public Vector3 cardLine;
    public Desk localDesk;
    Access turnController;
	public bool isMyTurn = false;
	public List<Card> Cards = new List<Card>();
    GameObject desk;
    public bool isNewCards;
	public string playerName;
    int maxCards = 5;
    int result;
    
	enum Results{
		Nothing, NextPlayer, Skip, ChangeDir, TakeCards, DrawCard, Victory
	}

    void Start()
    {
        result = -1;
        turnController = GameObject.Find("Access").GetComponent<Access>();
        localDesk = GameObject.Find("Desk").GetComponent<Desk>();
        if(gameObject.name == "Hand")
        {
            playerName = "player1";
        }
        else
        {
            playerName = "computer";
        }
        desk = GameObject.Find("Desk");
        isNewCards = false;
    }
    void ResultController(string owner, Type type)
    {
        Results result = ChooseController(owner, type);
        if (result == Results.Nothing) { return; }
        switch (result)
        {
            case Results.Nothing:
                break;
            case Results.NextPlayer:
                EndTurn();
                break;
            case Results.Skip:
                break;
            case Results.ChangeDir:
                break;
            case Results.TakeCards:
                int count = 0;
                switch (localDesk.discardPile.Cards[0].value)
                {
                    case "2Cards":
                        count = 2;
                        break;
                    case "4Cards":
                        count = 4;
                        break;
                }
                for (int i = 0; i < count; i++)
                {
                    localDesk.GiveCard(turnController.NextPlayer(), 0, true);
                }
                break;
            case Results.DrawCard:
                EndTurn();
                break;
            case Results.Victory:
                turnController.win = true;
                EndTurn();
                break;
            default:
                break;
        }
    }
    void Update()
    {
        //Test
        if (isNewCards)
        {
            RenderCard();
        }
		if (isMyTurn) 
		{
            ResultController(playerName, Type.Hand);
            ResultController(localDesk.localDeck.name, Type.Deck);
        }
    }
    public bool CheckForVictory()
    {
        if (result == 2)
        {
            Debug.Log("Win " + playerName);
            return true;
        }
        return false;
    }
    void EndTurn()
    {
        isMyTurn = false;
        turnController.EndTurn();
    }
    Results ChooseController(string owner, Type type)
    {
        Results result = Results.Nothing;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                int res = String.Compare(hit.collider.gameObject.name, 0, owner, 0, owner.Length);
                if (res == 0)
                {
                    string[] numeration = hit.collider.gameObject.name.Split('_');
                    int index = Convert.ToInt32(numeration[1]); 
                    switch (type)
                    {
                        case Type.Deck:
                            result = DrawCard(owner);
                            break;
                        case Type.Hand:
                            result = ChooseCard(owner, index);
                            break;
                        case Type.Pattern:
                            break;
                    }
                   
                }
            }
        }
        return result;
    }

    Results ChooseCard(string owner, int index)
    {
        Results result = Results.Nothing;
        if (index != cardChosen)
        {
            cardChosen = index;
            foreach (var item in Cards)
            {
                Vector3 buf = item.gameobj.transform.position;
                buf.Set(buf.x, cardLine.y, buf.z);
                item.gameobj.transform.position = buf;
            }
            Cards[index].gameobj.transform.position += new Vector3(0, 1, 0);
        }
        else
        {
            result = PlayCard(cardChosen);
            cardChosen = -1;
        }
        return result;
    }

    Results DrawCard(string name)
    {
        if(localDesk.localDeck.Cards.Count > 0)
        {
            //Debug.Log(playerName + isMyTurn.ToString());
            localDesk.GiveCard(this, 0, true);
            return Results.DrawCard;
        }
        return Results.Nothing;
    }

    Results PlayCard(int index)
    {
		Results res = Results.Nothing;
        if (Cards[index].value == localDesk.discardPile.Cards[localDesk.discardPile.Cards.Count - 1].value ||
            Cards[index].color == localDesk.discardPile.Cards[localDesk.discardPile.Cards.Count - 1].color || 
			Cards[index].color == CardColor.Any)
        {
#if true
            Debug.Log(Cards[index].value);
			switch (Cards[index].value) {
				case "CC":
					break;
				case "2Cards":
					res = Results.TakeCards;
					break;
				case "CD":
					res = Results.ChangeDir;
					break;
				case "Skip":
					res = Results.Skip;
					break;
				case "4Cards":
					res = Results.TakeCards;
					break;
				default:
					res = Results.NextPlayer;
					break;
			}
#endif
            int error = GiveCard(localDesk.discardPile, index, true);
			if (error == 0) 
			{
				if (Cards.Count == 0) 
				{
					return Results.Victory;
				}
			}
			return res;
        }
        else
        {
            Debug.Log("Incorrect");
			return Results.Nothing;
        }
    }
    public void AddCard(Card card)
    {
		card.owner = playerName;
        Cards.Add(card);
        isNewCards = true;
    }
    public int GiveCard(Deck deck, int index, bool remove)
    {
        if (Cards.Count > 0)
        {
            Cards[index].isOnScreen = false;
            deck.AddCard(new Card(Cards[index]));
            if (remove)
            {
                Destroy(Cards[index].gameobj);
                Cards.RemoveAt(index);
                RenderCard();
            }
            deck.Render(5, true);
            return 0;
        }
        return -1;
    }

    public void ResetHand()
    {
        while (Cards.Count > 0)
        {
            Destroy(Cards[0].gameobj);
            Cards.RemoveAt(0);
        }
        UsefulShortcuts.ClearConsole();
    }

    void RenderCard()
    {
		//(2.5f - 2.5f * Cards.Count + 5 * i, -5.5f, 31f - 0.1f * i);
		RenderMaster.Render (this, new Vector3(0, 0, 0));
/*
        float x = 2.5f;
        float y = 2 * x;
        float z = 2.5f; // 2.5f

        for (int i = 0; i < Cards.Count; ++i)
        {
            if (!Cards[i].isOnScreen)
            {
                //GameObject handCard = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Card"));
                handCard = (GameObject)GameObject.Instantiate(Resources.Load("FPC/PlayingCards_" + Cards[i].value + Cards[i].GetSuit())); //скачал бесплатные карты
                handCard.transform.position = new Vector3(z - x * Cards.Count + y * i, -5.5f, 31f - 0.1f * i); //так выглядит лучше, теперь карты можно накладывать, они сдвигаются и не пересекаются.
                handCard.transform.rotation *= Quaternion.AngleAxis(-90, new Vector3(1, 0, 0)); //повернул на -90 по оси X
                handCard.transform.localScale += new Vector3(60, 60, 0); //увеличил в 60 раз
                handCard.gameObject.transform.SetParent(transform);
				handCard.name = String.Concat(Cards[i].owner, "_", i);
                Cards[i].gameobj = handCard;
                Cards[i].isOnScreen = true;
                //Debug.Log(Cards[i].value + " " + Cards[i].type);
            }
            else
            {
                Cards[i].gameobj.transform.position = new Vector3(z - x * Cards.Count + y * i, -5.5f, 31f - 0.1f * i);
            }
        }
        isNewCards = false;
*/
    }
}