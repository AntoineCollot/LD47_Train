using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentItemSpawner : MonoBehaviour
{
    [SerializeField] EnvironmentItem[] items;

    [SerializeField] AnimationCurve itemSpawnCountCurve;
    [SerializeField,Range(0,1)] float spawnableDistFromCenter = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        int itemsToSpawnCount = Mathf.FloorToInt(itemSpawnCountCurve.Evaluate(Random.Range(0f, 1f)));

        float totalProbabilies = 0;
        foreach(EnvironmentItem item in items)
        {
            totalProbabilies += item.probability;
        }

        for (int i = 0; i < itemsToSpawnCount; i++)
        {
            float probTarget = Random.Range(0f, 1f) * totalProbabilies;
            float cumuledProb = 0;
            foreach (EnvironmentItem item in items)
            {
                cumuledProb += item.probability;
                if (cumuledProb > probTarget)
                {
                    SpawnItem(item);
                    break;
                }
            }
        }
    }

    void SpawnItem(EnvironmentItem item)
    {
        Vector3 localPosition = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * TileMap.TIlE_SIZE * 0.5f * spawnableDistFromCenter;
        Quaternion rotation;
        if (item.rotateZOnly)
            rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        else
            rotation = Random.rotation;
        Vector3 localScale = Vector3.one * Random.Range(item.minScale, item.maxScale);

        Transform newItem = Instantiate(item.prefab, transform);
        newItem.localPosition = localPosition;
        newItem.rotation = rotation;
        newItem.localScale = localScale;

        if(item.applyWind)
        {
            //Armature->Root->Wind
            Transform windBone = newItem.GetChild(0).GetChild(0).GetChild(0);
            Wind.Instance.RegisterBone(windBone);
        }    
    }
}
