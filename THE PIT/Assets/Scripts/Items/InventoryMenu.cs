using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private List<InventorySlot> inventorySlots;

    private Unit subjectUnit;

    private void EventSubscription()
    {
        VagueGameEvent.Instance.onInventoryOpenRequest += InMenuSetup;
        VagueGameEvent.Instance.onInventoryCloseRequest += DisableCanvasGroup;
    }

    private void GetComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventorySlots = new List<InventorySlot>();
        
        Transform container = transform.Find("ContainerCenter/InventoryDisplay/ContainerRows");
        if (container == null) return;
        foreach (Transform child in container)
        {
            if (child.TryGetComponent(out InventorySlot slot))
            {
                inventorySlots.Add(slot);
            }
        }
    }

    private void Start()
    {
        GetComponents();
        EventSubscription();
        DisableCanvasGroup();
    }   

    private void InMenuSetup(object data)
    {  
        if (data is not UnitInventory unitInventory) return;

        DisplayItems(unitInventory.itemArray);

        EnableCanvasGroup();
    }

    private void DisplayItems(ItemSO[] itemArray)
    {
        if (!itemArray.Any()) { return; }
        for (int i = 0; i < itemArray.Length; i++)
        {
            if (itemArray[i] == null) continue;
            inventorySlots[i].Display(itemArray[i]);
        }
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
