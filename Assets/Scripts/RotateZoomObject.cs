using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZoomObject : MonoBehaviour
{
    float speed = 20;

    void OnMouseDrag() 
    {
        float rotX = Input.GetAxis("Mouse X")*speed*Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y")*speed*Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }
}
