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

    private enum CursorState
    {
        free,
        unitSelected,
        unitMoved,
        attackCursor,
        healCursor
    }
    CursorState cursorState = CursorState.free;

    void Awake()
    {
        playerControls = new PlayerInputActions();
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

    // Left click / z / num6
    private void Interact(InputAction.CallbackContext context)
    {
        Square sq = GetCurrentSquare();
        if (sq == null) { return; }

        switch (cursorState)
        {
            case CursorState.free:
                if (sq.unitOn == null) return;
                
                selectedUnit = sq.unitOn;
                GameEvents.Instance.NewUnitClicked(this, sq.coords);
                
                cursorState = CursorState.unitSelected;
                break;
            
            case CursorState.unitSelected:
                if (sq.squareState != Square.SquareState.enabled) return;

                GameEvents.Instance.UnitMoveRequest(this, selectedUnit.GetCoords(), sq.coords);
                break;

            case CursorState.unitMoved:
                break;

            case CursorState.attackCursor:
                break;

            case CursorState.healCursor:
                break;

            default:
                break;
        }
        //Debug.Log($"{cursorState}, {selectedUnit}, {sq.terrain["cost"]}");
    }

    // Right click / X / num2
    private void Cancel(InputAction.CallbackContext context)
    {
        switch (cursorState)
        {
            case CursorState.free:
                break;
            
            case CursorState.unitSelected:
                selectedUnit = null;
                GameEvents.Instance.UnitDeselected(this);

                cursorState = CursorState.free;
                break;

            case CursorState.unitMoved:
                // Put unit back where he was
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
