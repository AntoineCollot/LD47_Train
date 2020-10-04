using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] bool reverse = false;
    // Start is called before the first frame update
    void Start()
    {
        if(!reverse)
            transform.LookAt(Camera.main.transform.position);
        else
        {
            Vector3 lookAtPos = Camera.main.transform.position * 2 - transform.position;
            transform.LookAt(lookAtPos);
        }
    }
}
