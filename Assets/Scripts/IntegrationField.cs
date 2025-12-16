using UnityEngine;
using System.Collections.Generic;


public class IntegrationField : MonoBehaviour // Dijkstra map
{
	private CellGrid cellGrid;
	private int[] costs;

	public IntegrationField(CellGrid _cellGrid)
	{
		cellGrid = _cellGrid;
		costs = new int[cellGrid.gridCount];
	}
	
	// 1.	Acknowledge origin
	// 2.	Find origin eligible neighbors - add them all to prio queue (first iteration should have 4 assuming plain grid, each overlap should be removed due to dequeuing)
	// 3.	See if VISITED or NOT VISITED
	//	3a. if VISITED: skip
	//  3b. if NOT VISITED: SET neighborConnectCost = currentTraverseToCost + neighborMovementCost
	//		IMPORTANT: for edge cases where an expensive path, e.g. crossing a 50-points bridge vs walking around
	//				   graph distance, NOT spatial distance - don't forget this for graphs
	//  3bi.set neighbor's 
	// 4.	Consider the neighbors as new origins
	public void populateCosts(int targetX, int targetZ)
	{
		for (int i = 0; i < costs.Length; i++)
		costs[i] = int.MaxValue;

        // minheaps seems like cheating - research more please
		// TODO: implement priority queue
        PriorityQueue<int, int> solveQueue = new PriorityQueue<int, int>();

        int targetCellIndex = targetZ * cellGrid.gridSize + targetX; // continue
        solveQueue.Enqueue((1,1));
        while (solveQueue.Count > 0) 
		{
            int currentCellIndex = solveQueue.Dequeue();
            if (cellGrid.cells[targetCellIndex + 1].cellMovementCost < 255 ) neighbors[count++] = cells[Index(x, z + 1)];
            if (z - 1 >= 0) neighbors[count++] = cells[Index(x, z - 1)];
            if (x + 1 < gridSize) neighbors[count++] = cells[Index(x + 1, z)];
            if (x - 1 >= 0) neighbors[count++] = cells[Index(x - 1, z)];
        }
	}

}
