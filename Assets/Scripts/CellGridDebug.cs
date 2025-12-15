using UnityEngine;

public class CellGridDebug : MonoBehaviour
{
    [Header("Visualization Settings")]
    public bool showGrid = true;
    public bool showIndices = false;
    public bool showCellCenters = false;
    public Color gridColor = Color.white;
    public Color centerColor = Color.yellow;
    public int shouldBeFalse = 2;

    void OnDrawGizmos()
    {
        if (!showGrid) return;

        CellGridController controller = GetComponent<CellGridController>();
        CellGrid grid = controller.cellGrid;
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
                    Cell cell = grid.GetCell(x, z);
                    Gizmos.DrawSphere(cell.cellPosition, 0.1f);
                }
            }
        }

        // Draw indices
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
                    Cell cell = grid.GetCell(x, z);
                    UnityEditor.Handles.Label(cell.cellPosition, cell.cellIndex.ToString(), style);
                }
            }
        }
#endif
    }
}