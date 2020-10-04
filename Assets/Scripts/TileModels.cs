using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModels : MonoBehaviour
{
    [Header("Rails")]
    [SerializeField] Transform railStraightPrefab = null;
    [SerializeField] Transform railTurnPrefab = null;
    Transform instancedRail;
    RailType currentType;

    // Start is called before the first frame update
    void Start()
    {
        TileRail rail = GetComponent<TileRail>();
        rail.onTypeChanged.AddListener(OnTileTypeChanged);
        InstantiateModel(rail.type);
        RotateRailModel(rail.type);
    }

    void OnTileTypeChanged(RailType newType)
    {
        //Instantiation
        //If the new type is None, simply destroy our existing model if any
        if (newType == RailType.None)
        {
            if (instancedRail != null)
                Destroy(instancedRail.gameObject);
            return;
        }

        if(currentType.IsStraight() && instancedRail != null)
        {
            if (newType.IsTurn())
                InstantiateModel(newType);
        }
        else if(currentType.IsTurn() && instancedRail != null)
        {
            if (newType.IsStraight())
                InstantiateModel(newType);
        }
        //No existing model
        else
        {
            InstantiateModel(newType);
        }

        RotateRailModel(newType);
    }

    void InstantiateModel(RailType railType)
    {
        //Destroy the existing rail
        if (instancedRail != null)
            Destroy(instancedRail.gameObject);

        switch (railType)
        {
            case RailType.Horizontal:
            case RailType.Vertical:
                instancedRail = Instantiate(railStraightPrefab, transform);
                break;
            case RailType.TopRight:
            case RailType.RightBottom:
            case RailType.BottomLeft:
            case RailType.LeftTop:
                instancedRail = Instantiate(railTurnPrefab, transform);
                break;
        }
    }

    void RotateRailModel(RailType railType)
    {
        //Rotation
        switch (railType)
        {
            case RailType.Horizontal:
            case RailType.RightBottom:
                instancedRail.localEulerAngles = Vector3.up * 90;
                break;
            case RailType.Vertical:
            case RailType.TopRight:
                instancedRail.localEulerAngles = Vector3.zero;
                break;
            case RailType.BottomLeft:
                instancedRail.localEulerAngles = Vector3.up * 180;
                break;
            case RailType.LeftTop:
                instancedRail.localEulerAngles = Vector3.up * 270;
                break;
        }
    }
}
