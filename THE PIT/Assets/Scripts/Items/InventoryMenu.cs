using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] List<GameObject> iSlotObjects;

    private CanvasGroup canvasGroup;
    private List<InventorySlot> inventorySlots;

    public UnitInventory unitInventory { get; private set; } 

    private void EventSubscription()
    {
        VagueGameEvent.Instance.onInventoryOpenRequest += InMenuSetup;
        VagueGameEvent.Instance.onInventoryCloseRequest += DisableCanvasGroup;
    }

    private void GetComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventorySlots = new List<InventorySlot>();
        
        foreach (GameObject ob in iSlotObjects)
        {
            if (ob.TryGetComponent(out InventorySlot slot)) { inventorySlots.Add(slot); }
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
        if (data is not UnitInventory invent) return;

        unitInventory = invent;
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
