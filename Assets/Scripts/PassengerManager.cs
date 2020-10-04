using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] RailWalker headOfTrain = null;
    [SerializeField] float scoreMultiplier = 0.25f;

    [Header("Passengers")]
    [SerializeField] GameObject passengerPrefab = null;
    [SerializeField] float passengerSpawnTime = 4;
    [SerializeField] float averageTimeBetweenPassengersEasy = 30;
    [SerializeField] float AverageTimeBetweenPassengersHard = 10;
    [SerializeField] int maxPassengerTilesCountEasy = 4;
    [SerializeField] int maxPassengerTilesCountHard = 8;
    List<SpawnedModel> passengerTiles = new List<SpawnedModel>();
    bool isSpawningPassengers;
    EvolvingStateToken hasPassengerToken = new EvolvingStateToken(true);
    int passengerSaved;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI passengerSavedCountText = null;

    public static PassengerManager Instance;

    float AverageTimeBetweenPassengers { get => Mathf.Lerp(averageTimeBetweenPassengersEasy, AverageTimeBetweenPassengersHard, GameManager.difficulty); }
    float PassengerSpawnProbability { get => Time.deltaTime / AverageTimeBetweenPassengers; }
    float MaxPassengerTilesCount { get => Mathf.Lerp(maxPassengerTilesCountEasy, maxPassengerTilesCountHard, GameManager.difficulty); }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we should spawn bandits
        if(!isSpawningPassengers && Random.Range(0f,1f) <= PassengerSpawnProbability)
        {
            //Find a cell
            Vector2Int coords = TileMap.Instance.GetRandomTileCoords();
            TileRail tile = TileMap.Instance.GetTile(coords);

            //Make sure the train isn't on it and no bandits yet
            int triesCount = 0;
            while(triesCount<100 && (tile.somethingOnRailState.IsOn || tile.hasHazardState.IsOn))
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

            if(passengerTiles.Count>= MaxPassengerTilesCount)
            {
                //Remove the oldest bandit tile
                if(passengerTiles[0].model!=null)
                    Destroy(passengerTiles[0].model);
                TileMap.Instance.GetTile(passengerTiles[0].coords).hasHazardState.Remove(hasPassengerToken);
                passengerTiles.RemoveAt(0);
            }
            //Spawn the new tile
            StartCoroutine(SpawnPassenger(tile, coords));
        }
    }

    IEnumerator SpawnPassenger(TileRail tile, Vector2Int coords)
    {
        tile.hasHazardState.Add(hasPassengerToken);
        isSpawningPassengers = true;
        //Highlight the tile for a while
        tile.SendMessage("SetHighlightOn");

        yield return new WaitForSeconds(passengerSpawnTime);

        tile.SendMessage("SetHighlightOff");

        //Spawn the bandits
        GameObject model = Instantiate(passengerPrefab, tile.transform.position, passengerPrefab.transform.rotation, transform);
        model.GetComponent<Passenger>().Init(headOfTrain,coords);
        isSpawningPassengers = false;
        passengerTiles.Add(new SpawnedModel(coords, model));

        SoundManager.Instance.Play(3);
    }

    public void PassengerSaved(Vector2Int coords)
    {
        passengerSaved++;
        GameManager.Instance.AddScoreMultiplier(scoreMultiplier);
        passengerSavedCountText.text = $"(x{GameManager.scoreMultiplier.ToString("N2")} |{passengerSaved.ToString()}";

        SoundManager.Instance.Play(4);
    }
}
