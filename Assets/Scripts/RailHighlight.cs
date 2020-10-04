using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailHighlight : MonoBehaviour
{
    new Renderer renderer = null;
    [SerializeField] Material highlightMetalMat = null;
    //[SerializeField] Material highlightWoodMat = null;
    [SerializeField] Material lockedMetalMat = null;
    //[SerializeField] Material lockedWoodMat = null;
    Material defaultMetalMat = null;
    //Material defaultWoodMat = null;

    public void SetHighlightOn()
    {
        renderer = GetComponent<Renderer>();
        defaultMetalMat = renderer.sharedMaterials[0];
       // defaultWoodMat = renderer.sharedMaterials[1];
        renderer.material = new Material(highlightMetalMat);
       // renderer.materials[1] = highlightWoodMat;
    }

    public void SetLockedMaterial()
    {
        if(renderer==null)
            renderer = GetComponent<Renderer>();
        renderer.material = new Material(lockedMetalMat);
       // renderer.materials[1] = lockedWoodMat;
    }

    public void SetHighlightOff()
    {
        if(renderer != null)
        {
            renderer.material.SetFloat("_Highlight", 0);
            //renderer.material = defaultMetalMat;
            //renderer.materials[1] = defaultWoodMat;
        }
    }
}
