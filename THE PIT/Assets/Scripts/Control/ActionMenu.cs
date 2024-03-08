using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private Unit selectedUnit;
    //private Unit targetUnit;

    [SerializeField] CUIButton ExecuteButton;

    [Header("Events")]
    public SpecificGameEvent onUnitCall;

    private void EventSubscription()
    {
        VagueGameEvent.Instance.OnActionMenuOpenRequest += UnitOpenActionMenu;
        VagueGameEvent.Instance.OnActionMenuCloseRequest += CloseActionMenu;
        
        VagueGameEvent.Instance.OnActivateExecuteButton += EnableExecuteButton;
        VagueGameEvent.Instance.OnDeactivateExecuteButton += DisableExecuteButton;
    }

    void Start()
    {
        EventSubscription();
        canvasGroup = GetComponent<CanvasGroup>();
        DisableCanvasGroup();
    }

    public void WaitButtonPress()
    {
        EventCloseAllMenus();
        VagueGameEvent.Instance.UnitEndAction(this, selectedUnit);
        VagueGameEvent.Instance.UnitDeselected();
    }

    public void ItemButtonPress()
    {
        VagueGameEvent.Instance.InventoryOpenRequest(selectedUnit.unitInventory, IMState.IMItem);
    }

    public void AttackButtonPress()
    {
        VagueGameEvent.Instance.InventoryOpenRequest(selectedUnit.unitInventory, IMState.IMAttack);
    }

    public void ExecuteButtonPress()
    {
        EventCloseAllMenus();
        VagueGameEvent.Instance.CombatRequest();
        VagueGameEvent.Instance.UnitEndAction(this, selectedUnit);
        VagueGameEvent.Instance.UnitDeselected();
    }

    private void EventCloseAllMenus()
    {
        VagueGameEvent.Instance.ActionMenuCloseRequest();
        VagueGameEvent.Instance.InventoryCloseRequest();
        VagueGameEvent.Instance.CloseBattleForecastRequest();
        VagueGameEvent.Instance.ItemStatsMenuCloseRequest();
    }

    private void UnitOpenActionMenu(object data)
    {
        if (data is not Unit ) return;

        Unit unit = (Unit) data;

        OnUnitCall(unit);
        EnableCanvasGroup();
    }

    private void CloseActionMenu()
    {
        DisableCanvasGroup();
    }

    public void OnUnitCall(Unit unit)
    {
        selectedUnit = unit;
        onUnitCall.Raise(this, selectedUnit);
    }

    private void EnableExecuteButton()
    {
        CanvasGroup cg = ExecuteButton.transform.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    private void DisableExecuteButton()
    {
        CanvasGroup cg = ExecuteButton.transform.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
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
