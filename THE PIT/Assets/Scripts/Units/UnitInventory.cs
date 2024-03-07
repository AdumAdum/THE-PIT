using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    [SerializeField] int slots = 5;
    public ItemSO[] itemArray { get; private set; }
    public Unit subjectUnit { get; private set; } 

    [Header("TestItem")]
    [SerializeField] Consumable testCons;

    public void Start()
    {
        subjectUnit = GetComponent<Unit>();
        itemArray = new ItemSO[slots];
        TestItemInit();
    }

    public void AddItem(ItemSO itemSO)
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            if (itemArray[i] == null) 
            {
                itemArray[i] = itemSO;
                return;
            }
        }
        Debug.Log($"Inventory Full!");
    }

    public void Use(ItemSO item)
    {
        switch (item.itemType)
        {
            case ItemType.consumable:
            Use((Consumable) item);
            break;

            case ItemType.material:
            //Use((MaterialItem) item);
            break;

            case ItemType.weapon:
            //Use((Weapon) item);
            break;
        }
    }

    public void Use(Consumable item)
    {
        subjectUnit.UseCosumable(item);
    }

    // public void Use(Weapon item)
    // {

    // }
    
    // public void Use(Material item)
    // {

    // }

    private void TestItemInit()
    {
        AddItem(testCons);
    } 
}
