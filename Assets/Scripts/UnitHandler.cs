using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    [SerializeField] GameObject grid;
    PathFinder pathFinder;
    Camera mainCam;
    [SerializeField] GameObject lineRenderer;
    LineRenderer pathLine;

    List<Vector3> path;
    float moveSpeed = .3f, planckLength;
    int moveLimit = 5, actionLimit = 2;
    bool isMoving, startMovement, showPath;

    void Awake()
    {
        pathLine = lineRenderer.GetComponent<LineRenderer>();
        lineRenderer.SetActive(false);
        mainCam = Camera.main;
        planckLength = moveLimit * moveSpeed * Time.deltaTime;
    }

    void Start()
    {
        pathFinder = grid.GetComponent<GridHandler>().pathFinder;
        PlaceUnit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit) && !isMoving && actionLimit > 0)
            {
                showPath = true;
                startMovement = false;
                pathFinder.GetGrid().GetXZ(raycastHit.point, out int x, out int z);
                SetPath(raycastHit.point);
                HighlightPath();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startMovement = true;

            //If path length > 1 spend action
            if (path.Count > 1)
            {
                SpendAction();
            }
        }

        if (!startMovement && !showPath)
        {
            DisablePathLine();
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
        }

        if (path == null)
        {
            SetPath(transform.position);
        }
    }

    void HandleMovement()
    {
        if (path != null && path.Count > 1 && startMovement)
        {
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
            isMoving = true;
        }
        else
        {
            isMoving = false;
            startMovement = false;
            showPath = false;
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

    void SpendAction()
    {
        actionLimit--;
    }

    void HighlightPath()
    {
        lineRenderer.SetActive(true);
        pathLine.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            pathLine.SetPosition(i, path[i] + Vector3.up*.5f);
        }
    }

    void DisablePathLine()
    {
        pathLine.positionCount = 0;
    }
}
