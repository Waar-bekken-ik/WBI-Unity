using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PelvisSectionShower : MonoBehaviour
{
    #region Singleton
    private static PelvisSectionShower _instance;
    public static PelvisSectionShower Instance
    {
        get
        {
            if (_instance == null)
            {
                // PelvisSectionShower gm = new PelvisSectionShower("PelvisSectionShower");
                // gm.AddComponent<PelvisSectionShower>();
            }

            return _instance;
        }
    }
    #endregion

    public List<GameObject> parts = new List<GameObject>();
    public GameObject mainCamera;
    private Vector3 startPosCamera;
    private Vector3 startRotCamera;
    public Image fader;
    private Color alphaChange;
    private Color startAlphaColor;
    private bool fadeIn = false;
    private bool fadeOut = false;

    private void Awake() 
    {
        //start values
        _instance = this;
        startPosCamera = mainCamera.transform.position;
        startRotCamera = new Vector3(mainCamera.transform.rotation.x, mainCamera.transform.rotation.y, mainCamera.transform.rotation.z);
        alphaChange = fader.color;
        startAlphaColor = fader.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        //MoveTowards("vagina");
        StartCoroutine(testRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeIn && alphaChange.a <= 1)
        {
            alphaChange.a += 0.5f * Time.deltaTime;
            fader.color = alphaChange;
            //Debug.Log("fading in");
        }
        else if(fadeIn && alphaChange.a >= 1)
        {
            fadeIn = false;
            //Debug.Log("fading in off");
        }

        if (fadeOut && alphaChange.a >= 0)
        {
            alphaChange.a -= 0.5f * Time.deltaTime;
            fader.color = alphaChange;
            //Debug.Log("fading out");
        }
        else if(fadeOut && alphaChange.a <= 0)
        {
            fadeOut = false;
            resetColor();
            //Debug.Log("fading out off");
        }
    }

    public void MoveTowards(string name)
    {
        foreach(GameObject part in parts)
        {
            if(name == part.name)
            {
                StartCoroutine(fadeRoutine(part));
                break;
            }
        }
    }

    public void ResetCamera()
    {
        StartCoroutine(reset());
    } 

    private void resetColor()
    {
        fader.color = startAlphaColor;
    }

    private IEnumerator fadeRoutine(GameObject part)
    {
        fadeIn = true;
        yield return new WaitForSeconds(2.5f);
        mainCamera.transform.position = new Vector3(part.transform.position.x + part.GetComponent<Part>().XposCorrection, 
                                                            part.transform.position.y + part.GetComponent<Part>().YposCorrection, 
                                                            part.transform.position.z + part.GetComponent<Part>().ZposCorrection);
        mainCamera.transform.rotation = part.transform.rotation;
        fadeOut = true;
    }

    private IEnumerator reset() 
    {
        fadeIn = true;
        yield return new WaitForSeconds(2.5f);
        fadeOut = true;

        mainCamera.transform.position = startPosCamera;
        mainCamera.transform.rotation = Quaternion.Euler(startRotCamera);
    }

    private IEnumerator testRoutine()
    {
        MoveTowards("vagina");
        yield return new WaitForSeconds(5f);
        MoveTowards("eierstokken");
        yield return new WaitForSeconds(5f);
        MoveTowards("anus");
        yield return new WaitForSeconds(5f);
        MoveTowards("endeldarm");
        yield return new WaitForSeconds(5f);
        ResetCamera();
    }
}
