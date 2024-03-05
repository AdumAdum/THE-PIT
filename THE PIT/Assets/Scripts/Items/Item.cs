using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemSO;

    [Header("UI")]
    public Sprite sprite;

    public void InitItem(ItemSO newItem)
    {
        itemSO = newItem; 
        sprite = itemSO.sprite;
    }
}