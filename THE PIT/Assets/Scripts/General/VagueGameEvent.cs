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

    // ItemStatsMenu->BattleMap
    public event Action onEnterAttackMode;
    public void EnterAttackMode()
    {
        onEnterAttackMode?.Invoke();
    }

    public event Action<object, object> onWeaponDisplayRangeRequest;
    public void WeaponRangeDisplayRequest(object unit, object weapon)
    {
        onWeaponDisplayRangeRequest?.Invoke(unit, weapon);
    }

    // InventoryMenu->ItemStatsMenu
    public event Action<object, object> onItemStatsMenuOpenRequest;
    public void ItemStatsMenuOpenRequest(object unit, object item)
    {
        onItemStatsMenuOpenRequest?.Invoke(unit, item);
    }

    public event Action onItemStatsMenuCloseRequest;
    public void ItemStatsMenuCloseRequest()
    {
        onItemStatsMenuCloseRequest?.Invoke();
    } 

    // InventorySlot -> Inventory
    public event Action<Component, object> onInventorySlotClicked;
    public void InventorySlotClicked(Component sender, object data)
    {
        onInventorySlotClicked?.Invoke(sender, data);
    }

    // ActionMenu->Unit & ActionMenu->Map
    public event Action<Component, object> onUnitEndAction;
    public void UnitEndAction(Component sender, object data)
    {
        onUnitEndAction?.Invoke(sender, data);
    }

    // ActionMenu->Inventory
    public event Action<object, object> onInventoryOpenRequest;
    public void InventoryOpenRequest(object unitInventory, object menuState)
    {
        onInventoryOpenRequest?.Invoke(unitInventory, menuState);
    }

    // Inventory->Cursor+...
    public event Action onInventoryCloseRequest;
    public void InventoryCloseRequest()
    {
        onInventoryCloseRequest?.Invoke();
    }

    // Cursor->Unit
    public event Action<Component, object> onCancelMove;
    public void CancelMove(Component sender, object data)
    {
        onCancelMove?.Invoke(sender, data);
    }

    // Unit->Map
    public event Action<Component, object> onUnitChangePosition;
    public void UnitChangePosition(Component sender, object data) 
    {
        onUnitChangePosition?.Invoke(sender, data);
    }

    // x->ActionMenu
    public event Action<object> onActionMenuOpenRequest;
    public void ActionMenuOpenRequest(object data)
    {
        onActionMenuOpenRequest?.Invoke(data);
    }

    public event Action onActionMenuCloseRequest;
    public void ActionMenuCloseRequest()
    {
        onActionMenuCloseRequest?.Invoke();
    }

    // Cursor->Map
    public event Action<object> onNewUnitClicked;
    public void NewUnitClicked(object data)
    {
        onNewUnitClicked?.Invoke(data);
    }

    public event Action onUnitDeselected; // + ActionMenu -> Cursor
    public void UnitDeselected()
    {
        onUnitDeselected?.Invoke();
    }

    public event Action<Component, object, object, object> onUnitMoveRequest;
    public void UnitMoveRequest(Component sender, object unit, object currentPosition, object finalPosition)
    {
        onUnitMoveRequest?.Invoke(sender, unit, currentPosition, finalPosition);
    }
    

}
