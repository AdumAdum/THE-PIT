using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static GameSettings _instance;
    public static GameSettings Instance { get { return _instance; } }

    public float gameSpd = 10f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
    }
}
