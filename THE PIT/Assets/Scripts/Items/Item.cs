using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemSO;

    [Header("UI")]
    public Sprite sprite;

    private void Start()
    {
        InitItem(itemSO);
    }

    public void InitItem(ItemSO newItem)
    {
        sprite = newItem.sprite;
    }
}

