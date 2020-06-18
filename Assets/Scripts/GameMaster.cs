using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameMaster : MonoBehaviour
{
    //jaja private maken met gameobject.find getcomponent<> etc --still testing and concpet fase
    public Game game;
    public MakeGameUI makeUI;
    public List<string> availableQuestions = new List<string>();

    #region Singleton
    private static GameMaster _instance;
    public static GameMaster Instance
    {
        get
        {
            if (_instance == null)
            {
                // GameMaster gm = new GameMaster("GameMaster");
                // gm.AddComponent<GameMaster>();
            }

            return _instance;
        }
    }
    #endregion

    void Awake()
    {
        //start values
        _instance = this;

        StartCoroutine(API.Instance.GetAnswers());
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(API.Instance.GetAnswers());
    }

    public void playerSubscribed(string name)
    {
        game.setPlayers();
        //makeUI.addPlayerName(name);
    }

    public void setPinCodeUI()
    {
        makeUI.setPinCode();
    }

    public void makeRoom()
    {

    }
}
