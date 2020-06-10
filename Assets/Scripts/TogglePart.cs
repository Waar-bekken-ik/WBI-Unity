using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePart : MonoBehaviour
{
    public Text labelText;
    public GameObject choosenPart;
    public GameObject choosenPartContent;
    public Text counterText;

    private void Start() 
    {
        choosenPartContent = GameObject.Find("PanelPartsChoosen");
        counterText = GameObject.Find("partsCounterText").GetComponent<Text>();
    }
    
    public void SetName(string name)
    {
        labelText.text = name;
    }

    public void addPart(bool delete)
    {
        if(!delete && GameMaster.Instance.game.getQuestionsCount() < 8)
        {
            GameMaster.Instance.game.addQuestion(labelText.text);

            //add too right side ;))) <- dit stuk code kan veel beter in een andere class, want nu heb je dit zovaak als de aantal delen die je hebt ;)
            float startPos = 467;
            float padding = 137;

            var toggle = Instantiate(choosenPart, new Vector3(0,0,0), Quaternion.identity);
            toggle.transform.SetParent(choosenPartContent.transform);

            toggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(2,startPos - (padding * (GameMaster.Instance.game.getQuestionsCount() - 1)));
            toggle.GetComponent<TogglePart>().SetName(labelText.text);

        }
        else
        {
            if(delete)
            {
                GameMaster.Instance.game.removeQuestion(labelText.text);
                Destroy(this.gameObject);
            }
        }

        counterText.text = "(" + GameMaster.Instance.game.getQuestionsCount().ToString() + "/8)";
    }
}
