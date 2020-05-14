using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeGameUI : MonoBehaviour
{
    public InputField rounds;
    public InputField time;
    public Dropdown question;

    public Text roomCode;
    public Text people;

    public GameObject makePanel;
    public GameObject startPanel;

    public void MakeGame()
    {
        makePanel.SetActive(false);
        startPanel.SetActive(true);
        string[] questions = new string[] { "eierstokken", "schaamlippen" };

        GameMaster.Instance.game.setMakeRoom(int.Parse(rounds.text), int.Parse(time.text), questions);
    }

    public void addPlayerName(string name)
    {
        people.text = name + "\n";
        Debug.Log("IN KANKER NAME CHANGE UI");
    }

    public void setPinCode()
    {
        roomCode.text = GameMaster.Instance.game.getPin();
    }

    public void StartGame()
    {

    }
}
