using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadMySkin : NetworkBehaviour {

    [SyncVar] public string cardName;
    [SyncVar] public int index;
    [SyncVar] public string owner;
    // Use this for initialization
    void Start ()
    {
        if (isServer)
        {
            cardName = name;
            owner = transform.parent.name;
            string[] buf = name.Split('_');
            index = -1;
            if (buf.Length > 1)
            {
                index = System.Convert.ToInt32(buf[1]);
            }
        }
        if (owner == "Desk")
        {
            return;
        }
        LoadData();
    }
    void LoadData()
    {
        if (isClient)
        {
            name = cardName;
            transform.SetParent(GameObject.Find(owner).transform);
        }

        HandController hand = GetComponentInParent<HandController>();
        if (hand != null)
        {
            if (index >= 0 && index < hand.Cards.Count)
            {
                GetComponent<Renderer>().material.mainTexture = (Texture)GameObject.Instantiate(Resources.Load
                ("UNOcards/sliced_sprites/unos_" + (hand.Cards[index].GetValue() + 1 + 14 * hand.Cards[index].GetColor()).ToString()));
            }
        }
    }
    private void Update()
    {
        if (transform.parent != null)
        {
            if (transform.parent.name == "Temp")
            {
                LoadData();
            }
        }
    }
}
