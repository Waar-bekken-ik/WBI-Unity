using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textShadow : MonoBehaviour
{
    public Text mainText;
    public Text shadowText;
    // Update is called once per frame
    void Update()
    {
        shadowText.text = mainText.text;
    }
}
