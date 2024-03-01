using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Square : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Vector2Int coords;
    public Dictionary<string, int> terrainBonuses;
    
    public Unit unitOn;
    private Map mapOn;
    private TileBase tileBase;

    void Start()
    {
        name = $"{coords.x},{coords.y}";
        spriteRenderer = GetComponent<SpriteRenderer>();
        terrainBonuses = new Dictionary<string, int>();

        TerrainSetup();
        HideTile();
    }
    
    public void ShowTile()
    {
        spriteRenderer.color = new Color(1,1,1,1f);
    }

    public void ShowAtkTile()
    {
        spriteRenderer.color = new Color(1f,0f,0f,1f);
    }

    public void HideTile()
    {
        spriteRenderer.color = new Color(1,1,1,0);
    }

    public void TieToMap(Map map, Vector2Int pos, TileBase tb)
    {
        // I think this dependency is fine, you need a map to have tiles after all.
        mapOn = map;
        coords = pos;
        tileBase = tb;
    }

    private void TerrainSetup()
    {
        //terrain bonus format, key=string value=int ex) (avo,20)
        Debug.Log($"{tileBase.name}");
        switch (tileBase.name)
        {
            case "Plain":
                terrainBonuses.Add("cost", 1);
                break;

            case "Forest":
                terrainBonuses.Add("cost", 2);
                terrainBonuses.Add("avo", 20);
                break;

            case "Dirt":
                terrainBonuses.Add("cost", 1);
                break;

            case "Mud":
                terrainBonuses.Add("cost", 2);
                terrainBonuses.Add("avo", -20);
                break;

            case "Gold":
                terrainBonuses.Add("cost", 1);
                terrainBonuses.Add("atk", 1);
                terrainBonuses.Add("def", 1);
                break;

            case "Fire":
                terrainBonuses.Add("cost", 2);
                terrainBonuses.Add("dmg", 5);
                break;

            case "Black":
                terrainBonuses.Add("cost", 99);
                break;

            default:
                terrainBonuses.Add("cost", 1);
                break;
        }
    }
}
