using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] RailWalker headOfTrain = null;

    [Header("Bandits")]
    [SerializeField] GameObject banditPrefab = null;
    [SerializeField] float banditSpawnTime = 4;
    [SerializeField] float averageTimeBetweenBanditsEasy = 30;
    [SerializeField] float AverageTimeBetweenBanditsHard = 10;
    [SerializeField] int maxBanditTilesCountEasy = 4;
    [SerializeField] int maxBanditTilesCountHard = 8;
    List<SpawnedModel> banditTiles = new List<SpawnedModel>();
    bool isSpawningBandits;
    EvolvingStateToken hasBanditToken = new EvolvingStateToken(true);

    float AverageTimeBetweenBandits { get => Mathf.Lerp(averageTimeBetweenBanditsEasy, AverageTimeBetweenBanditsHard, GameManager.difficulty); }
    float BanditSpawnProbability { get => Time.deltaTime / AverageTimeBetweenBandits; }
    float MaxBanditTilesCount { get => Mathf.Lerp(maxBanditTilesCountEasy, maxBanditTilesCountHard, GameManager.difficulty); }

    // Start is called before the first frame update
    void Start()
    {
        headOfTrain.onNewCoords.AddListener(OnNewTrainPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we should spawn bandits
        if(!isSpawningBandits && Random.Range(0f,1f) <= BanditSpawnProbability)
        {
            //Find a cell
            Vector2Int coords = TileMap.Instance.GetRandomTileCoords();
            TileRail tile = TileMap.Instance.GetTile(coords);

            //Make sure the train isn't on it and no bandits yet
            int triesCount = 0;
            while(triesCount<100 && (tile.somethingOnRailState.IsOn || banditTiles.Exists(t=>t.coords.x== coords.x && t.coords.y== coords.y) || tile.hasHazardState.IsOn))
            {
                coords = TileMap.Instance.GetRandomTileCoords();
                tile = TileMap.Instance.GetTile(coords);
                triesCount++;
            }

            if(triesCount>=100)
            {
                Debug.LogError("Max tries reached when finding bandit tile", this);
                return;
            }

            if(banditTiles.Count>= MaxBanditTilesCount)
            {
                //Remove the oldest bandit tile
                Destroy(banditTiles[0].model);
                TileMap.Instance.GetTile(banditTiles[0].coords).hasHazardState.Remove(hasBanditToken);
                banditTiles.RemoveAt(0);
            }
            //Spawn the new tile
            StartCoroutine(SpawnBandits(tile, coords));
        }
    }

    void OnNewTrainPosition(Vector2Int coords)
    {
        //Watch GameOver
        if (banditTiles.Exists(t => t.coords.x == headOfTrain.currentCoords.x && t.coords.y == headOfTrain.currentCoords.y))
        {
            GameManager.Instance.GameOver("Bandit");
        }
    }

    IEnumerator SpawnBandits(TileRail tile, Vector2Int coords)
    {
        tile.hasHazardState.Add(hasBanditToken);
        isSpawningBandits = true;
        //Highlight the tile for a while
        tile.SendMessage("SetHighlightOn");

        yield return new WaitForSeconds(banditSpawnTime);

        tile.SendMessage("SetHighlightOff");

        //Spawn the bandits
        GameObject model = Instantiate(banditPrefab, tile.transform.position, banditPrefab.transform.rotation, transform);
        isSpawningBandits = false;
        banditTiles.Add(new SpawnedModel(coords, model));
    }
}
