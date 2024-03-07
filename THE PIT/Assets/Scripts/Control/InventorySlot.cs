using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Setup")]
    [SerializeField] InventoryMenu parentInventory;
    [SerializeField] Color colDefault = new Color(1,1,1,1);
    [SerializeField] Color colSelected = new Color(0.9f,0.9f,0.9f,1);
    [SerializeField] Color colDisabled = new Color(0.2f,0.2f,0.2f,1);

    private enum SlotState {
        disabled,
        enabled,
    }
    private SlotState slotState = SlotState.disabled;

    private Item itemInSlot;

    // Child UI elements
    private Transform panel;
    private Image panelImage;
    private Image icon;
    private CUIText itemNameText;
    private CUIText usesText;

    private CanvasGroup canvasGroup;

    void GetComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        panel = transform.Find("Panel");
        panelImage = panel.GetComponent<Image>();

        Transform ItemIcon = panel.Find("ItemIcon");
        icon = ItemIcon.GetComponent<Image>();

        Transform ItemName = panel.Find("ItemName");
        itemNameText = ItemName.GetComponent<CUIText>();

        Transform ItemUses = panel.Find("ItemUses");
        usesText = ItemUses.GetComponent<CUIText>();
    }

    void Start()
    {
        GetComponents();
        DisableCanvasGroup();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (slotState == SlotState.disabled) return;
        IMState imstate = parentInventory.menuState;
        switch (itemInSlot.itemType)
        {
            case ItemType.consumable:
            if (imstate is IMState.IMItem){ 
                parentInventory.unitInventory.Use(itemInSlot);
                CloseIMandAM(); 
            }
            break;

            case ItemType.material:
            break;

            case ItemType.weapon:
            break;

            default: 
            break;
        }
        parentInventory.unitInventory.Use(itemInSlot);

        if (itemInSlot.itemType == ItemType.consumable)
        {

        }
    }

    void CloseIMandAM()
    {
        VagueGameEvent.Instance.InventoryCloseRequest();
        VagueGameEvent.Instance.ActionMenuCloseRequest();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (slotState == SlotState.disabled) { return; }
        panelImage.color = colSelected;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (slotState == SlotState.disabled) { return; }
        panelImage.color = colDefault;
    }

    public void Display(Item item)
    {
        EnableCanvasGroup();

        itemInSlot = item;
        icon.sprite = itemInSlot.sprite;
        itemNameText.SetText(itemInSlot.itemName);
        usesText.SetText(itemInSlot.uses.ToString());

        CheckShouldEnableSlot();

        gameObject.SetActive(true);
    }

    private void CheckShouldEnableSlot()
    {
        IMState imstate = parentInventory.menuState;
        switch (itemInSlot.itemType)
        {
            case ItemType.consumable:
            if (imstate == IMState.IMItem) { slotState = SlotState.enabled; }
            else { slotState = SlotState.disabled; }
            break;

            case ItemType.material:
            break;

            case ItemType.weapon:
            if (imstate == IMState.IMAttack) { slotState = SlotState.enabled; }
            else { slotState = SlotState.disabled; }
            break;

            default: 
            break;
        }
        StateToSlotColor();
    }

    private void StateToSlotColor()
    {
        switch(slotState)
        {
            case SlotState.disabled:
            panelImage.color = colDisabled;
            break;

            case SlotState.enabled:
            panelImage.color = colDefault;
            break;
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