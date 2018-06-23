using UnityEngine;
using UnityEngine.UI;

public class CPtoogle : MonoBehaviour {

    HandController currentPlayer;
    public GameObject CPButton;
	void Start () {
        Toogle(false);
    }
    public void Activate(HandController player)
    {
        currentPlayer = player;
        ChangePlayerText(currentPlayer.playerName);
        Toogle(true);
    }

    public void Toogle() {
        if (gameObject.activeSelf)
        {
            Toogle(false);
        }
        else
        {
            Toogle(true);
        }
    }

    public void Toogle(bool toogle) {
        gameObject.SetActive(toogle);
        CPButton.SetActive(toogle);
        if (toogle == false && currentPlayer != null)
        {
            currentPlayer.isMyTurn = true;
        }
    }

    private void ChangePlayerText(string playerName)
    {
        transform.Find("Text").GetComponent<Text>().text = "Ходит игрок:\n" + playerName;
    }
}
