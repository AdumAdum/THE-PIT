using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private static EventSystem _instance;
    public static EventSystem Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
    }
/*
    public event Action SampleAction;
    public void SampleActionFunction()
    {
        
    }
*/
}
