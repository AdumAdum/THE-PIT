using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Unit subjectUnit;
    private Unit targetUnit;

    private static CombatFormulas CF;

    void GetComponents()
    {
        CF = new CombatFormulas();
    }

    void EventSubscription()
    {
        VagueGameEvent.Instance.OnCombatBegin += Combat;
    }

    void Start()
    {
        GetComponents();
        EventSubscription();
    }

    // Handle weapon durability, unit hp, everything. Free reign CombatManager, free reign.
    private void Combat(object atk, object def)
    {
        if (atk is not Unit || def is not Unit) return;

        subjectUnit = (Unit) atk;
        targetUnit = (Unit) def;

        Debug.Log("Combat Begin!");
        
        if (Round(subjectUnit, targetUnit)) { 
            EndCombat(); 
            return; 
        }
        
        else if (Round(targetUnit, subjectUnit)) {
            EndCombat(); 
            return;
        }

        // Implement double attacking here
    }

    // returns true if defender died
    private bool Round(Unit attacker, Unit defender)
    {
        if (CF.DOES_HIT(attacker, defender))
        {
            Debug.Log($"{attacker.name} hits {defender.name}");
            
            int dmg = CF.DMG(attacker, defender);
            Debug.Log($"{attacker} deals {dmg} damage!");

            defender.stats["hp"] -= dmg;
            Debug.Log($"{defender} now has {defender.stats["hp"]} health");

            attacker.unitInventory.equippedItem.SubUses();
        }
        else
        {
            Debug.Log($"{attacker} missed...");
        }
        if ( defender.stats["hp"] <= 0 ) return true;
        else return true;
    }

    private void EndCombat()
    {
        subjectUnit.UpdateHealthBar();
        targetUnit.UpdateHealthBar();
        Debug.Log("Combat over");
    }
}
