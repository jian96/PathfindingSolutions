using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;

/// <summary>
/// Cell is the most basic unit in our project
/// cellIndex:      Number identifer for cell
/// cellSize:       Size of the cell, relative to the Unity world space
/// cellPosition:   Position of the cell, relative to the Unity world space. Represents dead center of the cell
///                 Currently, there are plans to consider the upward and downward directions - this is the reason
///                 it's a Vector3
/// </summary>
public struct Cell
{
    public int cellIndex { get; set; }
    public int cellSize { get; set; }
    public Vector3 cellPosition { get; set; } // at dead center of cell


    public Cell(int _cellIndex, int _cellSize, Vector3 _cellPosition)
    {
        cellSize = _cellSize;
        cellIndex = _cellIndex;
        cellPosition = _cellPosition;
    }
    public override string ToString() => "Cell Index: " + cellIndex;
}

/// <summary>
/// This is the most basic container in our project. 
/// It is a 1D array of cells that's supposed to represent a 2D square world, so we can use size in our traversal formula
/// Index to 2D position formula: (Size of the world)*(Z pos) + (X pos)
/// Currently, there are plans to use the Y dimension, so the Vector3 that represents the Unity world space position of the gridOrigin
/// </summary>

public class CellGrid
{
    public int gridSize;
    public Vector3 gridOrigin;
    private Cell[] cells;
    public int gridCount;
    public int cellSize;
    static readonly (int dx, int dz)[] Diagonals =
    {
        ( 1,  1),
        (-1,  1),
        ( 1, -1),
        (-1, -1),
    };

    int Index(int x, int z) => z * gridSize + x;
    public CellGrid(int _gridSize, int _cellSize, Vector3 _origin)
    {
        gridSize = _gridSize;
        gridOrigin = _origin;
        cellSize = _cellSize;
        gridCount = gridSize*gridSize;

        cells = new Cell[gridCount];
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = Index(x, z); // Index to 2D position formula: (Size of the world)*(Z pos) + (X pos)
                Vector3 cellPos = new Vector3(
                    gridOrigin.x + (x * cellSize) + (cellSize * 0.5f),
                    gridOrigin.y, // We'll probably want to add some Y functionality, this is why it's a Vector3
                    gridOrigin.z + (z * cellSize) + (cellSize * 0.5f)
                );

                cells[index] = new Cell(index, cellSize, cellPos);
            }
        }
    }

    public Cell GetCell(int x, int z)
    {
        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
            return cells[z*gridSize + x];

        return default;
    }

    public Cell? GetCellAtPosition(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
        int z = Mathf.FloorToInt((worldPos.z - gridOrigin.z) / cellSize);

        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
            return cells[z * gridSize + x];

        return null;
    }

    public Cell[] GetNeighbors(int x, int z, bool includeDiagonals = false)
    {
        int maxNeighbors = includeDiagonals ? 8 : 4;
        Cell[] neighbors = new Cell[maxNeighbors];
        int count = 0;

        // N S E W Index(x, z)
        if (z + 1 < gridSize) neighbors[count++] = cells[Index(x, z + 1)];
        if (z - 1 >= 0) neighbors[count++] = cells[Index(x, z - 1)];
        if (x + 1 < gridSize) neighbors[count++] = cells[Index(x + 1, z)];
        if (x - 1 >= 0) neighbors[count++] = cells[Index(x - 1, z)];
        
        // For diagonals
        if (includeDiagonals)
        {
            foreach (var (dx, dz) in Diagonals)
            {
                int nx = x + dx;
                int nz = z + dz;

                if (nx >= 0 && nx < gridSize &&
                    nz >= 0 && nz < gridSize)
                {
                    neighbors[count++] = cells[Index(nx, nz)];
                }
            }
        }

        // Not sure if this is necessary - for trimming array
        if (count < maxNeighbors)
        {
            Cell[] neighbors_edge = new Cell[count];
            System.Array.Copy(neighbors, neighbors_edge, count);
            return neighbors_edge;
        }
        return neighbors;
    }

    /// <summary>
    /// Overload for GetNeighbors() when passed in a Cell object
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="includeDiagonals"></param>
    /// <returns></returns>
    public Cell[] GetNeighbors(Cell cell, bool includeDiagonals = false)
    {
        int x = cell.cellIndex % gridSize;
        int z = cell.cellIndex / gridSize;
        return GetNeighbors(x, z, includeDiagonals);
    }

    public Cell[] GetRectNeighbors(int x, int z, int rectRadius)
    {
        int side = rectRadius * 2 + 1;
        int maxNeighbors = 1 << rectRadius - 1;
        Cell[] neighbors = new Cell[maxNeighbors];
        int count = 0;

        // For a rectangle of size rectRadius
        // Outer loop: starts from bottom-left and continues upward (dz - ^ ^ ^)
        // Inner loop: starts from most-left and continues rightward (dx - > > >)
        for (int dz = -rectRadius; dz <= rectRadius; dz++)
        {
            for (int dx = -rectRadius; dx <= rectRadius; dx++)
            {
                if (dx == 0 && dz == 0)
                    continue; // skip self
                int nx = x + dx;
                int nz = z + dz;

                // we can probably precalculate the real size to skip out-of-bounds checks entirely
                if (nx < 0 || nx >= gridSize ||
                    nz < 0 || nz >= gridSize)
                    continue;

                neighbors[count++] = cells[Index(nx, nz)];
            }
        }
        if (count < neighbors.Length)
            Array.Resize(ref neighbors, count);

        return neighbors;
    }

}
