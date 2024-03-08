using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BattleForecast : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] List<Image> pIcons;
    [SerializeField] CUIText pInfo;
    [SerializeField] List<CUIText> pTextFields;
    [SerializeField] List<CUIText> pIntFields;

    [Header("Enemy")]
    [SerializeField] List<Image> eIcons;
    [SerializeField] CUIText eInfo;
    [SerializeField] List<CUIText> eTextFields;
    [SerializeField] List<CUIText> eIntFields;

    private CanvasGroup canvasGroup;
    private static CombatFormulas CF;

    private Unit subjectUnit;
    private Unit targetUnit;

    void GetComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        CF = new CombatFormulas();
    }

    private void EventSubscription()
    {
        VagueGameEvent.Instance.OnOpenBattleForecastRequest += BattleForecastSetup;
        VagueGameEvent.Instance.OnCloseBattleForecastRequest += CloseBattleForecast;
        VagueGameEvent.Instance.onInventoryCloseRequest += CloseBattleForecast;

        VagueGameEvent.Instance.OnCombatRequest += CombatBegin;
    }

    void Start()
    {
        GetComponents();
        EventSubscription();
        DisableCanvasGroup();
    }

    private void CombatBegin()
    {
        VagueGameEvent.Instance.CombatBegin(subjectUnit, targetUnit);
    }

    private void BattleForecastSetup(object subject, object target)
    {
        if (subject is not Unit || target is not Unit) { return; }

        subjectUnit = (Unit) subject;
        targetUnit = (Unit) target;

        VagueGameEvent.Instance.ItemStatsMenuCloseRequest();
        InitBF();
        VagueGameEvent.Instance.ActivateExecuteButton();

        EnableCanvasGroup();
    }


    // Change to accomodate null weapon later
    private void InitBF()
    {
        IconInit(pIcons, subjectUnit);
        IconInit(eIcons, targetUnit);

        pInfo.SetText($"Player - {subjectUnit.unitInventory.equippedItem.itemName}");
        eInfo.SetText($"Enemy - {subjectUnit.unitInventory.equippedItem.itemName}");

        intFields(pIntFields, subjectUnit, targetUnit);
        intFields(eIntFields, targetUnit, subjectUnit);
    }


    private void IconInit(List<Image> icons, Unit unit)
    {
        icons[0].sprite = unit.GetSpriteRenderer().sprite;
        icons[1].sprite = unit.unitInventory.equippedItem.sprite;
    }

    // private void TxtFields(List<CUIText> txts, Unit unit)
    // {
    //     return; // Fine by default, but easy to change here if needed
    // }

    private void intFields(List<CUIText> txts, Unit subject, Unit target)
    {
        string hp = subject.stats["hp"].ToString();
        txts[0].SetText(hp);

        string dmg = CF.DMG(subject, target).ToString();
        txts[1].SetText(dmg);

        string hit = CF.HIT(subject, target).ToString();
        txts[2].SetText(hit);

        string crit = CF.CRIT(subject, target).ToString();
        txts[3].SetText(crit);
    }

    private void CloseBattleForecast()
    {
        VagueGameEvent.Instance.DeactivateExecuteButton();
        DisableCanvasGroup();
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
