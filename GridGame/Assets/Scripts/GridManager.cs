using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GridTile tilePrefab;
    public int numCols = 5;
    public int numRows = 5;
    public float padding = 0.1f;

    public PlayerController player;

    public List<Vector2Int> treasurePositions = new List<Vector2Int>(); // Set treasure locations
    public List<Vector2Int> trapPositions = new List<Vector2Int>();     // Set trap locations

    private Dictionary<Vector2Int, GridTile> gridTiles = new Dictionary<Vector2Int, GridTile>();

    void Start()
    {
        GenerateGrid();
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < numCols; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                GridTile tile = Instantiate(tilePrefab, transform);
                Vector2 tilePos = new Vector2(i + (padding * i), j + (padding * j));
                tile.transform.localPosition = tilePos;
                tile.name = $"Tile_{i}_{j}";
                tile.gridManager = this;
                tile.gridCoords = new Vector2Int(i, j);
                gridTiles[tile.gridCoords] = tile;

                // Assign special tile types
                if (treasurePositions.Contains(tile.gridCoords))
                {
                    tile.SetTileType(GridTile.TileType.Treasure);
                }
                else if (trapPositions.Contains(tile.gridCoords))
                {
                    tile.SetTileType(GridTile.TileType.Trap);
                }
                else
                {
                    tile.SetTileType(GridTile.TileType.Normal);
                }
            }
        }
    }
}
