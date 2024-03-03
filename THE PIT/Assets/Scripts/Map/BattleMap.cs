using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.Tilemaps;

public class BattleMap : MonoBehaviour
{
    public Square[,] sqArray;
    [SerializeField] Square overSqaurePrefab;
    [SerializeField] GameObject overSquareContainer;

    private static RangeAndPath rangeAndPath = new RangeAndPath();

    private List<Square> rangeCache;

    private void EventSubscription()
    {
        // Unit
        VagueGameEvent.Instance.onUnitChangePosition += UpdateUnitPosition;

        // Cursor
        VagueGameEvent.Instance.onNewUnitClicked += GetUnitRange;
        VagueGameEvent.Instance.onUnitDeselected += Cancel;
        VagueGameEvent.Instance.onUnitMoveRequest += MoveUnit;
    }

    // Get all tiles put in a 2d array according to their position
    private void Start()
    {
        EventSubscription();
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tileMap.cellBounds;
        int deltaX = bounds.max.x - bounds.min.x;
        int deltaY = bounds.max.y - bounds.min.y;

        sqArray = new Square[deltaX,deltaY];
    
        for (int y = 0; y < deltaY; y++){
            for (int x = 0; x < deltaX; x++){

                //Instantiate a square and add it to the battlemap
                var overSquare = Instantiate(overSqaurePrefab, overSquareContainer.transform);
                overSquare.TieToMap(this, new Vector2Int(x,y), tileMap.GetTile(new Vector3Int(x+bounds.min.x,y+bounds.min.y,(int)transform.position.z)));
                sqArray[x,y] = overSquare;


                // Move it to the corresponding spot on the grid
                var tilePosition = tileMap.GetCellCenterWorld(new Vector3Int(x+bounds.min.x,y+bounds.min.y,(int)transform.position.z+1));
                overSquare.transform.position = tilePosition;
            }
        }
    }

    private void MoveUnit(Component sender, object currentPosition, object finalPosition)
    {
        if (sender is not Cursor || currentPosition is not Vector2Int || finalPosition is not Vector2Int) return;

        Vector2Int start = (Vector2Int) currentPosition;
        Vector2Int end = (Vector2Int) finalPosition;

        if (sqArray[end.x,end.y].squareState != Square.SquareState.enabled) return;
        // DEPENDENCY ALERT ???
        sqArray[start.x,start.y].unitOn.StartMove(rangeAndPath.FindPath(sqArray, rangeCache, start, end));
    }

    private void UpdateUnitPosition(Component sender, object data)
    {
        if (sender is not Unit || data is not Vector2Int) return;
        
        Unit unit = (Unit) sender;
        Vector2Int coords = (Vector2Int) data;

        // Quickest way I found online to check if an array index is in bounds.
        if (coords.x < 0 || coords.x >= sqArray.Length || coords.y < 0 || coords.y >= sqArray.Length) return;

        // Move unit and tell the square it has a dude on it, unit's coords
        unit.gameObject.transform.position = sqArray[coords.x,coords.y].transform.position;
        unit.SetCoords(coords);
        sqArray[coords.x,coords.y].unitOn = unit;
    }

    private void Cancel(Component sender)
    {
        if (sender is not Cursor) return;
        HideUnitRange();
    }

    private void HideUnitRange()
    {
        foreach (var sq in rangeCache) sq.HideTile();
    }

    private void GetUnitRange(object data)
    {
        if (data is not Vector2Int) return;

        Vector2Int pos = (Vector2Int) data;

        rangeCache = rangeAndPath.GetMovSquaresInRange(sqArray, pos);
        //List<Square> atkSqs = GetAtkSquaresInRange(pos);
        foreach (var sq in rangeCache) sq.ShowTile();
    }
}