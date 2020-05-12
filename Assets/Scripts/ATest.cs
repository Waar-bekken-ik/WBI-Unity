using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATest : MonoBehaviour
{
    public Image fader;
    public GameObject camera;
    private Color alphaChange;
    private bool startTest = false;
    private bool startZoom = false;

    // Start is called before the first frame update
    void Start()
    {
        alphaChange = fader.color;

        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        if(startTest)
        {
            
                alphaChange.a += 0.5f * Time.deltaTime;
                fader.color = alphaChange;
                Debug.Log("in test setter");
            
        }

        if (startZoom)
        {
            
          
                alphaChange.a -= 0.5f * Time.deltaTime;
                fader.color = alphaChange;
                Debug.Log("in zoom setter");
            
        }
    }

    private IEnumerator timer()
    {
        yield return new WaitForSeconds(2);
        startTest = true;
        yield return new WaitForSeconds(2.2f);
        camera.transform.position = new Vector3(-65,-40,400);
        startTest = false;
        startZoom = true;
    }
}
