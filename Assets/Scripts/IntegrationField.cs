using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;


/// <summary>
/// This is the PriorityQueue implementation for pathfinding solutions. Consideration is made for usage in A*, IntegrationField, and FlowField
/// Therefore, only Enqueue and Pop operations are implemented/required for now.
/// </summary>
public class PriorityQueue
{
	public (int value, int prio)[] prioqueue;
	public int _count; // TODO: make this a property later

    /* 
	add (int val){
	int emptyspotindex = pq.getLength
	pq[emptyspotindex] = val
	while (pq[(emptyspotindex-1)/2] != null && pq[(emptyspotindex-1)/2] > val)
	pq[emptyspotindex] = pq[(emptyspotindex-1)/2]
	pq[(emptyspotindex-1)/2] = val
	emptyspotindex = (emptyspotindex-1)/2
	 */

    public PriorityQueue(int size) 
	{
        _count = 0;
		prioqueue = new (int value, int prio)[size];
	}

    /// <summary>
	/// Enqueue operation on the PriorityQueue
    /// _count is the currentIndex of the cell we work on
    /// parentIndex is the index for the parent of the child (the thing you're looking to operate on)
    /// </summary>
    /// <param name="cellData"></param>
    public void Enqueue((int cellValue, int cellIndex) cellData)
	{
		int parentIndex = (_count - 1) / 2;
		int currentIndex = _count;

        if (_count == 0)
		{
			prioqueue[_count++] = cellData;
			return; // LOW TODO: redundant return, research
		}
		
		while (prioqueue[parentIndex].value != 0 && prioqueue[parentIndex].value > cellData.cellValue) // TODO: don't use sentinel values here
		{
			prioqueue[currentIndex] = prioqueue[parentIndex];
            prioqueue[parentIndex] = cellData;
			currentIndex = (currentIndex - 1) / 2;
            parentIndex = (currentIndex - 1) / 2;
        }
		_count++;
	}

}

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

        int targetCellIndex = targetZ * cellGrid.gridSize + targetX; // continue
		PriorityQueue pq = new(cellGrid.gridCount);
        pq.Enqueue((1,1));
        while (pq.Count > 0) 
		{
            int currentCellIndex = pq.Dequeue();
            if (cellGrid.cells[targetCellIndex + 1].cellMovementCost < 255 ) neighbors[count++] = cells[Index(x, z + 1)];
            if (z - 1 >= 0) neighbors[count++] = cells[Index(x, z - 1)];
            if (x + 1 < gridSize) neighbors[count++] = cells[Index(x + 1, z)];
            if (x - 1 >= 0) neighbors[count++] = cells[Index(x - 1, z)];
        }
	}

}
