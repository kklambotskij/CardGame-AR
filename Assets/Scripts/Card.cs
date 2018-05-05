   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

static class UsefulShortcuts
{
    public static void ClearConsole()
    {
        //var assembly = Assembly.GetAssembly(typeof(SceneView));
        //var type = assembly.GetType("UnityEditor.LogEntries");
        //var method = type.GetMethod("Clear");
        //method.Invoke(new object(), null);
    }
}
public enum Type
{
    Deck, Hand, Pattern
}
public enum Game
{
    Poker, BlackJack, UNO,None
}
public enum Suit
{
    Hearts, Spades, Diamonds, Clubs
}
public enum CardColor
{
    Red, Yellow, Green, Blue, Any
}
public class Card {
    public CardColor color;
	public Suit type;
	public string value;	
	public bool isOnScreen;
    public GameObject gameobj;
	public string owner;

    public int GetColor()
    {
        switch (color)
        {
            case CardColor.Red:
                return 0;
            case CardColor.Yellow:
                return 1;
            case CardColor.Green:
                return 2;
            case CardColor.Blue:
                return 3;
            default:
                return 4;
        }
    }
	static public CardColor GetColor(int c)
	{
		switch (c) 
		{
			case 0:
				return CardColor.Red;
			case 1:
				return CardColor.Yellow;
			case 2:
				return CardColor.Green;
			case 3:
				return CardColor.Blue;
			default:
				return CardColor.Any;
		}
	}
    public int GetValue()
    {
        int result;
		switch (value)
		{
            case "CC": result = 2; break;
            case "2Cards": result = 12; break;
            case "CD": result = 11;  break;
            case "Skip": result = 10;  break;
            case "4Cards": result = 1; break;
			default: result = System.Convert.ToInt32(value); break;
        }
        return result;

    }
    public string GetSuit()
    {
        switch (type)
        {
            case Suit.Hearts:
                return "Heart";
            case Suit.Spades:
                return "Spades";
            case Suit.Diamonds:
                return "Diamond";
            case Suit.Clubs:
                return "Club";
            default:
                return "Error";
        }
    }
	public Card (string value, Suit type)
	{
		this.type = type;
		this.value = value;
        isOnScreen = false;
		this.owner = "none";
	}

	public Card (string value, Suit type, string owner)
	{
		this.type = type;
		this.value = value;
		isOnScreen = false;
		this.owner = owner;
	}
    public Card(string value, CardColor type, string owner)
    {
        this.color = type;
        this.value = value;
        isOnScreen = false;
        this.owner = owner;
    }
    public Card(Card card)
    {
        type = card.type;
		color = card.color;
        value = card.value;
        isOnScreen = false;
        owner = card.owner;
    }
}
public class TablePattern
{
	public bool isNewCards = false;
	public string patternName;
    public Deck localDeck;
    public Game game;
    public float positionX, positionY;
    public int maxOfCards;
    //public Desk desk;
    public GameObject parrent;
    public TablePattern()
    {
        parrent = GameObject.Find ("CentreOfTable");
		localDeck = new Deck(Game.None, "None", parrent);
    }
    public TablePattern(List<Card> cards)
    {
        parrent = GameObject.Find ("CentreOfTable");
        localDeck = new Deck(Game.None, "None", parrent);
        foreach (var item in cards)
        {
            item.isOnScreen = false;
            localDeck.Cards.Add(item);
        }
    }
	public bool AddCard(Card card)
	{
		card.owner = patternName;
		localDeck.Cards.Add(card);
		isNewCards = true;
		return true;
	}
    public bool Render()
    {
        RenderMaster.Render(this, new Vector3 (0, 0, 0));
        /*float x = 2.5f;
		float y = 2 * x;
		float z = x;
        GameObject handCard;
        for(int i = 0; i < localDeck.Cards.Count; ++i)
        {
            if (!localDeck.Cards[i].isOnScreen)
            {
                handCard = (GameObject)GameObject.Instantiate(Resources.Load("FPC/PlayingCards_" + localDeck.Cards[i].value + localDeck.Cards[i].GetSuit())); //скачал бесплатные карты
				handCard.transform.position = new Vector3(z - x * localDeck.Cards.Count + y * i, -5.5f, 31f - 0.1f * i) + obj.transform.position;
                handCard.transform.rotation *= Quaternion.AngleAxis(-90, new Vector3(1, 0, 0)); //повернул на -90 по оси X
                handCard.transform.localScale += new Vector3(60, 60, 0); //увеличил в 60 раз
                
                localDeck.Cards[i].gameobj = handCard;
                localDeck.Cards[i].isOnScreen = true;
				handCard.gameObject.transform.SetParent(obj.transform);
                //Debug.Log(Cards[i].value + " " + Cards[i].type);
            }
            else
            {
                localDeck.Cards[i].gameobj.transform.position = new Vector3(z - x * localDeck.Cards.Count + y * i, -5.5f, 31f - 0.1f * i);
            }
        }
		isNewCards = false;
        */
        return true;
    }
}

public class Deck
{
    public string name;
    public Game game;
    public List<Card> Cards { get; set; }
    public GameObject desk;

    public Deck(Game game, string name, GameObject desk)
    {
        this.desk = desk;
        this.name = name;
        this.game = game;
        Cards = new List<Card>();
        if (game == Game.Poker || game == Game.BlackJack)
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                Cards.Add(new Card("A", suit, name));
                for (int y = 2; y < 11; y++)
                {
                    Cards.Add(new Card(y.ToString(), suit, name));
                }
                Cards.Add(new Card("J", suit, name));
                Cards.Add(new Card("Q", suit, name));
                Cards.Add(new Card("K", suit, name));
            }
        }
        if(game == Game.UNO)
        {
            
			for (int i = 0; i < 4; i++)
            {
				Cards.Add(new Card("2Cards", Card.GetColor(i), name));
				Cards.Add(new Card("CD", Card.GetColor(i), name));
				Cards.Add(new Card("Skip", Card.GetColor(i), name));
				Cards.Add(new Card("CC", Card.GetColor(4), name));
				Cards.Add(new Card("4Cards", Card.GetColor(4), name));//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
				for (int j = 0; j < 10; j++)
                {
					Cards.Add(new Card(j.ToString(), Card.GetColor(i), name));
                }
            }
        }
    }

	public void Shuffle()
	{        
		Card c;
		for (int j, i = Cards.Count - 1; i > 0; i--)
		{
			j = UnityEngine.Random.Range(0, i);
			c = Cards[j];
			Cards[j] = Cards[i];
			Cards[i] = c;
		}

	}

    public void AddCard(Card card)
    {
        card.owner = name;
        Cards.Add(card);
    }

    public void Render(int shift = 0, bool flip = false)
    {
        Quaternion quat = Quaternion.AngleAxis(flip ? 50 : 230, new Vector3(1, 0, 0));
		quat *= Quaternion.AngleAxis(180, new Vector3(0, 0, 1));
        RenderMaster.Render(this, new Vector3(21 - shift*1.3f , 0, 50), quat);
    }
}

public class TurnController
{
    Type type;
    public TurnController()
    {

    }
    public bool Conditions()
    {
        return true;
    }


}