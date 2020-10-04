using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlight : MonoBehaviour
{
    Material sharedDefaultMat;
    new MeshRenderer renderer;
    [Header("Highlight")]
    [SerializeField] Material highlightMat = null;
    [SerializeField] float highlightLerpTime = 1;
    Material instancedHighlightMat;
    public bool isHighlighted { get; private set; }

    [Header("Hovering")]
    [SerializeField] Material hoveredMaterial = null;
    public bool isHovered { get; private set; }


    private void Start()
    {
        InitMaterials();
    }

    public void OnHoverEnter()
    {
        isHovered = true;

        if (!isHighlighted)
            renderer.material = hoveredMaterial;
    }

    public void OnHoverExit()
    {
        isHovered = false;

        if (!isHighlighted)
            renderer.material = sharedDefaultMat;
    }

    public void SetHighlightOn()
    {
        if (isHighlighted)
            return;

        StopAllCoroutines();
        StartCoroutine(HighlightLerp(0,1,highlightLerpTime));
        isHighlighted = true;
    }

    public void SetHighlightOff()
    {
        if (!isHighlighted)
            return;

        StopAllCoroutines();
        StartCoroutine(HighlightLerp(1, 0, highlightLerpTime*0.5f));

        isHighlighted = false;

    }

    IEnumerator HighlightLerp(float start, float end, float time)
    {
        if (end > 0)
            renderer.material = instancedHighlightMat;

        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / time;

            instancedHighlightMat.SetFloat("_Highlight", Mathf.Lerp(start,end,t));

            yield return null;
        }

        if (end == 0)
        {
            if(isHovered)
                renderer.material = hoveredMaterial;
            else
                renderer.material = sharedDefaultMat;
        }
    }

    void InitMaterials()
    {
        renderer = GetComponent<MeshRenderer>();
        instancedHighlightMat = new Material(highlightMat);
        sharedDefaultMat = renderer.sharedMaterial;
    }
}
