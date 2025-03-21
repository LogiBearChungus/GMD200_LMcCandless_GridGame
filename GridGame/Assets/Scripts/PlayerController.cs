using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;
    public Vector2Int playerCoords;
    public float moveSpeed = 0.2f;

    public int score = 0;
    public int moveCount = 0;
    public int treasureCount = 0;
    public int trapsTriggered = 0;

    public TMP_Text scoreText;
    public TMP_Text moveText;

    public GameObject gameOverScreen;
    public GameObject winScreen;
    public GameObject pointCanvas;

    public TMP_Text winScoreText;
    public TMP_Text winMoveText;
    public TMP_Text winTreasureText;
    public TMP_Text winTrapText;

    private void Start()
    {
        UpdateUI();
        winScreen.SetActive(false);
    }

    private void Update()
    {
        if (gameOverScreen.activeSelf) return;
        if (winScreen.activeSelf) return;

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
                moveCount++;
                score -= 10;

                int index = targetCoords.x * gridManager.numRows + targetCoords.y;
                if (index < gridManager.transform.childCount)
                {
                    GridTile targetTile = gridManager.transform.GetChild(index).GetComponent<GridTile>();

                    if (targetTile.tileType == GridTile.TileType.Treasure)
                    {
                        score += 100;
                        treasureCount++;
                        targetTile.CollectTreasure(this);
                    }
                    else if (targetTile.tileType == GridTile.TileType.Trap)
                    {
                        score -= 50;
                        trapsTriggered++;
                        targetTile.ActivateTrap(this);
                    }
                    else if (targetTile.tileType == GridTile.TileType.Exit)
                    {
                        WinGame();
                    }
                }

                Vector2 targetPos = new Vector2(
                    targetCoords.x + (gridManager.padding * targetCoords.x),
                    targetCoords.y + (gridManager.padding * targetCoords.y)
                );

                StartCoroutine(MoveToPosition(targetPos));

                UpdateUI();
            }
        }
    }

    private bool IsValidMove(Vector2Int coords)
    {
        return coords.x >= 0 && coords.x < gridManager.numCols &&
               coords.y >= 0 && coords.y < gridManager.numRows;
    }

    private IEnumerator MoveToPosition(Vector2 targetPos)
    {
        Vector2 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (moveText != null) moveText.text = "Moves: " + moveCount;
    }

    public void WinGame()
    {
        pointCanvas.SetActive(false);
        winScreen.SetActive(true);

        if (winScoreText != null) winScoreText.text = "Final Score: " + score;
        if (winMoveText != null) winMoveText.text = "Total Moves: " + moveCount;
        if (winTreasureText != null) winTreasureText.text = "Treasures Collected: " + treasureCount;
        if (winTrapText != null) winTrapText.text = "Traps Triggered: " + trapsTriggered;
    }
}
