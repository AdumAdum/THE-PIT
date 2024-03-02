using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class RangeAndPath
{
    // =============
    // RANGE FINDING
    // =============
    public List<Square> GetMovSquaresInRange(Square[,] map, Vector2Int startVect)
    {
        var frontier = new List<Square>();
        var explored = new List<Square>();
        var inRangeTiles = new List<Square>();

        Square startSq = map[startVect.x,startVect.y];
        startSq.pathCost = 0;
        Unit unit = startSq.unitOn;

        frontier.Add(startSq);
        inRangeTiles.Add(startSq);

        while (frontier.Any())
        {
            var sq = frontier.First();
            frontier.Remove(sq);
            explored.Add(sq);

            if (sq.pathCost <= unit.stats["mov"])
            {
                inRangeTiles.Add(sq);
            }

            var neighbors = GetAdjSquares(map, sq.coords);
            neighbors = neighbors.Except(explored).ToList();

            foreach (var neighbor in neighbors)
            {
                var newCost = sq.pathCost + neighbor.terrain["cost"];
                if (newCost < neighbor.pathCost) neighbor.pathCost = newCost;
                if (newCost <= unit.stats["mov"]) frontier.Add(neighbor);
            } 
        }
        foreach (var tile in explored) tile.pathCost = 99;

        return inRangeTiles.Distinct().ToList();
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

