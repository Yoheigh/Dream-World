using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    public Image itemIcon;
    public GameObject DisableObj;
    public Text itemAmount;

    public void Draw(ItemV2 _item)
    {
        Debug.Log("¾Æ´Ï ¹¹ÇÔ");
        itemIcon.sprite = _item.itemIcon;
        // itemAmount.text = _item.itemCount.ToString();
        // itemText.text = _item.itemDescription;
    }
    public void DrawWithCount(ItemV2 _item)
    {
        Debug.Log("¾Æ´Ï ¹¹ÇÔ");
        itemIcon.sprite = _item.itemIcon;
        itemAmount.text = _item.itemCount.ToString();
        // itemText.text = _item.itemDescription;
    }

    public void Init()
    {
        itemIcon.sprite = null;
        // itemAmount.text = "";
    }
}
