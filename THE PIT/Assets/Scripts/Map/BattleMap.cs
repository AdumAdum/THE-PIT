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

    private static RangeAndPath RP = new RangeAndPath();

    private List<Square> movRangeCache;
    private List<Square> atkRangeCache;

    private void EventSubscription()
    {
        // Cursor
        VagueGameEvent.Instance.OnNewUnitClicked += GetUnitRange;
        VagueGameEvent.Instance.OnUnitDeselected += HideRangeCache;
        VagueGameEvent.Instance.OnUnitMoveRequest += MoveUnit;

        // Unit
        VagueGameEvent.Instance.OnUnitChangePosition += UpdateUnitPosition;

        //Inventory
        VagueGameEvent.Instance.onInventoryCloseRequest += ShowRangeCache;
        VagueGameEvent.Instance.OnInventoryOpenRequest += HideRangeCache;
  
        //ItemStats
        VagueGameEvent.Instance.OnWeaponDisplayRangeRequest += ShowAtkFromTile;
        VagueGameEvent.Instance.OnEnterPreAttackMode += ShowOnlyEnemySquares;
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

    private void MoveUnit(Component sender, object unit, object currentPosition, object finalPosition)
    {
        if (sender is not Cursor || currentPosition is not Vector2Int || finalPosition is not Vector2Int) return;

        Unit unitToMove = (Unit) unit;
        Vector2Int start = (Vector2Int) currentPosition;
        Vector2Int end = (Vector2Int) finalPosition;

        unitToMove.StartMove(RP.FindPath(sqArray, movRangeCache, start, end));
    }

    private void UpdateUnitPosition(Component sender, object data)
    {
        if (sender is not Unit || data is not Vector2Int) return;
        
        Unit unit = (Unit) sender;
        Vector2Int coords = (Vector2Int) data;

        // Quickest way I found online to check if an array index is in bounds.
        if (coords.x < 0 || coords.x >= sqArray.Length || coords.y < 0 || coords.y >= sqArray.Length) return;

        // Clean up previous square
        sqArray[unit.GetCoords().x, unit.GetCoords().y].SetUnitOn(null);

        // Move unit and tell the square it has a dude on it, unit's coords
        unit.gameObject.transform.position = sqArray[coords.x,coords.y].transform.position;
        unit.SetCoords(coords);
        sqArray[coords.x,coords.y].SetUnitOn(unit);
    }

    private void GetUnitRange(object data)
    {
        if (data is not Vector2Int) return;

        Vector2Int pos = (Vector2Int) data;

        movRangeCache = RP.GetMovSquaresInRange(sqArray, pos);
        //List<Square> atkSqs = GetAtkSquaresInRange(pos);
        ShowUnitRange(movRangeCache);
    }

    // Misc Event-Based Hide/show functions, could reformat but this works fine
    private void ShowOnlyEnemySquares()
    {
        List<Square> sqs = RP.GetEnemyUnitSquaresFromRange(atkRangeCache);
        HideUnitRange(atkRangeCache);
        ShowAtkRange(sqs);
    }

    private void ShowAtkFromTile(object un, object wp)
    {
        if (un is not Unit unit || wp is not Weapon weapon) return;

        atkRangeCache = RP.GetAtkSquaresFromTileAndWeaponRange(sqArray, unit.GetPosCache(), weapon.range);
        ShowAtkRange(atkRangeCache);
    }
    
    private void ShowRangeCache()
    {
        HideUnitRange(atkRangeCache);
        ShowUnitRange(movRangeCache);
    }

    private void HideRangeCache(object who, object cares)
    {
        HideUnitRange(atkRangeCache);
        HideUnitRange(movRangeCache);
    }

    private void HideRangeCache()
    {
        HideUnitRange(atkRangeCache);
        HideUnitRange(movRangeCache);
    }

    private void ShowAtkRange(List<Square> range)
    {
        if (range == null) { return; }
        foreach (var sq in range) sq.ShowAtkTile();
    }

    private void ShowUnitRange(List<Square> range) {
        if (range == null) { return; }
        foreach (var sq in range) sq.ShowTile();
    }

    private void HideUnitRange(List<Square> range) {
        if (range == null) { return; }
        foreach (var sq in range) sq.HideTile();
    }
}