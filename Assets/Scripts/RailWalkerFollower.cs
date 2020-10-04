using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailWalkerFollower : RailWalker
{
    [Header("Following")]
    [SerializeField] RailWalker followTarget = null;
    [SerializeField] float progressDelay = 1;

    // Update is called once per frame
    void Update()
    {
        entryDirection = followTarget.OutDirection;
        currentCoords = followTarget.currentCoords;
        FollowRails(progressDelay);
    }

    void FollowRails(float progressDelay)
    {
        if(followTarget.RealProgress>1)
        {
            //If the progress is above one, it means the target as yet to change tile but is effectyvely on the next tile, so go one time less far.
            progressDelay--;
        }

        float progressAmount = followTarget.RealProgress % 1 - progressDelay;

        //Finds the tile we are in
        while (progressAmount < 0 )
        {
            //Find the next tile
            currentCoords = TileMap.Instance.GetNextTileCoords(currentCoords, ref entryDirection);

            //Remove one from progress since we moved one tile
            progressAmount++;
        }

        //Place ourself into this tile
        TileRail tile = TileMap.Instance.GetTile(currentCoords);

        //We are going the opposite way that we are doing the search, so invert it
        goingReverse = !tile.IsGoingReverse(entryDirection);
        progress = progressAmount;

    }
}
