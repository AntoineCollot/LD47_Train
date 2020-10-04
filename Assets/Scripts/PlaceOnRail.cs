using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnRail : MonoBehaviour
{
    RailWalker railWalker;

    // Start is called before the first frame update
    void Start()
    {
        railWalker = GetComponent<RailWalker>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        TileRail tile = TileMap.Instance.GetTile(railWalker.currentCoords);
        Vector3 targetPosition = tile.GetPoint(railWalker.EffectiveProgress);
        targetPosition.y = transform.position.y;
        transform.position = targetPosition;
        Vector3 lookAtPosition;
        if (railWalker.goingReverse)
            lookAtPosition = targetPosition - tile.GetDirection(railWalker.EffectiveProgress);
        else
            lookAtPosition = targetPosition + tile.GetDirection(railWalker.EffectiveProgress);
        lookAtPosition.y = transform.position.y; ;
        transform.LookAt(lookAtPosition);
    }
}
