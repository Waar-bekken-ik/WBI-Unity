using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BScript : MonoBehaviour
{
    public Camera cam;

    public GameObject pelvis;
    private float[] vectors = new float[]{0.5f, 0.5f, 0.5f};
    private bool stopRotate = false;
    private bool startTest = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        if (startTest)
        {
            //380
            if (cam.transform.position.z >= 385)
            {
                if (!stopRotate)
                {
                    //pelvis
                    vectors[0] += Random.Range(0.1f, 0.6f);
                    vectors[1] += Random.Range(0.1f, 0.6f);
                    vectors[2] += Random.Range(0.1f, 0.6f);

                    pelvis.transform.Rotate(vectors[0], vectors[1], vectors[2], Space.Self);
                }
            }
            else
            {
                cam.transform.Translate(Vector3.forward * (Time.deltaTime * 500));
            }
        }
    }

    private IEnumerator timer()
    {
        yield return new WaitForSeconds(1);
        startTest = true;
        yield return new WaitForSeconds(2);
        stopRotate = true;
    }
}
