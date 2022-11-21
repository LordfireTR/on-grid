using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    Grid<PathNode> grid;
    List<PathNode> openList, closedList;

    int MOVE_DIAGONAL_COST = 140;
    int MOVE_STRAIGHT_COST = 10;

    public PathFinder(int width, int height, float cellSize, Vector3 originPosition)
    {
        grid = new Grid<PathNode>(width, height, cellSize, originPosition, (Grid<PathNode> g, int x, int z) => new PathNode(g, x, z));

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            {
                grid.GetGridObject(x, z).SetNeighbourList();
                grid.GetGridObject(x, z).isWalkable = true;
                grid.GetGridObject(x, z).isOccupied = false;
            }
        }
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    public List<PathNode> FindPath(int startX, int startZ, int targetX, int targetZ)
    {
        PathNode startNode = grid.GetGridObject(startX, startZ);
        PathNode targetNode = grid.GetGridObject(targetX, targetZ);

        if (targetNode == null)
        {
            return null;
        }

        openList = new List<PathNode>{ startNode };
        closedList= new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            {
                PathNode pathNode = grid.GetGridObject(x, z);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();

                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, targetNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == targetNode)
            {
                return CalculatePath(targetNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in currentNode.neighbourList)
            {
                if (neighbourNode == null)
                {
                    continue;
                }
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (!neighbourNode.isWalkable && neighbourNode.isOccupied)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, targetNode);
                    neighbourNode.CalculateFCost();
                }

                if (!openList.Contains(neighbourNode))
                {
                    openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 targetWorldPosition)
    {
        grid.GetXZ(startWorldPosition, out int startX, out int startZ);
        grid.GetXZ(targetWorldPosition, out int targetX, out int targetZ);

        List<PathNode> path = FindPath(startX, startZ, targetX, targetZ);

        if (path == null)
        {
            return null;
        }

        List<Vector3> vector3Path = new List<Vector3>();
        foreach (PathNode node in path)
        {
            vector3Path.Add(grid.GetCellPosition(node.x, node.z) + new Vector3(1, 0, 1) * grid.GetCellSize() * .5f);
        }
        return vector3Path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int zDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        return Mathf.Min(xDistance, zDistance) * MOVE_DIAGONAL_COST + remaining * MOVE_STRAIGHT_COST;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodeList)
    {
        PathNode lowestFCostNode = nodeList[0];

        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = nodeList[i];
            }
        }
        return lowestFCostNode;
    }

    private List<PathNode> CalculatePath(PathNode targetNode)
    {
        List<PathNode> path = new List<PathNode>();
        
        path.Add(targetNode);
        PathNode currentNode = targetNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        return path;
    }
}