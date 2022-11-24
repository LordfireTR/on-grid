using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    Grid<PathNode> grid;
    public int x, z;

    public int gCost, hCost, fCost;
    public PathNode cameFromNode;
    public List<PathNode> neighbourList;
    public bool isWalkable, isOccupied, isGoal;

    public PathNode(Grid<PathNode> grid, int x, int z)
    {
        this.x = x;
        this.z = z;
        this.grid = grid;
    }

    public void SetNeighbourList()
    {
        neighbourList = new List<PathNode>();

        if (x >= 1)
        {
            //Left
            neighbourList.Add(grid.GetGridObject(x-1, z));
            //Down Left
            if (z >= 1)
            {
                neighbourList.Add(grid.GetGridObject(x-1, z-1));
            }
            //Up Left
            if (z <= grid.GetHeight() - 1)
            {
                neighbourList.Add(grid.GetGridObject(x-1, z+1));
            }
        }

        if (x <= grid.GetWidth() - 1)
        {
            //Right
            neighbourList.Add(grid.GetGridObject(x+1, z));
            //Down Right
            if (z >= 1)
            {
                neighbourList.Add(grid.GetGridObject(x+1, z-1));
            }
            //Up Left
            if (z <= grid.GetHeight() - 1)
            {
                neighbourList.Add(grid.GetGridObject(x+1, z+1));
            }
        }
        //Down
        if (z >= 1)
        {
            neighbourList.Add(grid.GetGridObject(x, z-1));
        }
        if (z <= grid.GetHeight() - 1)
        {
            neighbourList.Add(grid.GetGridObject(x, z+1));
        }
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + z;
    }
}
