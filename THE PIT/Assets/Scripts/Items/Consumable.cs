using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="Item/ConsumableSO",fileName ="ConsumableSO")]
public class Consumable : Item
{
    public Consumable(ItemSO itemSO) : base(itemSO) {}

    public UDictionary<string, int> GetProperties()
    {
        return properties;
    }

    public void SubUses(int val=1)
    {
        uses -= val;
    }
}