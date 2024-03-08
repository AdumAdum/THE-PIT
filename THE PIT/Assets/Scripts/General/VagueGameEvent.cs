using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagueGameEvent : MonoBehaviour
{
    private static VagueGameEvent _instance;
    public static VagueGameEvent Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
    }

    // X->Y isn't all events can do. If I want to send this to multiple different scripts I can easily. Hooray!

    // BattleForecast->CombatManager
    public event Action<object, object> OnCombatBegin;
    public void CombatBegin(object subject, object target)
    {
        OnCombatBegin?.Invoke(subject, target);
    }

    // ActionMenu->BattleForecast
    public event Action OnCombatRequest;
    public void CombatRequest()
    {
        OnCombatRequest?.Invoke();
    }

    // BattleForecast->ActionMenu
    public event Action OnActivateExecuteButton;
    public void ActivateExecuteButton()
    {
        OnActivateExecuteButton?.Invoke();
    }

    public event Action OnDeactivateExecuteButton;
    public void DeactivateExecuteButton()
    {
        OnDeactivateExecuteButton?.Invoke();
    }

    // x->BattleForecast
    public event Action<object, object> OnOpenBattleForecastRequest;
    public void OpenBattleForecastRequest(object subject, object target)
    {
        OnOpenBattleForecastRequest?.Invoke(subject, target);
    }

    public event Action OnCloseBattleForecastRequest;
    public void CloseBattleForecastRequest()
    {
        OnCloseBattleForecastRequest?.Invoke();
    }


    // ItemStatsMenu->BattleMap
    public event Action OnEnterPreAttackMode;
    public void EnterPreAttackMode()
    {
        OnEnterPreAttackMode?.Invoke();
    }

    public event Action<object, object> OnWeaponDisplayRangeRequest;
    public void WeaponRangeDisplayRequest(object unit, object weapon)
    {
        OnWeaponDisplayRangeRequest?.Invoke(unit, weapon);
    }

    // InventoryMenu->ItemStatsMenu
    public event Action<object, object> OnItemStatsMenuOpenRequest;
    public void ItemStatsMenuOpenRequest(object unit, object item)
    {
        OnItemStatsMenuOpenRequest?.Invoke(unit, item);
    }

    public event Action OnItemStatsMenuCloseRequest;
    public void ItemStatsMenuCloseRequest()
    {
        OnItemStatsMenuCloseRequest?.Invoke();
    } 

    // InventorySlot -> Inventory
    public event Action<Component, object> OnInventorySlotClicked;
    public void InventorySlotClicked(Component sender, object data)
    {
        OnInventorySlotClicked?.Invoke(sender, data);
    }

    // ActionMenu->Unit & ActionMenu->Map
    public event Action<Component, object> OnUnitEndAction;
    public void UnitEndAction(Component sender, object data)
    {
        OnUnitEndAction?.Invoke(sender, data);
    }

    // ActionMenu->Inventory
    public event Action<object, object> OnInventoryOpenRequest;
    public void InventoryOpenRequest(object unitInventory, object menuState)
    {
        OnInventoryOpenRequest?.Invoke(unitInventory, menuState);
    }

    // Inventory->Cursor+...
    public event Action onInventoryCloseRequest;
    public void InventoryCloseRequest()
    {
        onInventoryCloseRequest?.Invoke();
    }

    // Cursor->Unit
    public event Action<Component, object> OnCancelMove;
    public void CancelMove(Component sender, object data)
    {
        OnCancelMove?.Invoke(sender, data);
    }

    // Unit->Map
    public event Action<Component, object> OnUnitChangePosition;
    public void UnitChangePosition(Component sender, object data) 
    {
        OnUnitChangePosition?.Invoke(sender, data);
    }

    // x->ActionMenu
    public event Action<object> OnActionMenuOpenRequest;
    public void ActionMenuOpenRequest(object data)
    {
        OnActionMenuOpenRequest?.Invoke(data);
    }

    public event Action OnActionMenuCloseRequest;
    public void ActionMenuCloseRequest()
    {
        OnActionMenuCloseRequest?.Invoke();
    }

    // Cursor->Map
    public event Action<object> OnNewUnitClicked;
    public void NewUnitClicked(object data)
    {
        OnNewUnitClicked?.Invoke(data);
    }

    public event Action OnUnitDeselected; // + ActionMenu -> Cursor
    public void UnitDeselected()
    {
        OnUnitDeselected?.Invoke();
    }

    public event Action<Component, object, object, object> OnUnitMoveRequest;
    public void UnitMoveRequest(Component sender, object unit, object currentPosition, object finalPosition)
    {
        OnUnitMoveRequest?.Invoke(sender, unit, currentPosition, finalPosition);
    }
}
