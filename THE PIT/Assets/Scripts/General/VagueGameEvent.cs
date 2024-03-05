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

    // Inventory->Cursor+...
    public event Action<object> onInventoryOpenRequest;
    public void InventoryOpenRequest(object data)
    {
        onInventoryOpenRequest?.Invoke(data);
    }

    public event Action onInventoryCancelRequest;
    public void InventoryCancelRequest()
    {
        onInventoryCancelRequest?.Invoke();
    }

    // ActionMenu->Unit & ActionMenu->Map
    public event Action<Component, object> onWaitButtonPressed;
    public void WaitButtonPress(Component sender, object data)
    {
        onWaitButtonPressed?.Invoke(sender, data);
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
