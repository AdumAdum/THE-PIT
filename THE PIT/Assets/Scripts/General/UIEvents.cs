using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    private static UIEvents _instance;
    public static UIEvents Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
    }

    // HPController->UI(?)
    public event Action<Component, object> onValueChanged;
    public void ValueChanged(Component sender, object data)
    {
        onValueChanged?.Invoke(sender, data);
    }
}
