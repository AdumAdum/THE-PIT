using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/ConsumableSO",fileName ="ConsumableSO")]
public class Consumable : ItemSO
{
    [SerializeField] UDictionary<string, int> properties;

    public UDictionary<string, int> GetProperties()
    {
        return properties;
    }
}