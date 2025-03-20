using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public GridManager gridManager;
    public PlayerController player; // Reference to player
    public Vector2Int patrolPointA; // First patrol point
    public Vector2Int patrolPointB; // Second patrol point
    public float moveSpeed = 1f; // Speed of movement

    private Vector2Int currentTarget;
    private Vector2Int currentPos;
    private bool movingToA = true; // Determines patrol direction

    public GameObject gameOverScreen;
    public GameObject pointCanvas;

    private void Start()
    {
        currentPos = patrolPointA; // Start at point A
        currentTarget = patrolPointB; // Move towards point B first
        transform.localPosition = GetTileWorldPosition(currentPos);
        StartCoroutine(PatrolRoutine());
        gameOverScreen.SetActive(false);
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Move one step toward the target
            currentPos = MoveOneStep(currentPos, currentTarget);
            transform.localPosition = GetTileWorldPosition(currentPos);

            // Check if the guard reached the patrol point
            if (currentPos == currentTarget)
            {
                movingToA = !movingToA;
                currentTarget = movingToA ? patrolPointA : patrolPointB;
            }

            // Check if the guard caught the player
            CheckForPlayer();

            yield return new WaitForSeconds(0.5f); // Pause before next step
        }
    }

    private Vector2Int MoveOneStep(Vector2Int start, Vector2Int end)
    {
        // Move only one tile in a straight line
        if (start.x < end.x) return new Vector2Int(start.x + 1, start.y);
        if (start.x > end.x) return new Vector2Int(start.x - 1, start.y);
        if (start.y < end.y) return new Vector2Int(start.x, start.y + 1);
        if (start.y > end.y) return new Vector2Int(start.x, start.y - 1);

        return start; // If already at the target, stay put
    }

    private Vector2 GetTileWorldPosition(Vector2Int gridCoords)
    {
        return new Vector2(gridCoords.x + (gridManager.padding * gridCoords.x),
                           gridCoords.y + (gridManager.padding * gridCoords.y));
    }

    private void CheckForPlayer()
    {
        if (player != null && player.playerCoords == currentPos)
        {
            Debug.Log("Game Over! You were caught!");
            gameOverScreen.SetActive(true);
            pointCanvas.SetActive(false);
        }
    }

    public void AlertGuard(Vector2Int trapCoords)
    {
        // If the trap is on a patrol tile, move directly to it
        if (trapCoords == patrolPointA || trapCoords == patrolPointB)
        {
            currentTarget = trapCoords;
            movingToA = (trapCoords == patrolPointA); // Adjust patrol direction
        }
    }
}
