using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemStatsMenu : MonoBehaviour
{
    private Item item;
    private Unit subjectUnit;
    
    [SerializeField] Image itemIcon;
    [SerializeField] CUIText itemName;
    [SerializeField] List<CUIText> textFields;
    [SerializeField] List<CUIText> intFields;
    
    private CanvasGroup canvasGroup;
    private static CombatFormulas CF;

    void GetComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        CF = new CombatFormulas();
    }

    private void EventSubscription()
    {
        VagueGameEvent.Instance.onItemStatsMenuOpenRequest += ItemStatsMenuSetup;
        VagueGameEvent.Instance.onItemStatsMenuCloseRequest += DisableCanvasGroup;
        VagueGameEvent.Instance.onInventoryCloseRequest += DisableCanvasGroup;
    }

    void Start()
    {
        GetComponents();
        EventSubscription();
        DisableCanvasGroup();
    }

    private void ItemStatsMenuSetup(object un, object it)
    {  
        if (un is not Unit || it is not Item ) return;

        subjectUnit = (Unit) un;
        item = (Item) it;

        if (item.itemType != ItemType.weapon) return; //Only works for weapons right now

        EnableCanvasGroup();
        DisplayItemStats();
    }

    private void DisplayItemStats()
    {
        switch (item.itemType)
        {
            case ItemType.consumable:
            return;

            case ItemType.material:
            return;

            case ItemType.weapon:
            DisplayWeaponInfo();
            break;
        }
    }

    private void DisplayWeaponInfo()
    {
        itemIcon.sprite = item.sprite;
        itemName.SetText(item.itemName);

        textFields[0].SetText("Atk:");
        textFields[1].SetText("Hit:");
        textFields[2].SetText("Crit:");
        textFields[3].SetText("Avo:");

        Weapon weapon = (Weapon)item;
        
        string atk = CF.WPN_ATK_PHYS(subjectUnit, weapon).ToString();
        intFields[0].SetText($"{atk}");
        
        string hit = CF.WPN_HIT(subjectUnit, weapon).ToString();
        intFields[1].SetText($"{hit}");
    
        string crt = CF.WPN_CRT(subjectUnit, weapon).ToString();
        intFields[2].SetText($"{crt}");

        string avo = CF.WPN_AVO(subjectUnit, weapon).ToString();
        intFields[3].SetText($"{avo}");

        VagueGameEvent.Instance.WeaponRangeDisplayRequest(subjectUnit, weapon);
    }

    private void EnableCanvasGroup()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void DisableCanvasGroup()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
