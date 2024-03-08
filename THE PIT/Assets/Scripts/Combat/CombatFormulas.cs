using UnityEngine;

public class CombatFormulas
{
    // ======= //
    // GENERAL //
    // ======= //
    public bool RNG(int percent, int chance=100)
    {
        int RNG = Random.Range(1,chance);
        if (RNG <= percent) return true;
        else return false;
    }

    public int GEN_MINMAX(int value)
    {
        return Mathf.Max(0, Mathf.Min(value, 999));
    }

    public int PERCENT_MINMAX(int value)
    {
        return Mathf.Max(0, Mathf.Min(value, 100));
    }



    // ========== //
    // EXPERIENCE //
    // ========== //




    // ====== //
    // WEAPON //
    // =======//
    public int WPN_ATK_PHYS(Unit unit, Weapon weapon)
    {
        return GEN_MINMAX(unit.stats["STR"] + weapon.properties["PWR"]);
    }

    public int WPN_HIT(Unit unit, Weapon weapon)
    {
        return GEN_MINMAX(weapon.properties["ACC"]);
        
    }

    public int WPN_CRT(Unit unit, Weapon weapon)
    {
        return GEN_MINMAX(weapon.properties["CRT"]);
    }

    public int WPN_AVO(Unit unit, Weapon weapon)
    {
        return GEN_MINMAX(weapon.properties["AVO"]);
    }



    // ====== //
    // DAMAGE //
    // ====== //
    public int DMG(Unit attacker, Unit defender)
    {
        Weapon awp = attacker.unitInventory.equippedItem;
        if (awp.properties["MAG"] == 0) { 
            return DMG_PHYS(attacker, awp, defender);
        } else {
            return DMG_MAG(attacker, awp, defender);
        }
    }

    public int DMG_PHYS(Unit attacker, Weapon awp, Unit defender)
    {
        return GEN_MINMAX(attacker.stats["STR"] + awp.properties["PWR"] - defender.stats["DEF"]);
    }

    public int DMG_MAG(Unit attacker, Weapon awp, Unit defender)
    {
        return GEN_MINMAX(attacker.stats["INT"] + awp.properties["PWR"] - defender.stats["RES"]);
    }



    // ============ //
    // HIT/AVO/CRIT //
    // ============ //
    public int HIT(Unit attacker, Unit defender)
    {
        return 100;
    }

    public int AVO(Unit attacker)
    {
        return 0;
    }

    public bool DOES_HIT(Unit attacker, Unit defender)
    {
        return true;
    }

    public int CRIT(Unit attacker, Unit defender)
    {
        return WPN_CRT(attacker, attacker.unitInventory.equippedItem);
    }
    
    public bool DOES_CRIT(Unit attacker, Unit defender)
    {
        return false;
    }
}
