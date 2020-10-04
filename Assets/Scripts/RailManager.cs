using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    [SerializeField] int maxRailCount = 20;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI railCountText = null;

    [Header("Changing rails")]
    bool isChangingRail;
    public bool allowChagingRails = true;
    [SerializeField] float timeChangingRailEasy = 20;
    [SerializeField] float timeChangingRailHard = 5;

    float ChangingRailTime { get => Mathf.Lerp(timeChangingRailEasy, timeChangingRailHard, GameManager.difficulty); }

    List<TileRail> rails = new List<TileRail>();
    public static RailManager Instance;

    public bool MaxRailCountReached { get => rails.Count >= maxRailCount; }
    public int RemainingRailsCount { get => maxRailCount - rails.Count; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.gameIsOver)
            return;

        if (allowChagingRails && !isChangingRail && Random.Range(0f, 1f) < ChangingRailTime * Time.deltaTime)
            StartCoroutine(SpawnChangingRail());
    }

    IEnumerator SpawnChangingRail()
    {
        isChangingRail = true;
         Vector2Int coords = TileMap.Instance.GetRandomTileCoords();
        TileRail tile = TileMap.Instance.GetTile(coords);

        //Select a cell with rail
        int triesCount = 0;
        while (triesCount < 100 && (tile.somethingOnRailState.IsOn || tile.hasHazardState.IsOn || tile.type==RailType.None))
        {
            coords = TileMap.Instance.GetRandomTileCoords();
            tile = TileMap.Instance.GetTile(coords);
            triesCount++;
        }

        if (triesCount >= 100)
            yield break;

        //Highlight rail
        RailHighlight railhighlight = tile.GetComponentInChildren<RailHighlight>();
        railhighlight.SetHighlightOn();

        //Lock rail
        EvolvingStateToken changingRailToken = new EvolvingStateToken(true);
        tile.lockChangeRailTypeState.Add(changingRailToken);

        //Wait for train - as long as nothing on it wait
        while(!tile.somethingOnRailState.IsOn)
        {
            yield return null;
        }

        //now wait for it to be empty again
        while(tile.somethingOnRailState.IsOn)
        {
            yield return null;
        }

        //Change the rail
        tile.SetRandomType();
        yield return null;
        railhighlight = tile.GetComponentInChildren<RailHighlight>();
        //FX
        railhighlight.SetLockedMaterial();

        yield return new WaitForSeconds(20);

        //unlock
        railhighlight.SetHighlightOff();
        tile.lockChangeRailTypeState.Remove(changingRailToken);

        isChangingRail = false;
    }

    public void RegisterNewRail(TileRail tile)
    {
        if (!rails.Contains(tile))
        {
            rails.Add(tile);
            railCountText.text = RemainingRailsCount.ToString();
        }
    }

    public void RemoveRail(TileRail tile)
    {
        rails.Remove(tile);
        railCountText.text = RemainingRailsCount.ToString();
    }
}
