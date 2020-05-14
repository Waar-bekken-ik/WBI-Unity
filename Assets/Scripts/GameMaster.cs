using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //jaja private maken met gameobject.find getcomponent<> etc --still testing and concpet fase
    public Game game;
    public MakeGameUI makeUI;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void playerSubscribed(string name)
    {
        game.setPlayers();
        makeUI.addPlayerName(name);
        Debug.Log("IN KANKER NAME CHANGE");
    }

    public void setPinCodeUI()
    {
        makeUI.setPinCode();
    }

    public void makeRoom()
    {

    }
}
