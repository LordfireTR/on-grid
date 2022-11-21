using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    PathFinder PathFinder;
    int gridWidth = 20, gridHeight = 20, currentX, currentZ;
    float cellSize = 5;
    [SerializeField] GameObject ground;
    
    void Start()
    {
        PathFinder = new PathFinder(gridWidth, gridHeight, 5, Vector3.zero);
        currentX = 0;
        currentZ = 0;

        ground.transform.localScale = new Vector3(gridWidth * cellSize, .1f, gridHeight * cellSize);
        ground.transform.position = new Vector3(gridWidth * cellSize, -.1f, gridHeight * cellSize) * .5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                PathFinder.GetGrid().GetXZ(raycastHit.point, out int x, out int z);
                Debug.Log(x + "," + z);

                List<PathNode> path = PathFinder.FindPath(currentX, currentZ, x, z);

                if (path != null)
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(path[i].x, 0, path[i].z) * 5f + new Vector3(1, .4f, 1) * 2.5f, new Vector3(path[i+1].x, 0, path[i+1].z) * 5f + new Vector3(1, .4f, 1) * 2.5f, Color.yellow, 100f);
                    }
                    currentX = path[path.Count-1].x;
                    currentZ = path[path.Count-1].z;
                }
            }
        }
    }
}
