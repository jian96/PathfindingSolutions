using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

// Pre-edit to work with the pathfinding solution, e.g. Debug.Log and Vector objects
// This version works as a separate C# class
public class PriorityQueue
{
    public (int value, int prio)[] prioqueue;
    public int _count;
    public int Count => _count;

    /// <summary>
    /// Constructor that takes in max size for instance, e.g. it should be the PQ's customer's worldsize
    /// </summary>
    /// <param name="maxSize"></param>
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

    public (int, int) Dequeue()
    {
        (int, int) retval = prioqueue[0];
        (int cellIndex, int cost) lastElement = prioqueue[--_count];

        int currentIndex = 0;
        int leftChildIndex = currentIndex * 2 + 1;
        int rightChildIndex = currentIndex * 2 + 2;

        prioqueue[0] = lastElement;
        while (leftChildIndex % 2 == 1 && (prioqueue[leftChildIndex].value < lastElement.cost || (rightChildIndex % 2 == 0 && prioqueue[rightChildIndex].value < lastElement.cost)))
        {
            if (rightChildIndex % 2 == 1 || prioqueue[leftChildIndex].value < prioqueue[rightChildIndex].value)
            {
                prioqueue[currentIndex] = prioqueue[leftChildIndex];
                currentIndex = leftChildIndex;
            }
            else
            {
                prioqueue[currentIndex] = prioqueue[rightChildIndex];
                currentIndex = rightChildIndex;
            }
            leftChildIndex = currentIndex * 2 + 1;
            rightChildIndex = currentIndex * 2 + 2;
        }
        prioqueue[currentIndex] = lastElement;
        return retval;
    }

    /// <summary>
    /// ToString() override that returns info
    /// </summary>
    /// <returns>A formatted string containing current size, max size, and list of current content</returns>
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Size: {_count}, Max: {prioqueue.Length}");

        for (int i = 0; i < _count; i++)
        {
            sb.AppendLine($"[{i}]: {prioqueue[i]}");
        }

        return sb.ToString();
    }

}

public class IntegrationField // Dijkstra map
{
    private CellGrid cellGrid;
    public int[] costs { get; private set; }

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
        // reset cpsts
        for (int i = 0; i < costs.Length; i++)
            costs[i] = int.MaxValue;
        
        int gridSize = cellGrid.gridSize; // readability
        int targetCellIndex = targetZ * gridSize + targetX; // continue

        PriorityQueue pq = new(cellGrid.gridCount);
        costs[targetCellIndex] = 0;
        pq.Enqueue((0, targetCellIndex));   // origin
        while (pq.Count > 0)
        {
            var (currentCost, currentIndex) = pq.Dequeue();

            // ignore outdated entries
            if (currentCost > costs[currentIndex])
                continue;

            int x = currentIndex % gridSize;
            int z = currentIndex / gridSize;

            // visit neighbors
            TryRelax(x, z + 1);
            TryRelax(x, z - 1);
            TryRelax(x + 1, z);
            TryRelax(x - 1, z);

            void TryRelax(int nx, int nz)
            {
                if (nx < 0 || nz < 0 || nx >= gridSize || nz >= gridSize)
                    return;

                int nIndex = nz * gridSize + nx;
                int moveCost = cellGrid.cells[nIndex].cellMovementCost;

                if (moveCost >= 255) return; // blocked

                int newCost = currentCost + moveCost;

                if (newCost < costs[nIndex])
                {
                    costs[nIndex] = newCost;
                    pq.Enqueue((newCost, nIndex));
                }
            }
        }
    }

}
