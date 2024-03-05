using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("Inventory")]
    public string itemName;
    public Sprite sprite;
    public int uses;

    [Header("Data")]
    public ItemType itemType;
    public bool stackable;

    public enum ItemType {
        consumable,
        material,
        weapon
    }
}
