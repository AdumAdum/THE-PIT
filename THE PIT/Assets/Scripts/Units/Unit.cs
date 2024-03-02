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
        stats = new Dictionary<string, int>{ {"mov", 3} };
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
