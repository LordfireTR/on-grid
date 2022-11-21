using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    [SerializeField] GameObject grid;
    PathFinder pathFinder;
    Camera mainCam;

    List<Vector3> path;
    float moveSpeed = .3f, planckLength;
    int moveLimit = 2, actionLimit = 2;
    bool isMoving;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Start()
    {
        pathFinder = grid.GetComponent<GridHandler>().pathFinder;
        planckLength = moveLimit * moveSpeed * Time.deltaTime;
        PlaceUnit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit) && !isMoving && actionLimit > 0)
            {
                pathFinder.GetGrid().GetXZ(raycastHit.point, out int x, out int z);
                SetPath(raycastHit.point);
            }
        }
        
        HandleMovement();
    }

    void SetPath(Vector3 targetCellPosition)
    {
        pathFinder.GetGrid().GetXZ(transform.position, out int xCurrent, out int zCurrent);
        pathFinder.GetGrid().GetXZ(targetCellPosition, out int xTarget, out int zTarget);
        pathFinder.GetGrid().GetGridObject(transform.position).isOccupied = false;

        if (Mathf.Abs(xCurrent - xTarget) + Mathf.Abs(zCurrent - zTarget) <= moveLimit)
        {
            path = pathFinder.FindPath(transform.position, targetCellPosition);
            isMoving = true;
            actionLimit--;
        }

        if (path == null)
        {
            SetPath(transform.position);
        }

        if (path.Count <= 1)
        {
            actionLimit++;
        }
    }

    void HandleMovement()
    {
        if (path != null && path.Count > 1)
        {
            pathFinder.GetGrid().GetGridObject(transform.position).isOccupied = false;
            Vector3 nextCellPosition = path[1];
            nextCellPosition.y = transform.position.y;

            if (Vector3.Distance(transform.position, nextCellPosition) > planckLength)
            {
                transform.position -= (transform.position - nextCellPosition).normalized * planckLength;
            }
            else
            {
                transform.position = nextCellPosition;
                path.RemoveAt(0);
            }
        }
        else
        {
            isMoving = false;
            pathFinder.GetGrid().GetGridObject(transform.position).isOccupied = true;
        }
    }

    void PlaceUnit()
    {
        pathFinder.GetGrid().GetXZ(transform.position, out int x, out int z);
        Vector3 properPosition = pathFinder.GetGrid().GetCellPosition(x, z) + new Vector3(1, 0, 1) * pathFinder.GetGrid().GetCellSize() * .5f;
        if (transform.position != properPosition)
        {
            transform.position = properPosition;
            pathFinder.GetGrid().GetGridObject(x, z).isOccupied = true;
        }
    }
}
