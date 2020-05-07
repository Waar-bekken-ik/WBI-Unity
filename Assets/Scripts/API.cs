using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public class API : MonoBehaviour
{
    private readonly string baseURL = "https://pokeapi.co/api/v2/";

    private void Start()
    {
        StartCoroutine(GetData());
    }

    //change name to more appropiate name for the job ;)
    IEnumerator GetData()
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

    private string CapitializeFirstCharacter(string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }

}
