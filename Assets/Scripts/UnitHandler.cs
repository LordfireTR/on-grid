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
    float baseSpeed = 1, planckLength;
    int moveLimit = 5, actionLimit = 2;

    State state;

    void Awake()
    {
        state = State.Normal;
        pathLine = lineRenderer.GetComponent<LineRenderer>();
        pathLine.positionCount = 0;
        mainCam = Camera.main;
    }

    void Start()
    {
        pathFinder = grid.GetComponent<GridHandler>().pathFinder;
        PlaceUnit();
    }

    enum State
    {
        Normal,
        Moving,
        Waiting,
        Dead
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                if (Input.GetMouseButton(0))
                {
                    Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        pathFinder.GetGrid().GetXZ(raycastHit.point, out int x, out int z);
                        SetPath(raycastHit.point);
                    }
                    else
                    {
                        SetPath(transform.position);
                    }
                    HighlightPath();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    //If path length > 1 spend action
                    if (path.Count > 1)
                    {
                        state = State.Moving;
                        SpendAction();
                    }
                }
                break;
            case State.Moving:
                HandleMovement();
                break;
            case State.Waiting:
                if (actionLimit > 0)
                {
                    state = State.Normal;
                }
                break;
            case State.Dead:
                break;
        }
    }

    void SetPath(Vector3 targetCellPosition)
    {
        pathFinder.GetGrid().GetXZ(transform.position, out int xCurrent, out int zCurrent);
        pathFinder.GetGrid().GetXZ(targetCellPosition, out int xTarget, out int zTarget);
        pathFinder.GetGrid().GetGridObject(transform.position).isOccupied = false;

        int stepCount = Mathf.Abs(xCurrent - xTarget) + Mathf.Abs(zCurrent - zTarget);

        if (stepCount <= moveLimit)
        {
            path = pathFinder.FindPath(transform.position, targetCellPosition);
        }

        if (path == null || stepCount > moveLimit)
        {
            SetPath(transform.position);
        }
    }

    void HandleMovement()
    {
        if (path != null && path.Count > 1)
        {
            Vector3 nextCellPosition = path[1];
            nextCellPosition.y = transform.position.y;
            planckLength = moveLimit * baseSpeed * Time.deltaTime;

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
            pathFinder.GetGrid().GetGridObject(transform.position).isOccupied = true;
            DisablePathLine();
            state = State.Waiting;
            
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

    public void SpendAction()
    {
        actionLimit--;
    }

    public void GainAction()
    {
        actionLimit++;
    }

    public void SetAction(int i)
    {
        if (i > -1)
        {
            actionLimit = i;
        }
    }

    void HighlightPath()
    {
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
