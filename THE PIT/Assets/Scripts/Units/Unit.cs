using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] Vector2Int coords;
    public UDictionary<string, int> stats;
    public string team;

    private void Start()
    {
        GameEvents.Instance.UnitChangePosition(this, coords);
    }

    public string GetTeam()
    {
        return GetComponentInParent<Transform>().name;
    }

    public Vector2Int GetCoords()
    {
        return coords;
    }
}
