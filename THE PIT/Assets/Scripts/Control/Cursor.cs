using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction interact;
    private InputAction cancel;

    private Square selectedSquare;
    private Unit selectedUnit;
    private Unit targetedUnit;
    private Vector2Int fluxPos;

    public enum CursorState
    {
        free,
        unitSelected,
        locked,
        unitMoved,
        inMenu,
        attackCursor,
        healCursor
    }
    private CursorState cursorState = CursorState.free;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void EventSubscription()
    {
        VagueGameEvent.Instance.OnUnitDeselected += CleanCursor;
        VagueGameEvent.Instance.OnInventoryOpenRequest += InventoryOpened;
        VagueGameEvent.Instance.onInventoryCloseRequest += InventoryCancel; 
        VagueGameEvent.Instance.OnEnterPreAttackMode += EnterPreAttackMode;
    }

    void Start()
    {
        EventSubscription();
    }

    void Update()
    {
        CenterCursor();
    }

    void CenterCursor()
    {
        Square sq = GetCurrentSquare();
        if (sq is not null) { transform.position = sq.gameObject.transform.position; }
    }

    void CleanCursor()
    {
        cursorState = CursorState.free;
        selectedUnit = null;
        fluxPos = new Vector2Int();
    }

    void InventoryOpened(object who, object cares)
    {
        cursorState = CursorState.inMenu;
    }

    void InventoryCancel()
    {
        cursorState = CursorState.unitMoved;
    }

    void EnterPreAttackMode()
    {
        cursorState = CursorState.inMenu;
    }

    // Left click / z / num6
    private void Interact(InputAction.CallbackContext context)
    {
        Square sq = GetCurrentSquare();
        if (sq == null) { return; }
        
        bool locked = ShouldLock();
        if (locked) { cursorState = CursorState.locked; }

        retry: 
        switch (cursorState)
        {
            case CursorState.free:
                if (!ValidUnitClick(sq)) return;
                
                selectedUnit = sq.GetUnitOn();
                VagueGameEvent.Instance.NewUnitClicked(sq.coords);
                VagueGameEvent.Instance.ActionMenuOpenRequest(selectedUnit);
                fluxPos = sq.coords;
                
                cursorState = CursorState.unitSelected;
                break;
            
            case CursorState.unitSelected:
                if (!ValidMovTarget(sq)) return;
                VagueGameEvent.Instance.UnitMoveRequest(this, selectedUnit, selectedUnit.GetCoords(), sq.coords);
                fluxPos = sq.coords;

                cursorState = CursorState.unitMoved;
                break;

            case CursorState.locked:
                // Unique. Can exhibit different behavior depending on if you were just locked or are being unlocked
                if (!locked) 
                {
                    cursorState = CursorState.unitMoved;
                    VagueGameEvent.Instance.ActionMenuOpenRequest(selectedUnit);
                    goto retry;
                }
                break;

            case CursorState.unitMoved:
                if (!ValidMovTarget(sq)) return;
                VagueGameEvent.Instance.UnitMoveRequest(this, selectedUnit, fluxPos, sq.coords);
                fluxPos = sq.coords;

                break;

            case CursorState.inMenu:
                if (!ValidAtkTarget(sq)) return;
                VagueGameEvent.Instance.OpenBattleForecastRequest(selectedUnit, sq.GetUnitOn());

                cursorState = CursorState.attackCursor;
                break;

            case CursorState.attackCursor:
                if (!ValidAtkTarget(sq)) return;
                VagueGameEvent.Instance.OpenBattleForecastRequest(selectedUnit, sq.GetUnitOn());
                break;

            case CursorState.healCursor:
                break;

            default:
                break;
        }
        
    }

    // Right click / X / num2
    private void Cancel(InputAction.CallbackContext context)
    {
        // Debug.Log($"StartState: {cursorState}");
        switch (cursorState)
        {
            case CursorState.free:
                break;
            
            case CursorState.unitSelected:
                CleanCursor();
                VagueGameEvent.Instance.UnitDeselected();
                VagueGameEvent.Instance.ActionMenuCloseRequest();

                cursorState = CursorState.free;
                break;
            
            case CursorState.locked:
                break;

            case CursorState.unitMoved:
                VagueGameEvent.Instance.UnitChangePosition(selectedUnit, selectedUnit.GetCoords());
                VagueGameEvent.Instance.CancelMove(this, selectedUnit);

                cursorState = CursorState.unitSelected;
                break;

            case CursorState.inMenu:
                VagueGameEvent.Instance.InventoryCloseRequest();
                //VagueGameEvent.Instance.
                if (selectedUnit.GetCoords() != fluxPos) cursorState = CursorState.unitMoved;
                else cursorState = CursorState.unitSelected;
                
                break;

            case CursorState.attackCursor:
                VagueGameEvent.Instance.CloseBattleForecastRequest();
                cursorState = CursorState.inMenu;
                break;

            case CursorState.healCursor:
                break;

            default:
                break;
        }
        // Debug.Log($"EndState: {cursorState}");
    }

    private bool ShouldLock()
    {
        if (selectedUnit == null) return false;
        else if (selectedUnit.unitState == Unit.UnitState.moving) return true;
        else return false;
    }

    private bool ValidMovTarget(Square targetSq)
    {
        if (targetSq.GetUnitOn() == selectedUnit) return true;
        if (targetSq.GetUnitOn() != null) return false;
        else if (targetSq.squareState != Square.SquareState.enabled) return false;
        else return true;
    }

    private bool ValidUnitClick(Square targetSq)
    {
        Unit unit = targetSq.GetUnitOn();
        if (unit == null) { return false; } 
        if (unit.unitState != Unit.UnitState.free) { return false; }
        if (unit.GetTeam() != "PlayerUnits") { return false; }
        return true;
    }

    private bool ValidAtkTarget(Square targetSq)
    {
        if (targetSq.squareState != Square.SquareState.attack) { return false; }
        if (targetSq.GetUnitOn() == null) { return false; }
        if (targetSq.GetUnitOn().GetTeam() != "EnemyUnits") { return false; }
        return true;
    }

    Square GetCurrentSquare()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if (hits.Length <= 0) { return null; }
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.TryGetComponent(out Square sq))
            {
                return sq;
            }
        }
        return null;
    }

    void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
        
        cancel = playerControls.Player.Cancel;
        cancel.Enable();
        cancel.performed += Cancel;

    }

    void OnDisable()
    {
        playerControls.Disable();
        move.Disable();
        interact.Disable();
        cancel.Disable();
    }
}
