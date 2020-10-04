using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask tileLayers = int.MaxValue;

    //Overing
    GameObject hoveredObject;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Overing
        GameObject newHoveredObject = RaycastMouse();
        //If the object hovered changed
        if(newHoveredObject != hoveredObject)
        {
            if (hoveredObject != null)
                hoveredObject.SendMessage("OnHoverExit");

            if (newHoveredObject!=null)
            {
                newHoveredObject.SendMessage("OnHoverEnter");
            }
            hoveredObject = newHoveredObject;
        }

        //Clicks
        if (Input.GetMouseButtonDown(0))
        {
            if(newHoveredObject != null)
                newHoveredObject.SendMessage("OnLeftClick", SendMessageOptions.DontRequireReceiver);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (newHoveredObject != null)
                newHoveredObject.SendMessage("OnRightClick", SendMessageOptions.DontRequireReceiver);
        }

        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Space))
        {
            if (newHoveredObject != null)
                newHoveredObject.SendMessage("OnCancelClick", SendMessageOptions.DontRequireReceiver);
        }
    }

    GameObject RaycastMouse()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, tileLayers))
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
