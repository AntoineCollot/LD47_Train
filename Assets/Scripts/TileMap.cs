using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public enum Direction { None, Left, Top, Right, Bottom }

public class TileMap : MonoBehaviour
{
    [SerializeField] TileRail tilePrefab = null;
    [SerializeField] Vector2Int mapSize = new Vector2Int(10, 10);
    public const float TIlE_SIZE = 1;
    [SerializeField] TileRail[,] tiles;

    public static TileMap Instance;

    public int TileCount { get => mapSize.x * mapSize.y; }

    private void Awake()
    {
        Instance = this;
        LoadTileArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        //GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Generate Map")]
    void GenerateMap()
    {
        if (tilePrefab == null)
            return;

        tiles = new TileRail[mapSize.x, mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector3 pos = new Vector3(TIlE_SIZE * (x - mapSize.x * 0.5f + 0.5f), 0, TIlE_SIZE * (y - mapSize.y * 0.5f + 0.5f));
                TileRail newTile = Instantiate(tilePrefab, transform);
                newTile.name = $"TileRail_{x}_{y}";
                newTile.transform.localPosition = pos;
                tiles[x, y] = newTile;
            }
        }
    }

    void LoadTileArray()
    {
        tiles = new TileRail[mapSize.x, mapSize.y];
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                tiles[x, y] = transform.GetChild(mapSize.x * y + x).GetComponent<TileRail>();
            }
        }
    }

    void ClearMap()
    {
        if (tiles == null)
            return;
        foreach (TileRail tile in tiles)
        {
            if (tile != null)
                Destroy(tile.gameObject);
        }
        tiles = null;
    }

    public Vector2Int GetNextTileCoords(in Vector2Int coords, ref Direction entryDirection)
    {
        Direction outputDirection = tiles[coords.x, coords.y].GetOutputDirection(entryDirection);
        Vector2Int nextCoords = coords;

        //Move one tile according to the output direction
        //Also inverse the output direction, since when going out of a tile from right when enter the next one left
        switch (outputDirection)
        {
            case Direction.Left:
                nextCoords.x--;
                break;
            case Direction.Top:
                nextCoords.y++;
                break;
            case Direction.Right:
                nextCoords.x++;
                break;
            case Direction.Bottom:
                nextCoords.y--;
                break;
        }

        entryDirection = outputDirection.Inverse();
        return nextCoords;
    }

    public TileRail GetTile(Vector2Int coords)
    {
        return tiles[coords.x, coords.y];
    }
}
