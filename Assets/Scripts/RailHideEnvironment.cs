using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailHideEnvironment : MonoBehaviour
{
    List<GameObject> hiddenObjects = new List<GameObject>();

    private void OnDestroy()
    {
        foreach(GameObject hiddenObject in hiddenObjects)
        {
            if(hiddenObject!=null)
                hiddenObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Hide the object
        other.gameObject.SetActive(false);
        hiddenObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        hiddenObjects.Remove(other.gameObject);
        other.gameObject.SetActive(true);
    }
}
