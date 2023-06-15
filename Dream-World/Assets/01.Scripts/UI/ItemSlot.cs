using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    public Image itemIcon;
    public GameObject DisableObj;

    public void Draw(ItemV2 _item)
    {
        itemIcon.sprite = _item.itemIcon;
        // itemText.text = _item.itemDescription;
    }
}
