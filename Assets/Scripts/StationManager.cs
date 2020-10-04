using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    [Header("Station")]
    [SerializeField] GameObject stationPrefab = null;
    [SerializeField] float stationSpawnTime = 4;
    [SerializeField] float minTimeBetweenStations = 35;
    float lastStationSpawnTime;
    [SerializeField] float averageTimeBetweenStationsEasy = 30;
    [SerializeField] float AverageTimeBetweenStationsHard = 10;
    List<SpawnedModel> stationTiles = new List<SpawnedModel>();
    bool isSpawningStation;
    EvolvingStateToken hasStationToken = new EvolvingStateToken(true);

    float AverageTimeBetweenStations { get => Mathf.Lerp(averageTimeBetweenStationsEasy, AverageTimeBetweenStationsHard, GameManager.difficulty); }
    float StationSpawnProb { get => Time.deltaTime / AverageTimeBetweenStations; }
    float MaxStationCount { get => 1; }

    private void Start()
    {
        SpawnStation();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we should spawn station
        if (!isSpawningStation && lastStationSpawnTime + minTimeBetweenStations< Time.time && Random.Range(0f, 1f) <= StationSpawnProb)
        {
            SpawnStation();
        }
    }

    void SpawnStation()
    {
        //Find a cell
        Vector2Int coords = TileMap.Instance.GetRandomTileCoordsNotEdge();
        TileRail tile = TileMap.Instance.GetTile(coords);

        //Make sure no rail on it
        int triesCount = 0;
        while (triesCount < 100 && (tile.type != RailType.None || tile.hasHazardState.IsOn))
        {
            coords = TileMap.Instance.GetRandomTileCoordsNotEdge();
            tile = TileMap.Instance.GetTile(coords);
            triesCount++;
        }

        if (triesCount >= 100)
        {
            Debug.LogError("Max tries reached when finding station tile", this);
            return;
        }

        if (stationTiles.Count >= MaxStationCount)
        {
            //Remove the oldest station tile
            Destroy(stationTiles[0].model);
            TileRail tileToClear = TileMap.Instance.GetTile(stationTiles[0].coords);
            tileToClear.hasHazardState.Remove(hasStationToken);
            tileToClear.lockChangeRailTypeState.Add(hasStationToken);

            stationTiles.RemoveAt(0);
        }
        //Spawn the new tile
        StartCoroutine(SpawnStation(tile, coords));

        lastStationSpawnTime = Time.time;
    }

    IEnumerator SpawnStation(TileRail tile, Vector2Int coords)
    {
        tile.hasHazardState.Add(hasStationToken);
        isSpawningStation = true;

        //Highlight the tile for a while
        tile.SendMessage("SetHighlightOn");

        yield return new WaitForSeconds(stationSpawnTime);

        tile.SendMessage("SetHighlightOff");

        //Spawn the station
        GameObject model = Instantiate(stationPrefab, tile.transform.position, Quaternion.identity, transform);
        tile.lockChangeRailTypeState.Add(hasStationToken);
        //Set up the tile
        //Horizontal
        if (Random.Range(0f, 1f) > 0.5f)
        {
            tile.SetType(RailType.Horizontal);
            if (Random.Range(0f, 1f) > 0.5f)
                 model.transform.localEulerAngles = Vector3.up * 90;
            else
                 model.transform.localEulerAngles = Vector3.up * -90;
        }
        //Vertical
        else
        {
            tile.SetType(RailType.Vertical);
            if (Random.Range(0f, 1f) > 0.5f)
                model.transform.localEulerAngles = Vector3.up * 180;
        }

        isSpawningStation = false;
        stationTiles.Add(new SpawnedModel(coords, model));
    }
}
