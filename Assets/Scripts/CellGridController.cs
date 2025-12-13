 using UnityEngine;

public class CellGridController : MonoBehaviour
{
    public CellGrid cellGrid { get; private set; }
    [SerializeField] public int gridSize = 10;
    [SerializeField] public int cellSize;
    [SerializeField] public Vector3 gridOrigin = Vector3.zero; 

    public void Start()
    {
        InitializeCellGrid();
    }
    public void InitializeCellGrid()
    {
        cellGrid = new CellGrid(gridSize, cellSize, gridOrigin);
    }
}
