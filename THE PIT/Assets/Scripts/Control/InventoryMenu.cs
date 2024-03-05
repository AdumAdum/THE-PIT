using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private Unit subjectUnit;

    private void EventSubscription()
    {
        VagueGameEvent.Instance.onInventoryOpenRequest += EnableCanvasGroup;
        VagueGameEvent.Instance.onInventoryCancelRequest += DisableCanvasGroup;
    }

    private void Start()
    {
        EventSubscription();
        canvasGroup = GetComponent<CanvasGroup>();
        DisableCanvasGroup();
    }

    public void Hello()
    {
        EnableCanvasGroup();
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
