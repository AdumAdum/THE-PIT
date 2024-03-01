using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] Vector2Int coords;
    public Dictionary<string, int> stats;

    [Header("Events")]
    public GameEvent UnitPositionUpdate;

    void Start()
    {
        PositionUpdate(coords);
    }

    void PositionUpdate(Vector2Int position)
    {
        UnitPositionUpdate.Raise(this, position);
        coords = position;
    }
}
