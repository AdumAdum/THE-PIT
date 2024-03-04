using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("Events")]
    public SpecificGameEvent onUnitCall;

    private void EventSubscription()
    {
        VagueGameEvent.Instance.onActionMenuOpenRequest += UnitOpenActionMenu;
        VagueGameEvent.Instance.onActionMenuCloseRequest += CloseActionMenu;
    }

    void Start()
    {
        EventSubscription();
        canvasGroup = GetComponent<CanvasGroup>();
        DisableCanvasGroup();
    }

    private void UnitOpenActionMenu(object data)
    {
        if (data is not Unit ) return;

        Unit unit = (Unit) data;

        OnUnitCall(unit);
        EnableCanvasGroup();
    }

    private void CloseActionMenu()
    {
        DisableCanvasGroup();
    }

    public void OnUnitCall(Unit unit)
    {
        Debug.Log($"Raising onUnitCall");
        onUnitCall.Raise(this, unit.name);
    }

    private void EnableCanvasGroup()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void DisableCanvasGroup()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
