using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    consumable,
    material,
    weapon
}

[CreateAssetMenu(menuName ="Item/ItemSO",fileName ="ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("Inventory")]
    public string itemName;
    public Sprite sprite;
    public int uses;

    [Header("Gameplay")]
    public ItemType itemType;
    public UDictionary<string, int> properties;

    [Header("Weapon Only")]
    public int[] range;
}
