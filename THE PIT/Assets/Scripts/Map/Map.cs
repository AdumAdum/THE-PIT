using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Square[,] BattleMap;
    public Square overSqaurePrefab;
    public GameObject overSquareContainer;

    // Get all tiles put in a 2d array according to their position
    void Start()
    {
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        BoundsInt bounds = tileMap.cellBounds;
        int deltaX = bounds.max.x - bounds.min.x;
        int deltaY = bounds.max.y - bounds.min.y;

        BattleMap = new Square[deltaX,deltaY];
    
        for (int y = 0; y < deltaY; y++){
            for (int x = 0; x < deltaX; x++){

                //Instantiate a square and add it to the battlemap
                var overSquare = Instantiate(overSqaurePrefab, overSquareContainer.transform);
                overSquare.coords = new Vector2Int(x,y);
                BattleMap[x,y] = overSquare;

                // Move it to the corresponding spot on the grid
                var tilePosition = tileMap.GetCellCenterWorld(new Vector3Int(x+bounds.min.x,y+bounds.min.y,(int)transform.position.z+1));
                overSquare.transform.position = tilePosition;
            }
        }
    }
}

/*
private int GetManhattanDistance(Square sqA, Square sqB)
{
    return Mathf.Abs(sqA.gridLocation.x - sqB.gridLocation.x) + Mathf.Abs(sqA.gridLocation.y - sqB.gridLocation.y);
}
*/