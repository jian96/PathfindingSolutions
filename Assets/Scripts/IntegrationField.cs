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
        int prio = prioqueue[--_count].value;
        int currentIndex = 0;
        int leftChildIndex = currentIndex * 2 + 1;
        int rightChildIndex = currentIndex * 2 + 2;
        prioqueue[0] = prioqueue[_count];
        while (leftChildIndex % 2 == 1 && (prioqueue[leftChildIndex].value < prio || (rightChildIndex % 2 == 0 && prioqueue[rightChildIndex].value < prio)))
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
        prioqueue[currentIndex].value = prio;
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
        pq.Enqueue((1, 1));
        //while (pq.Count() > 0)
        //{
        //    (int, int) entry = pq.Dequeue();
            //if (cellGrid.cells[targetCellIndex + 1].cellMovementCost < 255) neighbors[count++] = cells[Index(x, z + 1)];
            //if (z - 1 >= 0) neighbors[count++] = cells[Index(x, z - 1)];
            //if (x + 1 < gridSize) neighbors[count++] = cells[Index(x + 1, z)];
            //if (x - 1 >= 0) neighbors[count++] = cells[Index(x - 1, z)];
        }
    }

}
