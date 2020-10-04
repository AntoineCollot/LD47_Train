using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnvironmentItem", order =0)]
public class EnvironmentItem : ScriptableObject
{
    public Transform prefab;

    [Header("Randomize")]
    public float probability = 1;
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public bool rotateZOnly = true;

    [Header("Wind")]
    public bool applyWind = true;
}
