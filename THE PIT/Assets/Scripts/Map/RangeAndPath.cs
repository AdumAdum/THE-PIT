using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class RangeAndPath
{
    // ============= //
    // RANGE FINDING //
    // ============= //
    public List<Square> GetMovSquaresInRange(Square[,] map, Vector2Int startVect)
    {
        var frontier = new List<Square>();
        var explored = new List<Square>();
        var inRangeTiles = new List<Square>();

        Square startSq = map[startVect.x,startVect.y];
        startSq.pathCost = 0;
        Unit unit = startSq.GetUnitOn();

        frontier.Add(startSq);
        inRangeTiles.Add(startSq);

        while (frontier.Any())
        {
            var sq = frontier.First();
            frontier.Remove(sq);
            explored.Add(sq);

            if (sq.pathCost <= unit.stats["MOV"])
            {
                inRangeTiles.Add(sq);
            }

            var neighbors = GetAdjSquares(map, sq.coords);
            neighbors = neighbors.Except(explored).ToList();

            foreach (var neighbor in neighbors)
            {
                if (EnemySquare(unit, neighbor)) continue;
                var newCost = sq.pathCost + neighbor.terrain["cost"];
                if (newCost < neighbor.pathCost) neighbor.pathCost = newCost;
                if (newCost <= unit.stats["MOV"]) frontier.Add(neighbor);
            } 
        }
        foreach (var tile in explored) tile.pathCost = 99;

        return inRangeTiles.Distinct().ToList();
    }

    public List<Square> GetAtkSquaresFromTileAndWeaponRange(Square[,] map, Vector2Int startVect, int[] wpnRange)
    {
        // return squares at a distance from int[] range away  with a loop
        var frontier = new List<Square>();
        var explored = new List<Square>();
        var inRangeTiles = new List<Square>();

        Square startSq = map[startVect.x,startVect.y];
        int maxRange = wpnRange.Last();
        startSq.pathCost = 0;

        frontier.Add(startSq);

        while (frontier.Any())
        {
            var sq = frontier.First();
            frontier.Remove(sq);
            explored.Add(sq);

            foreach (int val in wpnRange) { 
                if (sq.pathCost == val) { inRangeTiles.Add(sq); }
            }

            var neighbors = GetAdjSquares(map, sq.coords);
            neighbors = neighbors.Except(explored).ToList();

            foreach (var neighbor in neighbors)
            {
                var newCost = sq.pathCost + 1;
                if (newCost < neighbor.pathCost) neighbor.pathCost = newCost;
                if (newCost <= maxRange) frontier.Add(neighbor);
            } 
        }
        foreach (var tile in explored) tile.pathCost = 99;

        return inRangeTiles.Distinct().ToList();
    }

    public List<Square> GetEnemyUnitSquaresFromRange(List<Square> range)
    {
        List<Square> finishedList = new List<Square>();
        foreach (var sq in range){
            if (sq.GetUnitOn() != null && sq.GetUnitOn().GetTeam() == "EnemyUnits") { 
                finishedList.Add(sq);
            }
        } 
        return finishedList; 
    }

    private bool EnemySquare(Unit unit, Square sq)
    {
        if (sq.GetUnitOn() != null && unit.GetTeam() != sq.GetUnitOn().GetTeam()) return true;
        return false;
    }

    // ============ //
    // PATH FINDING //
    // ============ //
    public List<Square> FindPath(Square[,] map, List<Square> inRangeSqs, Vector2Int start, Vector2Int end)
    {
        var frontier = new List<Square>();
        var explored = new List<Square>();

        frontier.Add(map[start.x,start.y]);

        while (frontier.Any())
        {
            Square sq = frontier.OrderBy(x => x.terrain["cost"] + GetManhattanDistance(x.coords, end)).First();

            frontier.Remove(sq);
            explored.Add(sq);

            if (sq.coords == end) { return GetFinishedList(map, start, end); }

            foreach (var neighbor in GetAdjSquares(map, sq.coords))
            {
                if (explored.Contains(neighbor) || !inRangeSqs.Contains(neighbor)) continue;

                neighbor.prev = sq.coords;
            
                if (!frontier.Contains(neighbor))
                {
                    frontier.Add(neighbor);
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
        return finalList;
    }

    private int GetManhattanDistance(Vector2Int posA, Vector2Int posB)
    {
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
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