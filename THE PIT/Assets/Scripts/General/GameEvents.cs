using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private static GameEvents _instance;
    public static GameEvents Instance { get { return _instance; } }

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
    public void UnitChangePosition(Component component, object data) 
    {
        if (onUnitChangePosition != null) onUnitChangePosition(component, data);
    }

    // Cursor->Map
    public event Action<Component, object> onNewUnitClicked;
    public void NewUnitClicked(Component component, object data)
    {
        if (onNewUnitClicked != null) onNewUnitClicked(component, data);
    }

    public event Action<Component> onUnitDeselected;
    public void UnitDeselected(Component component)
    {
        if (onUnitDeselected != null) onUnitDeselected(component);
    }
}
