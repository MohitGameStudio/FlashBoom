using UnityEngine;

public class BombermanPlayAreaGenerator : MonoBehaviour
{
    [Header("Play Area Settings")]
    [SerializeField] private GameObject blockPrefab; // Prefab to instantiate
    [SerializeField] private Vector2Int gridSize = new Vector2Int(11, 11); // Grid dimensions (X, Z)
    [SerializeField] private float cellSize = 1f; // Size of each grid cell

    [Header("Randomization Settings")]
    [SerializeField] private float blockSpawnProbability = 0.7f; // Probability of spawning a block
    [SerializeField] private bool ensurePattern = true; // Leave pathways for the player

    [Header("Dependencies")]
    [SerializeField] private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    [SerializeField] private Transform groundTransform; // Reference to the ground

    private void Start()
    {
        if (playerMovement == null || groundTransform == null || blockPrefab == null)
        {
            Debug.LogError("Please assign all references (PlayerMovement, Ground, BlockPrefab).");
            return;
        }

        GeneratePlayAreaUsingGrid();
    }

    private void GeneratePlayAreaUsingGrid()
    {
        // Use the grid size and cell size from the PlayerMovement script for consistency
        Vector2Int playerGridSize = new Vector2Int(
            Mathf.CeilToInt(gridSize.x / cellSize),
            Mathf.CeilToInt(gridSize.y / cellSize)
        );

        // Calculate grid bounds based on the ground position and size
        Vector3 groundPos = groundTransform.position;

        float startX = groundPos.x - (playerGridSize.x / 2) * cellSize;
        float startZ = groundPos.z - (playerGridSize.y / 2) * cellSize;

        // Loop through the grid to place blocks
        for (int x = 0; x < playerGridSize.x; x++)
        {
            for (int z = 0; z < playerGridSize.y; z++)
            {
                // Skip cells for pathways
                if (ensurePattern && (x % 2 == 0 && z % 2 == 0))
                {
                    continue;
                }

                // Decide whether to spawn a block in this cell
                if (Random.value < blockSpawnProbability)
                {
                    Vector3 spawnPosition = new Vector3(
                        startX + x * cellSize,
                        groundPos.y + 10f, // Slightly above the ground
                        startZ + z * cellSize
                    );

                    Instantiate(blockPrefab, spawnPosition, Quaternion.identity, transform);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (groundTransform == null) return;

        // Draw the grid for visualization
        Gizmos.color = Color.green;
        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int z = 0; z <= gridSize.y; z++)
            {
                Vector3 gridPoint = new Vector3(
                    groundTransform.position.x - (gridSize.x / 2 * cellSize) + x * cellSize,
                    groundTransform.position.y+10f,
                    groundTransform.position.z - (gridSize.y / 2 * cellSize) + z * cellSize
                );

                Gizmos.DrawWireCube(gridPoint, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}
