using UnityEngine;

public class CellGridDebug : MonoBehaviour
{
    [Header("Visualization Settings")]
    public bool showGrid = true;
    public bool showIndices = false;
    public bool showCellCenters = false;
    public Color gridColor = Color.white;
    public Color centerColor = Color.yellow;

    [Header("Rectangle Selection Debug")]
    public bool showRectSelection = false;
    public int testX = 5;
    public int testZ = 5;
    public int rectRadius = 2;
    public Color rectCenterColor = Color.red;
    public Color rectNeighborColor = Color.green;

    void OnDrawGizmos()
    {
        CellGridController controller = GetComponent<CellGridController>();
        if (controller?.cellGrid == null) return;

        CellGrid grid = controller.cellGrid;

        if (showGrid)
        {
            DrawGrid(grid);
        }

        if (showRectSelection)
        {
            DrawRectSelection(grid);
        }
    }

    void DrawGrid(CellGrid grid)
    {
        Gizmos.color = gridColor;

        // Draw lines parallel to X axis (running east-west)
        for (int z = 0; z <= grid.gridSize; z++)
        {
            Vector3 start = new Vector3(
                grid.gridOrigin.x,
                grid.gridOrigin.y,
                grid.gridOrigin.z + (z * grid.cellSize)
            );
            Vector3 end = new Vector3(
                grid.gridOrigin.x + (grid.gridSize * grid.cellSize),
                grid.gridOrigin.y,
                grid.gridOrigin.z + (z * grid.cellSize)
            );
            Gizmos.DrawLine(start, end);
        }

        // Draw lines parallel to Z axis (running north-south)
        for (int x = 0; x <= grid.gridSize; x++)
        {
            Vector3 start = new Vector3(
                grid.gridOrigin.x + (x * grid.cellSize),
                grid.gridOrigin.y,
                grid.gridOrigin.z
            );
            Vector3 end = new Vector3(
                grid.gridOrigin.x + (x * grid.cellSize),
                grid.gridOrigin.y,
                grid.gridOrigin.z + (grid.gridSize * grid.cellSize)
            );
            Gizmos.DrawLine(start, end);
        }

        // Draw cell centers
        if (showCellCenters)
        {
            Gizmos.color = centerColor;
            for (int z = 0; z < grid.gridSize; z++)
            {
                for (int x = 0; x < grid.gridSize; x++)
                {
                    Cell? cell = grid.GetCell(x, z);
                    if (cell.HasValue)
                        Gizmos.DrawSphere(cell.Value.cellPosition, 0.1f);
                }
            }
        }

        // Draw indices - this kills performance pretty quick if renders so many UI elements
#if UNITY_EDITOR
        if (showIndices)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 10;
            style.alignment = TextAnchor.MiddleCenter;

            for (int z = 0; z < grid.gridSize; z++)
            {
                for (int x = 0; x < grid.gridSize; x++)
                {
                    Cell? cell = grid.GetCell(x, z);
                    if (cell.HasValue)
                        UnityEditor.Handles.Label(cell.Value.cellPosition, cell.Value.cellIndex.ToString(), style);
                }
            }
        }
#endif
    }

    void DrawRectSelection(CellGrid grid)
    {
        // Bounds check
        if (testX < 0 || testX >= grid.gridSize || testZ < 0 || testZ >= grid.gridSize)
            return;

        // Draw center cell in red
        Cell? centerCell = grid.GetCell(testX, testZ);
        if (!centerCell.HasValue) return;

        Gizmos.color = rectCenterColor;
        Gizmos.DrawCube(centerCell.Value.cellPosition, new Vector3(grid.cellSize * 0.9f, 0.2f, grid.cellSize * 0.9f));

        // Draw neighbors in green
        Cell[] neighbors = grid.GetRectNeighbors(testX, testZ, rectRadius);
        Gizmos.color = rectNeighborColor;

        foreach (Cell neighbor in neighbors)
        {
            Gizmos.DrawCube(neighbor.cellPosition, new Vector3(grid.cellSize * 0.8f, 0.15f, grid.cellSize * 0.8f));
        }

        // Draw expected rectangle outline in yellow
        Gizmos.color = Color.yellow;
        int side = rectRadius * 2 + 1;
        float rectSize = side * grid.cellSize;
        Gizmos.DrawWireCube(centerCell.Value.cellPosition, new Vector3(rectSize, 0.25f, rectSize));

#if UNITY_EDITOR
        // Debug label showing expected vs actual count
        int expectedCount = side * side - 1; // -1 for center
        UnityEditor.Handles.Label(
            centerCell.Value.cellPosition + Vector3.up * 0.5f,
            $"Expected: {expectedCount}\nGot: {neighbors.Length}",
            new GUIStyle() { normal = new GUIStyleState() { textColor = Color.white }, fontSize = 12 }
        );
#endif
    }
}