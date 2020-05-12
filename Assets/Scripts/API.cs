using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

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

    private readonly string baseURL = "https://pokeapi.co/api/v2/";

    void Awake()
    {
        //start values
        _instance = this;
    }

    private void Start()
    {
        StartCoroutine(GetData());
    }

    //change name to more appropiate name for the job ;)
    private IEnumerator GetData()
    {
        Debug.Log("API ROUTINE STARTED");

        string URL = baseURL + "pokemon/50";

        UnityWebRequest infoRequest = UnityWebRequest.Get(URL);
        yield return infoRequest.SendWebRequest();

        if (infoRequest.isNetworkError || infoRequest.isHttpError)
        {
            Debug.Log(infoRequest.error);
            Debug.Log("ERROR");
            yield break;
        }

        JSONNode info = JSONNode.Parse(infoRequest.downloadHandler.text);

        string pokeName = info["name"];
        string pokeSPriteURL = info["sprites"]["front_default"];

        JSONNode pokeTypes = info["types"];
        string[] pokeTypeNames = new string[pokeTypes.Count];

        for (int i = 0, j = pokeTypes.Count -1; i < pokeTypes.Count; i++, j--)
        {
            pokeTypeNames[j] = pokeTypes[i]["type"]["name"];
        }

        Debug.Log("MADE IT SHIT FAR");

        Debug.Log(pokeName + "\n" + pokeSPriteURL + "\n" + pokeTypeNames.ToString());
    }

    //change name to more appropiate name for the job ;)
    private IEnumerator SendData()
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        using (UnityWebRequest www = UnityWebRequest.Post("http://www.my-server.com/myform", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    private string CapitializeFirstCharacter(string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }

}
