using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class RangeAndPath
{
    // Get mov squares in range
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

            if (tile.pathCost <= unit.stats["mov"])
            {
                InRangeTiles.Add(tile);
            }

            // get neighbor tiles and remove explored
            var neighbors = GetAdjSquares(sqArray, tile.coords);
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

    public int GetManhattanDistance(Square sqA, Square sqB)
    {
        return Mathf.Abs(sqA.coords.x - sqB.coords.x) + Mathf.Abs(sqA.coords.y - sqB.coords.y);
    }

}

