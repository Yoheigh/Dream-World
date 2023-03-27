using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    public int itemID;
    public int slotID;
    public bool isCanCrafting = false;
    public bool isHaveItem = false;
    public KeyCode craftingKeyCode;
    public Image itemIcon;
    public Image outline;
    public void CraftingCall()
    {
        if(CraftingTable.instance.Crafting(itemID))
        {
            isCanCrafting = false;
            isHaveItem = true;
            itemIcon.color = Color.white;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(craftingKeyCode) && isCanCrafting && !isHaveItem)
            CraftingCall();
    }

    public void Select()
    {
        outline.gameObject.SetActive(true);
    }

    public void DeSelect()
    {
        outline.gameObject.SetActive(false);
    }

    private void Start()
    {
        itemIcon.color = new Color(0.25f, 0.25f, 0.25f);
        if(ItemDatabass.instance.GetItem(itemID).itemIconPath != null)
            itemIcon.sprite = Resources.Load<Sprite>(ItemDatabass.instance.GetItem(itemID).itemIconPath);
    }
}
