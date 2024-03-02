using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Square : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Vector2Int coords { get; private set; }
    public Dictionary<string, int> terrain { get; private set; }

    // See if we can't make a rangefinder that doesn't need this.
    
    public Unit unitOn;
    private BattleMap mapOn;
    private TileBase tileBase;

    public int pathCost = 99;

    // See if we can't make a move function that doesn't need this
    public enum SquareState {
        disabled,
        enabled,
        attack,
        heal,
    }
    public SquareState squareState;

    void Start()
    {
        name = $"{coords.x},{coords.y}";
        spriteRenderer = GetComponent<SpriteRenderer>();
        terrain = new Dictionary<string, int>();

        TerrainSetup();
        HideTile();
    }
    
    public void ShowTile()
    {
        spriteRenderer.color = new Color(0.5f,0.7f,1f,0.9f);
        squareState = SquareState.enabled;
    }

    public void ShowAtkTile()
    {
        spriteRenderer.color = new Color(1f,0f,0f,0.9f);
        squareState = SquareState.attack;
    }

    public void HideTile()
    {
        spriteRenderer.color = new Color(1,1,1,0);
        squareState = SquareState.disabled;
    }

    public void TieToMap(BattleMap map, Vector2Int pos, TileBase tb)
    {
        // I think this dependency is fine, you need a map to have tiles after all.
        mapOn = map;
        coords = pos;
        tileBase = tb;
    }

    private void TerrainSetup()
    {
        //terrain bonus format, key=string value=int ex) (avo,20)
        //Debug.Log($"{tileBase.name}");
        switch (tileBase.name)
        {
            case "Plain":
                terrain.Add("cost", 1);
                break;

            case "Forest":
                terrain.Add("cost", 2);
                terrain.Add("avo", 20);
                break;

            case "Dirt":
                terrain.Add("cost", 1);
                break;

            case "Mud":
                terrain.Add("cost", 2);
                terrain.Add("avo", -20);
                break;

            case "Gold":
                terrain.Add("cost", 1);
                terrain.Add("atk", 1);
                terrain.Add("def", 1);
                break;

            case "Fire":
                terrain.Add("cost", 2);
                terrain.Add("dmg", 5);
                break;

            case "Black":
                terrain.Add("cost", 99);
                break;

            default:
                terrain.Add("cost", 1);
                break;
        }
    }
}
