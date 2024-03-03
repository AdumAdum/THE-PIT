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

    // Unit->Map
    public event Action<Component, object> onUnitChangePosition;
    public void UnitChangePosition(Component sender, object data) 
    {
        onUnitChangePosition?.Invoke(sender, data);
    }

    // Cursor->Map
    public event Action<object> onNewUnitClicked;
    public void NewUnitClicked(object data)
    {
        onNewUnitClicked?.Invoke(data);
    }

    public event Action<Component> onUnitDeselected;
    public void UnitDeselected(Component sender)
    {
        onUnitDeselected?.Invoke(sender);
    }

    public event Action<Component, object, object> onUnitMoveRequest;
    public void UnitMoveRequest(Component sender, object currentPosition, object finalPosition)
    {
        onUnitMoveRequest?.Invoke(sender, currentPosition, finalPosition);
    }
}
