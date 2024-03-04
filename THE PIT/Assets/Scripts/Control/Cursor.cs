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

    private enum CursorState
    {
        free,
        unitSelected,
        locked,
        unitMoved,
        attackCursor,
        healCursor
    }
    CursorState cursorState = CursorState.free;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void EventSubscription()
    {
        VagueGameEvent.Instance.onUnitDeselected += CleanCursor;
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
                if (sq.GetUnitOn() == null || sq.GetUnitOn().unitState != Unit.UnitState.free) return;
                
                selectedUnit = sq.GetUnitOn();
                VagueGameEvent.Instance.NewUnitClicked(sq.coords);
                VagueGameEvent.Instance.ActionMenuOpenRequest(selectedUnit);
                
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

            case CursorState.attackCursor:
                break;

            case CursorState.healCursor:
                break;

            default:
                break;
        }
        //Debug.Log($"{cursorState}, {selectedUnit}");
    }

    // Right click / X / num2
    private void Cancel(InputAction.CallbackContext context)
    {
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

            case CursorState.unitMoved:
                VagueGameEvent.Instance.UnitChangePosition(selectedUnit, selectedUnit.GetCoords());
                VagueGameEvent.Instance.CancelMove(this, selectedUnit);

                cursorState = CursorState.unitSelected;
                break;

            case CursorState.attackCursor:
                break;

            case CursorState.healCursor:
                break;

            default:
                break;
        }
        //Debug.Log($"{cursorState}, {selectedUnit}");
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
