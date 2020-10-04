using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RailWalker : MonoBehaviour
{
    protected float progress;
    public Vector2Int currentCoords;
    public Direction entryDirection;
    public bool goingReverse { get; protected set; }

    public float EffectiveProgress
    {
        get
        {
            if (goingReverse)
                return 1 - progress;

            return progress;
        }
    }

    public float RealProgress
    {
        get
        {
            return progress;
        }
    }

    public Direction OutDirection
    {
        get
        {
            return TileMap.Instance.GetTile(currentCoords).GetOutputDirection(entryDirection);
        }
    }
}
