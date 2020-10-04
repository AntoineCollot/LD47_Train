using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEnvironmentOnContact : MonoBehaviour
{
    List<Renderer> hiddenObjects = new List<Renderer>();

    private void OnDestroy()
    {
        foreach(Renderer hiddenObject in hiddenObjects)
        {
            if (hiddenObject != null)
                hiddenObject.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Hide the object
        Renderer r = other.GetComponentInChildren<Renderer>();
        if (r != null)
        {
            r.enabled = false;
            hiddenObjects.Add(r);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Renderer r = other.GetComponentInChildren<Renderer>();
        if(r!=null)
        {
            r.enabled = true;
            hiddenObjects.Remove(r);
        }
    }
}
