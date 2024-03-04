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
    public bool moving { get; private set;}
    private List<Square> path;

    private void EventSubscription()
    {
        VagueGameEvent.Instance.onCancelMove += PrematureMoveCancel;
    }

    private void Start()
    {
        EventSubscription();
        VagueGameEvent.Instance.UnitChangePosition(this, coords);
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
        VagueGameEvent.Instance.ActionMenuCloseRequest();
        path = sqs;
        moving = true;
    }

    private void MoveAlongPath()
    {
        if (!path.Any()) 
        { 
            EndMovement();
            return; 
        }

        float step = 10f * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.001f)
        {
            path.RemoveAt(0);
        }
    }

    private void EndMovement()
    {
        moving = false;
        VagueGameEvent.Instance.ActionMenuOpenRequest(this);
    }

    private void PrematureMoveCancel(Component sender, object data)
    {
        if (sender is not Cursor || data is not Unit) return;
        
        Unit unit = (Unit) data;
        if (!ReferenceEquals(this, unit)) return;

        EndMovement();
    }
}
