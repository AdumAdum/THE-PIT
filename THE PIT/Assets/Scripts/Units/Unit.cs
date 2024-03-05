using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] Vector2Int coords;
    private List<Square> path;
    private Vector2Int posCache;

    [Header("Battle Info")]
    public UDictionary<string, int> stats;
    public string team { get; private set; }

    private SpriteRenderer spriteRenderer;
    public UnitInventory unitInventory { get; private set; }

    public enum UnitState
    {
        free,
        moving,
        done
    }
    public UnitState unitState = UnitState.free;

    // ============== //
    // INITIALIZATION //
    // ============== //
    private void EventSubscription()
    {
        VagueGameEvent.Instance.onCancelMove += PrematureMoveCancel;
        VagueGameEvent.Instance.onWaitButtonPressed += Wait;
    }

    private void GetComponents()
    {
        unitInventory = GetComponent<UnitInventory>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GetComponents();
        EventSubscription();
        posCache = coords;
        VagueGameEvent.Instance.UnitChangePosition(this, coords);
    }

    // =================== //
    // GETTERS AND SETTERS //
    // =================== //
    public Vector2Int GetCoords(){ return coords; }
    public void SetCoords(Vector2Int newCoords) { coords = newCoords; }

    public string GetTeam()
    {
        return GetComponentInParent<Transform>().name;
    }

    // ======== //
    // MOVEMENT //
    // ======== //
    public void StartMove(List<Square> sqs)
    {
        VagueGameEvent.Instance.ActionMenuCloseRequest();
        path = sqs;
        unitState = UnitState.moving;
    }
    
    private void Update()
    {
        if (unitState == UnitState.moving) MoveAlongPath();
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
            posCache = path[0].coords;
            path.RemoveAt(0);
        }
    }

    private void EndMovement()
    {
        unitState = UnitState.free;
        VagueGameEvent.Instance.ActionMenuOpenRequest(this);
    }

    private void PrematureMoveCancel(Component sender, object data)
    {
        if (sender is not Cursor || data is not Unit) return;
        
        Unit unit = (Unit) data;
        if (!ReferenceEquals(this, unit)) return;

        posCache = unit.coords;
        EndMovement();
    }

    // ======= //
    // ACTIONS //
    // ======= //
    private void Wait(Component sender, object data)
    {
        if (sender is not ActionMenu || data is not Unit) return;

        Unit unit = (Unit) data;
        if (!ReferenceEquals(this, unit)) return;

        UnitDone();
    }

    private void UnitDone()
    {
        unitState = UnitState.done;
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        VagueGameEvent.Instance.UnitChangePosition(this, posCache);
    }

    // ========= //
    // INVENTORY //
    // ========= //
}
