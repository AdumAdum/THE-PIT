using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    [SerializeField] int slots = 5;
    public ItemSO[] itemArray {get; private set;}

    [Header("TestItem")]
    [SerializeField] ItemSO testItemSO;

    public void Start()
    {
        itemArray = new ItemSO[slots];
        AddItem(testItemSO);
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
}
