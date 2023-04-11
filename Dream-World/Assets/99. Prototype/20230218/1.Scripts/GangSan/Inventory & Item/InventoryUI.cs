using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryObject;
    public Transform itemSlotsHolder;
    public ItemSlot[] itemSlots;

    private void Start()
    {
        itemSlots = itemSlotsHolder.GetComponentsInChildren<ItemSlot>();
        Inventory.instance.onChangeItem += RedrawItemSlot;

    }

    public void RedrawItemSlot()
    {
        foreach(ItemSlot itemSlot in itemSlots)
        {
            itemSlot.item = null;
            itemSlot.UpdateState();
        }
        for(int i = 0; i < Inventory.instance.ingredientsItems.Count; i++)
        {
            itemSlots[i].item = Inventory.instance.ingredientsItems[i];
            itemSlots[i].UpdateState();
        }
    }
}
