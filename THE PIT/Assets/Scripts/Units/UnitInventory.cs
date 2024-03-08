using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    [SerializeField] int slots = 5;
    public Item[] itemArray { get; private set; }
    public Unit subjectUnit { get; private set; } 
    public Weapon equippedItem { get; private set;}

    [Header("TestItem")]
    [SerializeField] List<ItemSO> testItems;

    public void Start()
    {
        subjectUnit = GetComponent<Unit>();
        itemArray = new Item[slots];
        TestItemInit();
        AutoEquip();
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

    public void Equip(Item it)
    {
        if (it is not Weapon weapon) { return; }
        equippedItem = weapon;
        // Debug.Log($"Equipped: {weapon.itemName}");
    }

    public void AutoEquip()
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            if (itemArray[i]?.itemType == ItemType.weapon) 
            {
                Equip(itemArray[i]);
                return;
            }
        }
    }

    public void Use(Item it)
    {
        if (it is not Consumable item) { return; }
        subjectUnit.UseCosumable(item);
    }

    private void TestItemInit()
    {
        foreach (ItemSO testItemSO in testItems) 
        { 
            if (testItemSO != null) AddItem(testItemSO); 
        } 
    } 
}
