using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventory : MonoBehaviour
{
    [SerializeField] int slots = 5;
    public ItemSO[] unitInventory {get; private set;}

    public void Start()
    {
        unitInventory = new ItemSO[slots];
    }

    public void AddItem(ItemSO itemSO)
    {
        for (int i = 0; i < unitInventory.Length; i++)
        {
            if (unitInventory[i] == null) 
            {
                unitInventory[i] = itemSO;
                break;
            }
            // Run inventory full please toss item method
        }
    } 
}
