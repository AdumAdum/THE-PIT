using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public Vector2Int coords;
    public Dictionary<string, int> terrainBonuses;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        name = $"{coords.x},{coords.y}";
        spriteRenderer = GetComponent<SpriteRenderer>();

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
}
