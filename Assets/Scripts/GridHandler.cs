using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    public PathFinder pathFinder { get; private set; }
    [SerializeField] int width, height;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 originPosition;
    [SerializeField] Transform ground;

    void Awake()
    {
        pathFinder = new PathFinder(width, height, cellSize, originPosition);

        Vector3 groundArea = new Vector3(width, 0, height) * cellSize;
        ground.localScale = groundArea/10 + Vector3.up;
        ground.position = originPosition + groundArea/2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
