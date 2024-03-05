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

    // X->Y isn't all events can do. I want to send this to multiple different scripts I can easily. Hooray!

    // InventorySlot -> Inventory+...
    public event Action<Component, object> onItemUsed;
    public void ItemUsed(Component sender, object data)
    {
        onItemUsed?.Invoke(sender, data);
    }

    // ActionMenu->Unit & ActionMenu->Map
    public event Action<Component, object> onUnitEndAction;
    public void UnitEndAction(Component sender, object data)
    {
        onUnitEndAction?.Invoke(sender, data);
    }

    // Inventory->Cursor+...
    public event Action<object> onInventoryOpenRequest;
    public void InventoryOpenRequest(object data)
    {
        onInventoryOpenRequest?.Invoke(data);
    }

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
