using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> NPCs = new List<GameObject>();
    [SerializeField] GameObject gridObject;
    PathFinder pathFinder;
    Grid<PathNode> grid;
    // Start is called before the first frame update
    void Start()
    {
        pathFinder = gridObject.GetComponent<GridHandler>().pathFinder;
        grid = pathFinder.GetGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
