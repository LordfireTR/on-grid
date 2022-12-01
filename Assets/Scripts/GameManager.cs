using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] List<Unit> units = new List<Unit>();
    [SerializeField] GameObject gridObject;
    PathFinder pathFinder;
    Grid<PathNode> grid;
    UnitHandler playerUnitHandler;
    // Start is called before the first frame update
    void Start()
    {
        pathFinder = gridObject.GetComponent<GridHandler>().pathFinder;
        grid = pathFinder.GetGrid();
        units.Add(new Unit(){Object = player, UnitHandler = player.GetComponent<UnitHandler>()});
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            units[0].UnitHandler.SetAction(2);
        }
    }
}

public class Unit
{
    public GameObject Object { get; set; }
    public UnitHandler UnitHandler { get; set; }
}
