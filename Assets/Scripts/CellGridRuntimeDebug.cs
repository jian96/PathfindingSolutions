using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CellGridRuntimeDebug : MonoBehaviour
{
    public Camera cam;
    public Transform targetBall;

    public LayerMask groundMask;

    CellGridController controller;
    CellGrid grid;

    InputAction heldDown;

    private bool isHeld = false;


    void Start()
    {
        controller = GetComponent<CellGridController>();
        grid = controller.cellGrid;
        heldDown = InputSystem.actions.FindAction("Click");
    }


    void Update()
    {
        
        // HandleTargetDragging();
        // HandleObstaclePlacement();
    }


    void HandleTargetDragging()
    {

    }



    void MoveTarget(Cell cell)
    {
        targetBall.position = cell.cellPosition + Vector3.up * .5f;

        Vector2Int coords = grid.Coordinates(cell.cellIndex);

        GetComponent<CellGridDebug>().dijkstraTargetX = coords.x;
        GetComponent<CellGridDebug>().dijkstraTargetZ = coords.y;

        // Recalculate immediately
        controller.integrationField.populateCosts(
            GetComponent<CellGridDebug>().dijkstraTargetX,
            GetComponent<CellGridDebug>().dijkstraTargetZ
        );
    }



    void HandleObstaclePlacement()
    {
        //if(!Input.GetMouseButtonDown(1))
        //    return;


        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);


        //if(Physics.Raycast(ray,out RaycastHit hit,100,groundMask))
        //{
        //    Cell? cell = grid.GetCellAtPosition(hit.point);

        //    if(cell.HasValue)
        //    {
        //        AddObstacle(cell.Value);
        //    }
        //}
    }



    //void AddObstacle(Cell cell)
    //{
    //    UnityEngine.Debug.Log(
    //        $"Obstacle added {GetComponent<CellGridDebug>().dijkstraTargetX},{GetComponent<CellGridDebug>().dijkstraTargetZ}"
    //    );
            

    //    cell.walkable = false;


    //    controller.integrationField.populateCosts(
    //        GetComponent<CellGridDebug>().dijkstraTargetX,
    //        GetComponent<CellGridDebug>().dijkstraTargetZ
    //    );
    //}
}