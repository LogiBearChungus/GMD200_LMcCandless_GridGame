using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int gridCoords;

    private void OnMouseOver()
    {
        gridManager.OnTileHoverEnter(this);
    }
    private void OnMouseExit()
    {
        gridManager.OnTileHoverExit(this);
    }
}
