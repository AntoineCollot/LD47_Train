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

            //Make sure it's a valid path
            if(TileMap.Instance.GetTile(currentCoords).GetOutputDirection(entryDirection)==Direction.None)
            {
                GameManager.Instance.GameOver("Rail path broken");
            }

            progress %= 1;
        }
    }

    IEnumerator WalkRail()
    {
        onNewCoords.Invoke(currentCoords);

        TileRail tile = TileMap.Instance.GetTile(currentCoords);
        tile.somethingOnRailState.Add(isOnRailToken);
        goingReverse = tile.IsGoingReverse(entryDirection);
        while (progress < 1)
        {
            progress += Time.deltaTime / timePerTile;

            yield return null;
        }
        tile.somethingOnRailState.Remove(isOnRailToken);
    }
}
