using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item
{
    // Inventory (from ItemSO) 
    public string itemName; //{ get; private set; }
    public Sprite sprite; //{ get; private set; }
    public int uses; //{ get; private set; }
    
    // Gameplay (from ItemSO)
    public ItemType itemType; //{ get; private set; }
    public UDictionary<string, int> properties;

    public Item(ItemSO itemSO)
    {
        itemName = itemSO.itemName;
        sprite = itemSO.sprite;
        uses = itemSO.uses;
        itemType = itemSO.itemType;
        properties = itemSO.properties;
    }
}

