using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMananer : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask clickRaycastLayers = int.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject hitObject = RaycastMouse();
            hitObject.SendMessage("OnLeftClick", SendMessageOptions.DontRequireReceiver);
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject hitObject = RaycastMouse();
            hitObject.SendMessage("OnRightClick", SendMessageOptions.DontRequireReceiver);
        }
    }

    GameObject RaycastMouse()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickRaycastLayers))
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
