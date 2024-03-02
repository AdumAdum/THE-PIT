using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [SerializeField] Vector2Int coords;
    public UDictionary<string, int> stats;
    public string team { get; private set; }

    // Movement
    private bool moving = false;
    private List<Square> path;

    private void Start()
    {
        GameEvents.Instance.UnitChangePosition(this, coords);
    }

    public Vector2Int GetCoords(){ return coords; }
    public void SetCoords(Vector2Int newCoords) { coords = newCoords; }

    public string GetTeam()
    {
        return GetComponentInParent<GameObject>().name;
    }

    private void Update()
    {
        if (moving) MoveAlongPath();
    }

    public void StartMove(List<Square> sqs)
    {
        path = sqs;
        moving = true;
    }

    private void MoveAlongPath()
    {
        if (!path.Any()) 
        { 
            EndPathFinding();
            return; 
        }

        float step = 10f * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.001f)
        {
            GameEvents.Instance.UnitChangePosition(this, path[0].coords);
            path.RemoveAt(0);
        }
    }

    private void EndPathFinding()
    {
        moving = false;
        //GameEvents.Instance.UnitChangePosition(this, coords);
    }
}
