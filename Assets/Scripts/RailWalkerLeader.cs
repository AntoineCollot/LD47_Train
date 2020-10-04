using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailWalkerLeader : RailWalker
{
    [Header("Leading")]
    [SerializeField] protected float timePerTileEasy = 2;
    [SerializeField] protected float timePerTileHard = 0.75f;

    float TimePerTime { get => Mathf.Lerp(timePerTileEasy, timePerTileHard, GameManager.difficulty); }

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
        while (!GameManager.gameIsOver)
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
        while (progress < 1 && !GameManager.gameIsOver)
        {
            progress += Time.deltaTime / TimePerTime;

            yield return null;
        }
        tile.somethingOnRailState.Remove(isOnRailToken);
    }
}
