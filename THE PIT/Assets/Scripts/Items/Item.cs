using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item
{
    // Inventory (from ItemSO) 
    public string itemName; 
    public Sprite sprite; 
    public int uses; 
    
    // Gameplay (from ItemSO)
    public ItemType itemType; 
    public UDictionary<string, int> properties;
    public int[] range;

    public Item(ItemSO itemSO)
    {
        itemName = itemSO.itemName;
        sprite = itemSO.sprite;
        uses = itemSO.uses;
        
        itemType = itemSO.itemType;
        properties = itemSO.properties;
        range = itemSO.range;
    }

    public UDictionary<string, int> GetProperties()
    {
        return properties;
    }
}

