using System.Collections.Generic;
using UnityEngine;
using System;

static public class RenderMaster {

	//храним ссылки на все руки, которые отрисовали, на все колоды и все Паттерны
    //При рисовании новой колоды/руки учитываем кол-во уже отрисованых колод/рук
    static public bool Render(TablePattern pattern, Vector3 position)
    {
        //вводим настройки для данного типа рендера.

        float x = 5; //z - x * Cards.Count + y * i
        float y = 0; //-5.5f
        float z = -0.1f; //31f - 0.1f * i
        float constX = 2.5f - 2.5f;
        float constY = -5.5f;
        float constZ = 31f;
        position += new Vector3(constX, constY, constZ);
        Quaternion rotation = Quaternion.AngleAxis(-90, new Vector3(1, 0, 0));
        Load(pattern.localDeck.Cards, position, rotation, pattern.parrent, x, y, z, Game.UNO);
        pattern.isNewCards = false;
        return true;
    }
    static public bool Render(Deck localdeck, Vector3 position, Quaternion rotation)
    {
        float x = 0; //z - x * Cards.Count + y * i
        float y = 0 + 0.1f; //-5.5f
        float z = 0; //31f - 0.1f * i
		//position += new Vector3(0, 0,-5); 
        Load(localdeck.Cards, position, rotation, localdeck.desk, x, y, z, Game.UNO);
        return true;
    }
    static public bool Render(HandController hand, Vector3 position)
	{
		//x = 0; y = -8.5; z = 8; 
		if(hand.Cards.Count >=	 1)
		{
            //вводим настройки для данного типа рендера.
			float x = 4; //z - x * Cards.Count + y * i
            float y = 0; //-5.5f
            float z = 0; //31f - 0.1f * i
			float constX =  -5;
			float constY = -4; //-12
            if (String.Compare(hand.playerName, "Player2") == 0)
            {
                //constY = -18f;
            }
            float constZ = -5;
            position += new Vector3(constX, constY, constZ);
            Quaternion rotation = Quaternion.AngleAxis(180, new Vector3(0, 0, 1));
            rotation *= Quaternion.AngleAxis(-40, new Vector3(1, 0, 0));
            Load(hand.Cards, position, rotation, hand.gameObject, x, y, z, Game.UNO);
            hand.cardLine = hand.Cards [hand.Cards.Count - 1].gameobj.transform.position;
		}
		hand.isNewCards = false;
        return true;
    }

    static public bool RenderChangeColor(Vector3 position, bool isCCPlayed)
    {
        if (isCCPlayed)
        {
            Quaternion rotation = Quaternion.AngleAxis(180, new Vector3(0, 0, 1));
            for (int i = 0; i < 4; i++)
            {
                GameObject gmObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Card"));
                gmObj.GetComponent<Renderer>().material.mainTexture = (Texture)GameObject.Instantiate(Resources.Load("UNOcards/images/unos_empty_" + i));
                gmObj.transform.position = position + new Vector3(6 * i, 0, 0);
                gmObj.transform.rotation *= rotation;
                gmObj.gameObject.transform.SetParent(GameObject.Find("ChangeColor").transform);
                if (gmObj.GetComponent<Collider>() == null)
                {
                    gmObj.AddComponent<BoxCollider>();
                }
                string name = "NoName";
                switch (i)
                {
                    case 0:
                        name = "Red";
                        break;
                    case 1:
                        name = "Yellow";
                        break;
                    case 2:
                        name = "Green";
                        break;
                    case 3:
                        name = "Blue";
                        break;
                }
                gmObj.name = name;
            }
        }
        return true;
    }

    static bool Load(List<Card> Cards, Vector3 position, Quaternion rotation, GameObject parrent, float changeX, float changeY, float changeZ)
    {
        for (int i = 0; i < Cards.Count; ++i)
        {
            if (!Cards[i].isOnScreen)
            {
                GameObject gmObj = (GameObject)GameObject.Instantiate(Resources.Load("FPC/PlayingCards_" + Cards[i].value + Cards[i].GetSuit()));
                gmObj.transform.localScale = new Vector3(10, 10, 0.05f);
				if (parrent != null) 
				{
					gmObj.gameObject.transform.SetParent(parrent.transform);
				}
				gmObj.transform.position = position + new Vector3(changeX * i, changeY * i, changeZ * i);
                gmObj.transform.rotation *= rotation;
				if (gmObj.GetComponent<Collider> () == null) 
				{
					gmObj.AddComponent<BoxCollider>();
				}
				gmObj.name = String.Concat(Cards[i].owner, "_", i);
                Cards[i].gameobj = gmObj;
                Cards[i].isOnScreen = true;
            }
            else
            {
				Cards[i].gameobj.transform.position = position + new Vector3(changeX * i, changeY * i, changeZ * i);
                Cards[i].gameobj.name = String.Concat(Cards[i].owner, "_", i);
            }

        }
        return true;
    }

    static bool Load(List<Card> Cards, Vector3 position, Quaternion rotation, GameObject parrent, float changeX, float changeY, float changeZ, Game game)
    {
        if (game == Game.UNO)
        {
            for (int i = 0; i < Cards.Count; ++i)
            {
                if (!Cards[i].isOnScreen)
                {
                    GameObject gmObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Card"));
                    Debug.Log ((Cards [i].GetValue () + 14 * Cards [i].GetColor ()).ToString ());
					gmObj.GetComponent<Renderer>().material.mainTexture = (Texture)GameObject.Instantiate(Resources.Load
						("UNOcards/images/unos_" + (Cards[i].GetValue() + 1 + 14*Cards[i].GetColor()).ToString()));

                    gmObj.transform.localScale = new Vector3(4, 6, 0.05f);
                    if (parrent != null)
                    {
                        gmObj.gameObject.transform.SetParent(parrent.transform);
                    }
					gmObj.transform.position = position + new Vector3(changeX * i, changeY * i, changeZ * i);
                    gmObj.transform.rotation *= rotation;
                    if (gmObj.GetComponent<Collider>() == null)
                    {
                        gmObj.AddComponent<BoxCollider>();
                    }
                    gmObj.name = String.Concat(Cards[i].owner, "_", i);
                    Cards[i].gameobj = gmObj;
                    Cards[i].isOnScreen = true;
                }
                else
                {
                    Cards[i].gameobj.transform.position = position + new Vector3(changeX * i, changeY * i, changeZ * i);
                    Cards[i].gameobj.name = String.Concat(Cards[i].owner, "_", i);
                }
                
            }
        }
        else
        {
            Load(Cards, position, rotation, parrent, changeX, changeY, changeZ); //Если игра != UNO, тогда подгружаем стандартные игральные карты
        }

        return true;
    }
}
