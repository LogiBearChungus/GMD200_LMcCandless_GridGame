using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
    public int numRows = 5;
    public int numCols = 6;
    public float padding = 0.1f;
    public int numTreasures = 3;
    public int numTraps = 2;

    [SerializeField] private GridTile tilePrefab;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private Vector2Int playerStartCoords;
    [HideInInspector] public PlayerController player;


    [SerializeField] private GuardController guardPrefab;
    [SerializeField] private List<Vector2Int[]> guardPatrolPaths;

    private void Start()
    {
        SpawnPlayer();

    }

    private void Awake()
    {
        InitGrid();
    }

    public void InitGrid()
    {
        List<Vector2Int> availableTiles = new List<Vector2Int>();

        // Create grid and add coordinates to available tiles
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
                availableTiles.Add(tile.gridCoords);
            }
        }

        // Place Treasures
        for (int i = 0; i < numTreasures; i++)
        {
            Vector2Int randCoord = availableTiles[Random.Range(0, availableTiles.Count)];
            availableTiles.Remove(randCoord);
            transform.GetChild(randCoord.y * numCols + randCoord.x).GetComponent<GridTile>().isTreasure = true;
            transform.GetChild(randCoord.y * numCols + randCoord.x).GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        // Place Traps
        for (int i = 0; i < numTraps; i++)
        {
            Vector2Int randCoord = availableTiles[Random.Range(0, availableTiles.Count)];
            availableTiles.Remove(randCoord);
            transform.GetChild(randCoord.y * numCols + randCoord.x).GetComponent<SpriteRenderer>().color = Color.red;
            transform.GetChild(randCoord.y * numCols + randCoord.x).GetComponent<GridTile>().isTrap = true;
        }

        // Generate patrol paths for guards
        GenerateGuardPatrolPaths();
        SpawnGuards();
    }

    // Function to generate patrol paths
    




    public void OnTileHoverEnter(GridTile gridTile)
    {
        text.text = gridTile.gridCoords.ToString();
    }

    public void OnTileHoverExit(GridTile gridTile)
    {
        text.text = "---";
    }


    private void SpawnPlayer()
    {
        PlayerController player = Instantiate(playerPrefab, transform);
        player.gridManager = this;
        player.playerCoords = playerStartCoords;
        player.transform.localPosition = new Vector2(playerStartCoords.x + (padding * playerStartCoords.x), playerStartCoords.y + (padding * playerStartCoords.y));
        this.player = player;
    }
    void SpawnGuards()
    {
        foreach (var path in guardPatrolPaths)
        {
            GuardController guard = Instantiate(guardPrefab, transform);
            guard.gridManager = this;
            guard.patrolPath = path;
            guard.transform.localPosition = new Vector2(path[0].x + (padding * path[0].x), path[0].y + (padding * path[0].y));
        }
    }
    private void GenerateGuardPatrolPaths()
    {
        guardPatrolPaths = new List<Vector2Int[]>();

        int numGuards = 3; // For example, spawning 3 guards

        for (int i = 0; i < numGuards; i++)
        {
            List<Vector2Int> guardPath = new List<Vector2Int>();

            // Choose a random starting point
            Vector2Int currentPos = new Vector2Int(Random.Range(0, numCols), Random.Range(0, numRows));
            guardPath.Add(currentPos);

            // Generate adjacent waypoints (one square at a time)
            int pathLength = Random.Range(2, 5); // Patrol path length between 2 and 5 waypoints
            for (int j = 1; j < pathLength; j++)
            {
                // Get all adjacent positions (up, down, left, right)
                List<Vector2Int> adjacentPositions = new List<Vector2Int>
            {
                new Vector2Int(currentPos.x - 1, currentPos.y), // left
                new Vector2Int(currentPos.x + 1, currentPos.y), // right
                new Vector2Int(currentPos.x, currentPos.y - 1), // down
                new Vector2Int(currentPos.x, currentPos.y + 1), // up
            };

                // Filter out invalid positions (e.g., out of bounds)
                adjacentPositions.RemoveAll(p => p.x < 0 || p.x >= numCols || p.y < 0 || p.y >= numRows);

                // Pick a random valid adjacent position
                Vector2Int nextPos = adjacentPositions[Random.Range(0, adjacentPositions.Count)];
                guardPath.Add(nextPos);

                // Update current position for next iteration
                currentPos = nextPos;
            }

            // Add the path to the list of patrol paths
            guardPatrolPaths.Add(guardPath.ToArray());
        }
    }

}
