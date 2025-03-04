using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int gridCoords;
    public bool isTrap = false;
    public bool isTreasure = false;


    public void CollectTreasure(PlayerController player)
    {
        if (isTreasure)
        {
            isTreasure = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white; // Clear visual
            Debug.Log($"Treasure Collected at {gridCoords}!");
        }
    }
    private void OnMouseOver()
    {
        gridManager.OnTileHoverEnter(this);
    }
    private void OnMouseExit()
    {
        gridManager.OnTileHoverExit(this);
    }

    public void ActivateTrap(PlayerController player)
    {
        if (isTrap)
        {
            Debug.Log($"Trap Activated at {gridCoords}!");
            foreach (GuardController guard in gridManager.GetComponentsInChildren<GuardController>())
            {
                guard.AlertGuard(gridCoords);
            }
        }
    }

}
