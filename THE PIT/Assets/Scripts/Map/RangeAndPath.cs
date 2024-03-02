using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class RangeAndPath
{
    // =============
    // RANGE FINDING
    // =============
    public List<Square> GetMovSquaresInRange(Square[,] sqArray, Vector2Int pos)
    {
        //Declare three lists, frontier, explored, and InRangeTiles
        var frontier = new List<Square>();
        var explored = new List<Square>();
        var InRangeTiles = new List<Square>();

        // start tile path cost = 0
        Square startSq = sqArray[pos.x,pos.y];
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

            //Debug.Log($"{frontier.Count}");
            //Debug.Log($"{explored.Count}");

            if (tile.pathCost <= unit.stats["mov"])
            {
                InRangeTiles.Add(tile);
            }

            // get neighbor tiles and remove explored
            var neighbors = GetAdjSquares(sqArray, tile.coords).Except(explored).ToList();

            // For each neighbor, if an enemy isn't on the square add to frontier. If cost lower than you thought, update
            foreach (var neighbor in neighbors)
            {
                if (neighbor.unitOn != null && (neighbor.unitOn.GetTeam() == startSq.unitOn.GetTeam())) continue;
                Debug.Log($"{tile.pathCost} + {neighbor.terrain["cost"]}");
                var newCost = tile.pathCost + neighbor.terrain["cost"];
                if (newCost < neighbor.pathCost) neighbor.pathCost = newCost;
                if (newCost <= unit.stats["mov"]) frontier.Add(neighbor);
            }
        }
        // Reset all path costs before returning
        foreach (var tile in explored) { tile.pathCost = 99; }
        
        return InRangeTiles.Distinct().ToList();
    }

    public List<Square> GetAdjSquares(Square[,] sqArray, Vector2Int pos)
    {
        List<Square> adjSquares = new List<Square>();
        
        // Left and Bottom index check and addition to list
        if (pos.x-1 >= 0) adjSquares.Add(sqArray[pos.x-1 , pos.y]);
        if (pos.y-1 >= 0) adjSquares.Add(sqArray[pos.x, pos.y-1]);
        
        // Right and Up index check and addition to list
        if (pos.x+1 <= sqArray.GetLength(0)) adjSquares.Add(sqArray[pos.x+1, pos.y]);
        if (pos.y+1 <= sqArray.GetLength(1)) adjSquares.Add(sqArray[pos.x, pos.y+1]);
        
        return adjSquares;
    }

    // =============
    // PATH FINDING
    // =============
    public List<Square> FindPath(Square[,] map, Vector2Int start, Vector2Int end)
    {
        Dictionary<Vector2Int, int> frontier = new Dictionary<Vector2Int, int>();
        Dictionary<Vector2Int, int> explored = new Dictionary<Vector2Int, int>();

        frontier.Add(start, 0);

        while (frontier.Any())
        {
            Vector2Int vect = frontier.OrderBy(x => x.Value).First().Key;

            frontier.Remove(vect);
            explored.Add(vect,frontier[vect]);

            if (vect == end) { return GetFinishedList(map, start, end); }

            foreach (var sq in GetAdjSquares(map, vect))
            {
                if (explored.ContainsKey(sq.coords)) continue;

                map[vect.x,vect.y].prev = vect;
            
                if (!frontier.ContainsKey(sq.coords))
                {
                    frontier.Add(sq.coords, GetManhattanDistance(sq.coords, end));
                }
            }
        }
        return null;
    }

    private List<Square> GetFinishedList(Square[,] map, Vector2Int start, Vector2Int end)
    {
        List<Square> finalList = new List<Square>();
        Vector2Int vect = end;
        while (vect != start)
        {
            finalList.Add(map[vect.x,vect.y]);
            vect = map[vect.x,vect.y].prev;
        }
        finalList.Reverse();
        Debug.Log("Done!");
        return finalList;
    }

    private int GetManhattanDistance(Vector2Int posA, Vector2Int posB)
    {
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
    }
}

