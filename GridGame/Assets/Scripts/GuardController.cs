using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int[] patrolPath; // List of grid positions the guard will follow
    public float moveSpeed = 1f; // Adjusted speed for movement (higher value = faster movement)
    private int patrolIndex = 0;

    private void Start()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Get the target position for the guard (the next patrol point)
            Vector2 targetPos = new Vector2(patrolPath[patrolIndex].x + (gridManager.padding * patrolPath[patrolIndex].x),
                                            patrolPath[patrolIndex].y + (gridManager.padding * patrolPath[patrolIndex].y));

            // Move to the target position
            yield return StartCoroutine(MoveToPosition(targetPos));

            // Move to the next patrol point
            patrolIndex = (patrolIndex + 1) % patrolPath.Length; // Loop back to start if at the end
            yield return new WaitForSeconds(0.5f); // Small pause before next move (this can be adjusted)
        }
    }

    IEnumerator MoveToPosition(Vector2 targetPos)
    {
        float elapsedTime = 0f;
        Vector2 startingPos = transform.localPosition;

        // Move the guard to the target position in a smooth manner, but only move one square
        float moveDuration = 0.3f; // Duration to move one square

        while (elapsedTime < moveDuration)
        {
            transform.localPosition = Vector2.Lerp(startingPos, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the guard reaches the target position exactly
        transform.localPosition = targetPos;

        // Check if the player is caught
        CheckForPlayer();
    }


    private void CheckForPlayer()
    {
        Vector2Int guardCoords = patrolPath[patrolIndex];
        if (gridManager.player.playerCoords == guardCoords)
        {
            Debug.Log("You got caught!");
            // Trigger Game Over Logic Here
        }
    }

    public void AlertGuard(Vector2Int trapCoords)
    {
        if (System.Array.Exists(patrolPath, p => p == trapCoords))
        {
            patrolIndex = System.Array.IndexOf(patrolPath, trapCoords); // Skip to the trap tile
        }
    }
}
