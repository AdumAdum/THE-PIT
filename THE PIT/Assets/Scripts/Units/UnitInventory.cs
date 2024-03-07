using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    [SerializeField] int slots = 5;
    public Item[] itemArray { get; private set; }
    public Unit subjectUnit { get; private set; } 

    [Header("TestItem")]
    [SerializeField] List<ItemSO> testItems;

    public void Start()
    {
        subjectUnit = GetComponent<Unit>();
        itemArray = new Item[slots];
        TestItemInit();
    }

    public void AddItem(ItemSO itemSO)
    {
        Item item = ConvertToItem(itemSO);
        for (int i = 0; i < itemArray.Length; i++)
        {
            if (itemArray[i] == null) 
            {
                itemArray[i] = item;
                return;
            }
        }
        Debug.Log($"Inventory Full!");
    }

    Item ConvertToItem(ItemSO itemSO)
    {
        switch (itemSO.itemType)
        {
            case ItemType.consumable:
            return new Consumable(itemSO);

            case ItemType.material:
            //Use((MaterialItem) item);
            break;

            case ItemType.weapon:
            return new Weapon(itemSO);
        }
        return null;
    }

    public void Use(Item item)
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
        foreach (ItemSO testItemSO in testItems) 
        { 
            if (testItemSO != null) AddItem(testItemSO); 
        } 
    } 
}
