using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private Square[,] BattleMap;
    [SerializeField] Square overSqaurePrefab;
    [SerializeField] GameObject overSquareContainer;

    private List<Square> rangeCache;

    private void EventSubscription()
    {
        // Unit
        GameEvents.Instance.onUnitChangePosition += UpdateUnitPosition;

        // Cursor
        GameEvents.Instance.onNewUnitClicked += GetUnitRange;
        GameEvents.Instance.onUnitDeselected += HideUnitRange;
    }

    // Get all tiles put in a 2d array according to their position
    private void Start()
    {
        EventSubscription();
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tileMap.cellBounds;
        int deltaX = bounds.max.x - bounds.min.x;
        int deltaY = bounds.max.y - bounds.min.y;

        BattleMap = new Square[deltaX,deltaY];
    
        for (int y = 0; y < deltaY; y++){
            for (int x = 0; x < deltaX; x++){

                //Instantiate a square and add it to the battlemap
                var overSquare = Instantiate(overSqaurePrefab, overSquareContainer.transform);
                overSquare.TieToMap(this, new Vector2Int(x,y), tileMap.GetTile(new Vector3Int(x+bounds.min.x,y+bounds.min.y,(int)transform.position.z)));
                BattleMap[x,y] = overSquare;


                // Move it to the corresponding spot on the grid
                var tilePosition = tileMap.GetCellCenterWorld(new Vector3Int(x+bounds.min.x,y+bounds.min.y,(int)transform.position.z+1));
                overSquare.transform.position = tilePosition;
            }
        }
    }

    private void UpdateUnitPosition(Component sender, object data)
    {
        if (sender is not Unit || data is not Vector2Int) return;
        
        Unit unit = (Unit) sender;
        Vector2Int coords = (Vector2Int) data;

        // Quickest way I found online to check if an array index is in bounds.
        if (coords.x < 0 || coords.x >= BattleMap.Length || coords.y < 0 || coords.y >= BattleMap.Length) return;

        // Move unit and tell the square it has a dude on it
        unit.gameObject.transform.position = BattleMap[coords.x,coords.y].transform.position;
        BattleMap[coords.x,coords.y].unitOn = unit;
    }

    public void HideUnitRange(Component sender)
    {
        if (sender is not Cursor) return;

        foreach (var sq in rangeCache) sq.HideTile();
    }

    private void GetUnitRange(Component sender, object data)
    {
        if (sender is not Cursor || data is not Vector2Int) return;

        Vector2Int pos = (Vector2Int) data;

        rangeCache = GetMovSquaresInRange(pos);
        //List<Square> atkSqs = GetAtkSquaresInRange(pos);
        foreach (var sq in rangeCache) sq.ShowTile();
    }
    

    // Get mov squares in range
    private List<Square> GetMovSquaresInRange(Vector2Int pos)
    {
        //Declare three lists, frontier, explored, and InRangeTiles
        var frontier = new List<Square>();
        var explored = new List<Square>();
        var InRangeTiles = new List<Square>();

        // start tile path cost = 0
        Square startSq = BattleMap[pos.x,pos.y];
        Unit unit = startSq.unitOn;
        startSq.pathCost = 0;

        // initialize frontier and inrangetiles with start tile
        frontier.Add(startSq);
        InRangeTiles.Add(startSq);

        // While the frontier isn't empty
        while (frontier.Any())
        {
            // current tile = last member of frontier
            var tile = frontier.First();
            frontier.Remove(tile);
            explored.Add(tile);

            if (tile.terrain["cost"] <= unit.stats["mov"])
            {
                InRangeTiles.Add(tile);
            }

            // get neighbor tiles and remove explored
            var neighbors = GetAdjSquares(tile.coords);
            neighbors = neighbors.Except(explored).ToList();

            // For each neighbor, if an enemy isn't on the square add to frontier. If cost lower than you thought, update
            foreach (var neighbor in neighbors)
            {
                if (startSq.unitOn.GetTeam() ==  neighbor.unitOn?.GetTeam()) continue;
                var newCost = tile.pathCost + neighbor.terrain["cost"];
                if (newCost < neighbor.pathCost) neighbor.pathCost = newCost;
                if (newCost <= unit.stats["mov"]) frontier.Add(neighbor);
            } 
        }
        // Reset all path costs before returning
        foreach (var tile in explored) tile.pathCost = 99;
        
        return InRangeTiles.Distinct().ToList();
    }

    private List<Square> GetAdjSquares(Vector2Int pos)
    {
        List<Square> adjSquares = new List<Square>();
        
        // Left and Bottom index check and addition to list
        if (pos.x-1 >= 0) adjSquares.Add(BattleMap[pos.x-1 , pos.y]);
        if (pos.y-1 >= 0) adjSquares.Add(BattleMap[pos.x, pos.y-1]);
        
        // Right and Up index check and addition to list
        if (pos.x+1 <= BattleMap.GetLength(1)) adjSquares.Add(BattleMap[pos.x+1, pos.y]);
        if (pos.y+1 <= BattleMap.GetLength(0)) adjSquares.Add(BattleMap[pos.x, pos.y+1]);
        
        return adjSquares;
    }

}

/*
private int GetManhattanDistance(Square sqA, Square sqB)
{
    return Mathf.Abs(sqA.gridLocation.x - sqB.gridLocation.x) + Mathf.Abs(sqA.gridLocation.y - sqB.gridLocation.y);
}
*/