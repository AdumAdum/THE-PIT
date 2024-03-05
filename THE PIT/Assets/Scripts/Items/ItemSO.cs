using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public ItemType itemType;
    public bool stackable;

    public enum ItemType {
        consumable,
        material,
        weapon
    }
}
