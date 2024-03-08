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
    public UDictionary<string, int> growths;
    public string team { get; private set; }
    
    // Components
    private SpriteRenderer spriteRenderer;
    public UnitInventory unitInventory { get; private set; }
    private CUISlider healthBar;

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
        VagueGameEvent.Instance.OnCancelMove += PrematureMoveCancel;
        VagueGameEvent.Instance.OnUnitEndAction += Wait;
    }

    private void GetComponents()
    {
        unitInventory = GetComponent<UnitInventory>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Canvas canvas = GetComponentInChildren<Canvas>() ?? null;
        healthBar = canvas?.GetComponentInChildren<CUISlider>();
    }

    private void Start()
    {
        GetComponents();
        EventSubscription();
        InitializeStats();
        posCache = coords;
        VagueGameEvent.Instance.UnitChangePosition(this, coords);
    }

    private void InitializeStats()
    {
        stats["hp"] = stats["MAXHP"] - 10;
        UpdateHealthBar();
        // Eventually, save and load stat valuse between maps (scenes)
    }

    // =================== //
    // GETTERS AND SETTERS //
    // =================== //
    public Vector2Int GetCoords(){ return coords; }
    public void SetCoords(Vector2Int newCoords) { coords = newCoords; }

    public Vector2Int GetPosCache(){ return posCache; }
    public void SetPosCache(Vector2Int newPos) { posCache = newPos; }

    public string GetTeam() { return transform.parent.name; }

    public SpriteRenderer GetSpriteRenderer() { return spriteRenderer; }

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
        if (data is not Unit) return;

        Unit unit = (Unit) data;
        if (!ReferenceEquals(this, unit)) return;

        UnitDone();
    }

    public void UseCosumable(Consumable item)
    {
        item.SubUses();
        UnitDone();
        
        UDictionary<string, int> properties = item.GetProperties();
        if (properties == null) { return; }
        foreach (string key in properties.Keys)
        {
            stats[key] = Mathf.Min(stats[key] + properties[key], stats["MAXHP"]);
        }
        UpdateHealthBar();
    }

    private void UnitDone()
    {
        unitState = UnitState.done;
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        VagueGameEvent.Instance.UnitChangePosition(this, posCache);
        VagueGameEvent.Instance.UnitDeselected();
    }

    // ========= //
    // INVENTORY //
    // ========= //



    // ================ //
    // HEALTH AND STATS //
    // ================ //

    public void UpdateHealthBar()
    {
        healthBar?.SetMinValue(0);
        healthBar?.SetMaxValue(stats["MAXHP"]);
        healthBar?.SetValue(stats["hp"]);
    }

}
