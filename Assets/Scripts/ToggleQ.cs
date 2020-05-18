using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleQ : MonoBehaviour
{
    public Text labelText;
    public Toggle toggleCheck;

    public void SetName(string name)
    {
        labelText.text = name;
    }

    public void changeToggle()
    {
        if(toggleCheck.isOn)
        {
            GameMaster.Instance.game.addQuestion(labelText.text);
        }
        else
        {
            GameMaster.Instance.game.removeQuestion(labelText.text);
        }
    }
}
