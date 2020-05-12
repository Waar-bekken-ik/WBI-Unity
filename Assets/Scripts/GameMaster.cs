using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public string roomPin;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
