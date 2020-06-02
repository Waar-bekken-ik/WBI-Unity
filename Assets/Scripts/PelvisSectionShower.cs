using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake() 
    {
        //start values
        _instance = this;
        startPosCamera = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        startRotCamera = new Vector3(mainCamera.transform.rotation.x, mainCamera.transform.rotation.y, mainCamera.transform.rotation.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveTowards("vagina");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoveTowards(string name)
    {
        foreach(GameObject part in parts)
        {
            if(name == part.name)
            {
                mainCamera.transform.position = new Vector3(part.transform.position.x + part.GetComponent<Part>().XposCorrection, 
                                                            part.transform.position.y + part.GetComponent<Part>().YposCorrection, 
                                                            part.transform.position.z + part.GetComponent<Part>().ZposCorrection);

                mainCamera.transform.rotation = part.transform.rotation;

                break;
            }
        }
    }

    public void ResetCamera()
    {
        mainCamera.transform.position = startPosCamera;
        mainCamera.transform.rotation = Quaternion.Euler(startRotCamera);
    }
}
