using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int playerCoords;
    public float moveSpeed = 0.2f;

    private void Update()
    {
        Vector2Int input = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) input.y = 1;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) input.y = -1;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) input.x = -1;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) input.x = 1;

        if (input != Vector2Int.zero)
        {
            Vector2Int targetCoords = playerCoords + input;

            if (IsValidMove(targetCoords))
            {
                playerCoords = targetCoords;

                // Fixed the tile index calculation (swapped x and y)
                int index = targetCoords.x * gridManager.numRows + targetCoords.y;
                if (index < gridManager.transform.childCount)
                {
                    GridTile targetTile = gridManager.transform.GetChild(index).GetComponent<GridTile>();
                    targetTile.CollectTreasure(this);
                    targetTile.ActivateTrap(this);
                    gridManager.OnTileHoverEnter(targetTile);

                    // Adjusted target position calculation
                    Vector2 targetPos = new Vector2(targetCoords.x + (gridManager.padding * targetCoords.x), targetCoords.y + (gridManager.padding * targetCoords.y));
                    StartCoroutine(MoveToPosition(targetPos));
                }
            }
        }
    }

    private bool IsValidMove(Vector2Int targetCoords)
    {
        return targetCoords.x >= 0 && targetCoords.x < gridManager.numCols &&
               targetCoords.y >= 0 && targetCoords.y < gridManager.numRows;
    }

    IEnumerator MoveToPosition(Vector2 targetPos)
    {
        float elapsedTime = 0f;
        Vector2 startingPos = transform.localPosition;

        while (elapsedTime < moveSpeed)
        {
            transform.localPosition = Vector2.Lerp(startingPos, targetPos, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPos;
    }
}
