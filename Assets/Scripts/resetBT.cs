using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetBT : MonoBehaviour
{
    public Component[] parts;

    public void ResetParts()
    {
        parts = GetComponentsInChildren<TogglePart>();
        int counter = 0;
        
        foreach(TogglePart part in parts)
        {
            part.addPart(true);
            parts[counter] = null;
            counter++;
        }

        counter = 0;
    }
}
