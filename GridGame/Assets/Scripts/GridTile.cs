using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public enum TileType { Normal, Treasure, Trap }
    public TileType tileType = TileType.Normal;

    public GridManager gridManager;
    public Vector2Int gridCoords;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateTileVisual();
    }

    public void SetTileType(TileType type)
    {
        tileType = type;
        UpdateTileVisual();
    }

    void UpdateTileVisual()
    {
        if (spriteRenderer == null) return;

        switch (tileType)
        {
            case TileType.Treasure:
                spriteRenderer.color = Color.yellow; // Treasure tile color
                break;
            case TileType.Trap:
                spriteRenderer.color = Color.red; // Trap tile color
                break;
            default:
                spriteRenderer.color = Color.white; // Normal tile color
                break;
        }
    }

    public void CollectTreasure(PlayerController player)
    {
        if (tileType == TileType.Treasure)
        {
            tileType = TileType.Normal;
            UpdateTileVisual();
            Debug.Log($"Treasure Collected at {gridCoords}!");
        }
    }

    public void ActivateTrap(PlayerController player)
    {
        if (tileType == TileType.Trap)
        {
            Debug.Log($"Trap Activated at {gridCoords}!");

            GuardController[] guards = gridManager.GetComponentsInChildren<GuardController>();
            foreach (GuardController guard in guards)
            {
                guard.AlertGuard(gridCoords);
            }
        }
    }
}
