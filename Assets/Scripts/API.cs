using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using System.Text;

public class API : MonoBehaviour
{
    #region Singleton
    private static API _instance;
    public static API Instance
    {
        get
        {
            if (_instance == null)
            {
                // API api = new API("API");
                // api.AddComponent<API>();
            }

            return _instance;
        }
    }
    #endregion

    private readonly string baseURL = "http://78.141.212.87:8000/";

    void Awake()
    {
        //start values
        _instance = this;
    }

    private void Start()
    {
        //StartCoroutine(GetData());
    }

    //change name to more appropiate name for the job ;)
    public IEnumerator GetAnswers()
    {
        string URL = baseURL + "answers/getanswers";

        UnityWebRequest infoRequest = UnityWebRequest.Get(URL);
        yield return infoRequest.SendWebRequest();

        if (infoRequest.isNetworkError || infoRequest.isHttpError)
        {
            Debug.Log(infoRequest.error);
            Debug.Log("ERROR");
            yield break;
        }

        JSONNode info = JSONNode.Parse(infoRequest.downloadHandler.text);

        // string pokeName = info["name"];
        // string pokeSPriteURL = info["sprites"]["front_default"];

        JSONNode answers = info["answers"];
        // string[] pokeTypeNames = new string[answers.Count];
        List<string> answersList = new List<string>();

        // for (int i = 0, j = answers.Count -1; i < answers.Count; i++, j--)
        // {
        //     pokeTypeNames[j] = answers[i]["type"]["name"];
        // }

        for(int i = 0; i < answers.Count; i++)
        {
            answersList.Add(answers[i]);
            //Debug.Log(answers[i]);
        }

        GameMaster.Instance.availableQuestions = answersList;

        //Debug.Log(pokeName + "\n" + pokeSPriteURL + "\n" + pokeTypeNames.ToString());
    }

    //change name to more appropiate name for the job ;)
    public IEnumerator MakeGame(int rounds, int time, List<string> questions)
    {
        WWWForm form = new WWWForm();
        form.AddField("rounds", rounds);
        form.AddField("time", time);

        foreach(string question in questions)
        {
            form.AddField("questions[]", question);
        }

        string URL = baseURL + "games/makegame";

        using (UnityWebRequest www = UnityWebRequest.Post("http://78.141.212.87:8000/games/makegame", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log("Form upload complete!");

                // Print Body
                Debug.Log(www.downloadHandler.text);

                JSONNode info = JSONNode.Parse(www.downloadHandler.text);

                string pin = info["pin"];
                string id = info["_id"];
      
                GameMaster.Instance.game.setPin(pin);
                Debug.Log(pin);
                PusherManager.instance.StartPusher();
                GameMaster.Instance.setPinCodeUI();
            }
        }
    }

     //change name to more appropiate name for the job ;)
    public IEnumerator StartGame(string pin)
    {
        WWWForm form = new WWWForm();
        form.AddField("pin", pin);

        using (UnityWebRequest www = UnityWebRequest.Post("http://78.141.212.87:8000/games/startgame", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log("Form upload complete!");

                // Print Body
                Debug.Log(www.downloadHandler.text);

                //JSONNode info = JSONNode.Parse(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator NextQuestion(string pin, string answer, bool last)
    {
        WWWForm form = new WWWForm();
        form.AddField("pin", pin);
        form.AddField("correctAnswer", answer);

        if(last)
        {
            form.AddField("lastQuestion", 1);
        }

        using (UnityWebRequest www = UnityWebRequest.Post("http://78.141.212.87:8000/games/nextquestion", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log("Form upload complete!");

                // Print Body
                Debug.Log(www.downloadHandler.text);

                //JSONNode info = JSONNode.Parse(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator correctAnswer(string pin, string answer)
    {
        WWWForm form = new WWWForm();
        form.AddField("pin", pin);
        form.AddField("correctAnswer", answer);

        using (UnityWebRequest www = UnityWebRequest.Post("http://78.141.212.87:8000/games/sendcorrectanswer", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log("Form upload complete!");

                // Print Body
                Debug.Log(www.downloadHandler.text);

                //JSONNode info = JSONNode.Parse(www.downloadHandler.text);
            }
        }
    }

    private string CapitializeFirstCharacter(string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}
