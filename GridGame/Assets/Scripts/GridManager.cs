using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
    public int numRows = 5;
    public int numCols = 6;
    public float padding = 0.1f;

    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        InitGrid();
    }

    public void InitGrid()
    {
        for (int i = 0; i < numCols; i++)
        {
            for(int j = 0; j < numRows; j++)
            {
                GridTile tile = Instantiate(tilePrefab, transform);
                Vector2 tilePos = new Vector2(i + (padding*i), j + (padding *j));
                tile.transform.localPosition = tilePos;
                tile.name = $"Tile_{i}_{j}";
                tile.gridManager = this;
                tile.gridCoords = new Vector2Int(i, j);
            }
        }
    }
    public void OnTileHoverEnter(GridTile gridTile)
    {
        text.text = gridTile.gridCoords.ToString();
    }

    public void OnTileHoverExit(GridTile gridTile)
    {
        text.text = "---";
    }
}
