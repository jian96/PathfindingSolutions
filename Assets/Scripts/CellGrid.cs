using UnityEngine;
using System.Collections.Generic;


public struct Cell
{
    public int cellSize { get; set; }
    public int cellIndex { get; set; }
    public Vector3 cellPosition { get; set; } // at center

    public Cell(int _cellSize, int _cellIndex, Vector3 _cellPosition)
    {
        cellSize = _cellSize;
        cellIndex = _cellIndex;
        cellPosition = _cellPosition;
    }
    public override string ToString() => "Cell Index: " + cellIndex;
}

public class CellGrid
{
    public int gridSize;
    public Vector3 gridOrigin;
    private Cell[,] cells;
    public int gridCount;
    public int cellSize;

    public CellGrid(int _gridSize, int _cellSize, Vector3 _origin)
    {
        gridSize = _gridSize;
        gridOrigin = _origin;
        cellSize = _cellSize;
        gridCount = gridSize * gridSize;

        cells = new Cell[gridSize, gridSize];
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = z * gridSize + x;
                Vector3 cellPos = new Vector3(
                    gridOrigin.x + (x * cellSize) + (cellSize * 0.5f),
                    gridOrigin.y,
                    gridOrigin.z + (z * cellSize) + (cellSize * 0.5f)
                );

                cells[z, x] = new Cell(cellSize, index, cellPos);
            }
        }
    }

    public Cell GetCell(int x, int z)
    {
        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
            return cells[z, x];

        return default;
    }

    public Cell? GetCellAtPosition(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
        int z = Mathf.FloorToInt((worldPos.z - gridOrigin.z) / cellSize);

        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
            return cells[z, x];

        return null;
    }

    public Cell[] GetNeighbors(int x, int z, bool includeDiagonals = false)
    {
        int maxNeighbors = includeDiagonals ? 8 : 4;
        Cell[] neighbors = new Cell[maxNeighbors];
        int count = 0;

        // N S E W
        if (z + 1 < gridSize) neighbors[count++] = cells[z + 1, x];
        if (z - 1 >= 0) neighbors[count++] = cells[z - 1, x];
        if (x + 1 < gridSize) neighbors[count++] = cells[z, x + 1];
        if (x - 1 >= 0) neighbors[count++] = cells[z, x - 1];

        // Diagonals
        if (includeDiagonals)
        {
            if (x + 1 < gridSize && z + 1 < gridSize) neighbors[count++] = cells[z + 1, x + 1];
            if (x - 1 >= 0 && z + 1 < gridSize) neighbors[count++] = cells[z + 1, x - 1];
            if (x + 1 < gridSize && z - 1 >= 0) neighbors[count++] = cells[z - 1, x + 1];
            if (x - 1 >= 0 && z - 1 >= 0) neighbors[count++] = cells[z - 1, x - 1];
        }

        if (count < maxNeighbors)
        {
            Cell[] result = new Cell[count];
            System.Array.Copy(neighbors, result, count);
            return result;
        }

        return neighbors;
    }

    public Cell[] GetNeighbors(Cell cell, bool includeDiagonals = false)
    {
        int x = cell.cellIndex % gridSize;
        int z = cell.cellIndex / gridSize;
        return GetNeighbors(x, z, includeDiagonals);
    }
}
