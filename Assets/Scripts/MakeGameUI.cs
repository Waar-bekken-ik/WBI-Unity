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
    public GameObject questionPanel;

    private List<string> dublicateNames = new List<string>();
    private bool joining = true;
    private bool playing = false;

    public GameObject scrollViewContent;
    public GameObject myToggle;

    void Start() 
    {
        //als api request er is de data daarmee aanvullen
        string[] questions = new string[] { "eierstokken", "schaamlippen", "dunnedarm", "dikkedarm", "schaamlippen"};
        float startPos = 135;

        foreach(string question in questions)
        {
            var toggle = Instantiate(myToggle, new Vector3(0,0,0), Quaternion.identity);
            toggle.transform.SetParent(scrollViewContent.transform);
            toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-6,startPos);
            toggle.GetComponent<ToggleQ>().SetName(question);
            startPos -= 20f;
        }
    }

    public void MakeGame()
    {
        makePanel.SetActive(false);
        startPanel.SetActive(true);

        GameMaster.Instance.game.setMakeRoom(int.Parse(rounds.text), int.Parse(time.text));
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
        GameMaster.Instance.game.startGame();
        joining = false;
        makePanel.SetActive(false);
        startPanel.SetActive(false);
        questionPanel.SetActive(true);

        //InCoroutine straks:
        playing = true;
    }

    void Update()
    {
        if(joining)
        {
            fillPlayers();
        }
        
        if(playing)
        {
            //fillPlayerAnswers();
            //verder gaan vanaf hier
        }
    }

    private void fillPlayers()
    {
        //pusher werkt kut, sorry voor deze lelijke slechte code;
        foreach (string player in PusherManager.instance.getPlayerNames())
        {
            if (!dublicateNames.Contains(player))
            {
                dublicateNames.Add(player);
                people.text += player + "\n";
                break;
            }
        }
    }
}
