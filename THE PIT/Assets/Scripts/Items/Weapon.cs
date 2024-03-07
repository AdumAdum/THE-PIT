using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/WeaponSO",fileName ="WeaponSO")]
public class Weapon : ItemSO
{
    [SerializeField] UDictionary<string, int> stats;

    public UDictionary<string, int> GetStats()
    {
        return stats;
    }
}