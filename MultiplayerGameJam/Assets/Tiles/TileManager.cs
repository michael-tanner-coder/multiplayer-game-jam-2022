using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<Tile> tileDataList;

    [SerializeField]
    private Tile defaultTile;

    private Dictionary<TileBase, Tile> dataFromTiles;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, Tile>();

        foreach(Tile tileData in tileDataList) 
        {
            foreach(TileBase tileBase in tileData.tiles)
            {
                dataFromTiles.Add(tileBase, tileData);
            }
        }
    }

    public Tile GetTileData(Vector3 worldPos) 
    {
        Vector3Int gridPosition = map.WorldToCell(worldPos);

        TileBase tile = map.GetTile(gridPosition);

        if (tile == null)
            return defaultTile;

        return dataFromTiles[tile];
    }
}
