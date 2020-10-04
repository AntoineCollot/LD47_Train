using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    List<Transform> windBones = new List<Transform>();

    [SerializeField] float windIntensityMutliplier = 3;
    [SerializeField] float windEvolutionSpeed = 1;
    [SerializeField] float windSpatialAmplitude = 1;
    public Vector2 windDirection = Vector2.right;
    float windOffset;

    public static Wind Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        windOffset = Random.Range(0, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        windOffset += Time.deltaTime * windEvolutionSpeed;
        ApplyWind();
    }

    void ApplyWind()
    {
        windDirection.Normalize();
        Vector3 baseRotation = new Vector3(windDirection.y, 0, windDirection.x);
        Vector3 rotation;
        foreach (Transform windBone in windBones)
        {
            rotation = windBone.InverseTransformVector(baseRotation * GetWindIntensity(windBone.position));
            windBone.localEulerAngles = rotation;
        }
    }

    public void RegisterBone(Transform windBone)
    {
        windBones.Add(windBone);
    }

    float GetWindIntensity(Vector3 position)
    {
        return Mathf.PerlinNoise(position.x*windSpatialAmplitude, position.z * windSpatialAmplitude + windOffset) * windIntensityMutliplier;
    }
}
