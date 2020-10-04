using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailWalkerLeader : RailWalker
{
    [Header("Leading")]
    [SerializeField] protected float timePerTile = 3;

    private void Start()
    {
        WalkRails();
    }

    [ContextMenu("WalkRails")]
    public void WalkRails()
    {
        StartCoroutine(WalkRailsC());
    }

    IEnumerator WalkRailsC()
    {
        while (true)
        {
            //Walk the current rail
            yield return StartCoroutine(WalkRail());

            //Find the next tile
            currentCoords = TileMap.Instance.GetNextTileCoords(currentCoords, ref entryDirection);
            progress %= 1;
        }
    }

    IEnumerator WalkRail()
    {
        TileRail tile = TileMap.Instance.GetTile(currentCoords);
        goingReverse = tile.IsGoingReverse(entryDirection);
        while (progress < 1)
        {
            progress += Time.deltaTime / timePerTile;

            yield return null;
        }
    }
}
