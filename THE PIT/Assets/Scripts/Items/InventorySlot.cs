using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ItemSO item;
    private InventoryMenu parentMenu;

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
        VagueGameEvent.Instance.InventoryCloseRequest();
        VagueGameEvent.Instance.ActionMenuCloseRequest();
        VagueGameEvent.Instance.ItemUsed(this, item);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        panelImage.color = new Color(0.9f,0.9f,0.9f,1);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        panelImage.color = Color.white;
    }

    public void Display(ItemSO itemSO)
    {
        EnableCanvasGroup();

        item = itemSO;
        icon.sprite = item.sprite;
        itemNameText.SetText(item.itemName);
        usesText.SetText(item.uses.ToString());

        gameObject.SetActive(true);
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
