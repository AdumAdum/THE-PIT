using UnityEngine;

public class CombatFormulas
{
    // General
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

    // XP

    // Weapon
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

    // Combat
}
