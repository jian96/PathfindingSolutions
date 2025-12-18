using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

// Pre-edit to work with the pathfinding solution, e.g. Debug.Log and Vector objects
// This version works as a separate C# class
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

    public static void Main()
    {
        Console.WriteLine("Creating PQ");
        PriorityQueue pq = new(50);
        pq.toString(0);
        Console.WriteLine("Adding 10-larger than before elements\n");
        var data = new List<(int dx, int dz)> { (0, 0) };
        for (int i = 1; i < 10; i++)
        {
            // Access the previous element (data[i-1]) and increment its fields
            int nx = data[i - 1].dx + 2;
            int nz = data[i - 1].dz + 2;
            (int, int) n = (nx, nz);
            data.Add(n);
            // for defaulted identifiers
            // data[i].Item1 = data[i-1].Item1 + 1;
            // data[i].Item2 = data[i-1].Item2 + 1;
        }

        for (int i = 0; i < data.Count(); i++)
        {
            Console.WriteLine("{0}", data[i].ToString());
        }




        for (int i = 0; i < data.Count(); i++)
        {
            (int, int) curdata = data[i];
            pq.Enqueue(data[i]);
        }

        pq.toString(1);
        var datatwo = (1, 1);
        pq.Enqueue(datatwo);
        Console.WriteLine("Adding a low member");
        pq.toString(1);

    }

    public PriorityQueue(int maxSize)
    {
        _count = 0;
        prioqueue = new (int value, int prio)[maxSize];
        for (int i = 0; i < maxSize; i++)
        {
            prioqueue[i] = (int.MaxValue, -1); // or whatever sentinel makes sense
        }
    }

    /// <summary>
	/// Enqueue operation on the PriorityQueue
    /// _count is the currentIndex of the cell we work on
    /// parentIndex is the index for the parent of the child (the thing you're looking to operate on)
    /// </summary>
    /// <param name="tuple"></param>
    public void Enqueue((int value, int index) tuple)
    {
        int currentIndex = _count;
        int parentIndex = (_count - 1) / 2;

        if (_count == 0)
        {
            prioqueue[_count++] = tuple;
            return; // LOW TODO: redundant return, research
        }
        bool bA = prioqueue[parentIndex].value != 0;
        bool bB = prioqueue[parentIndex].value > tuple.value;

        while (prioqueue[parentIndex].value != 0 && prioqueue[parentIndex].value > tuple.value) // TODO: don't use sentinel values here
        {
            prioqueue[currentIndex] = prioqueue[parentIndex];
            prioqueue[parentIndex] = tuple;
            currentIndex = (currentIndex - 1) / 2;
            parentIndex = (currentIndex - 1) / 2;
        }
        prioqueue[currentIndex] = tuple;
        _count++;
    }

    public void toString(int displayContents)
    {
        Console.WriteLine("Size is {0}\n" + "Max size is {1}\n", _count, prioqueue.Length);
        if (Convert.ToBoolean(displayContents))
        {
            Console.WriteLine("Printing Contents:\n");
            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine("{0}", prioqueue[i]);
            }
        }
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
