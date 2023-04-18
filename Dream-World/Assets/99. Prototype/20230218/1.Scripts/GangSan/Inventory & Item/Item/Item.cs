using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    ingredients , equipment , structure
}

[System.Serializable]
public class Item
{
    [Header("Item")]
    [Tooltip("아이템 ID")]
    public int itemID;
    [Tooltip("아이템 타입")]
    public ItemType itemType;
    [Tooltip("아이템 개수")]
    public int itemCurrentCount = 1;
    [Tooltip("아이템 최대 개수")]
    public int itemMaxCount;
    [Tooltip("아이템 이름")]
    public string itemName;
    [Tooltip("아이템 아이콘 사진 경로")]
    public string itemIconPath;
    [Tooltip("아이템 아이콘 사진 경로")]
    public string itemGameObjectPath;


    public int[] needIngredientsItemID;
    public int[] needIngredientsItemAmount;

    public Item(int itemID_)
    {
        Item item = ItemDatabass.instance.GetItem(itemID_);

        itemID = item.itemID;
        itemType = item.itemType;
        itemCurrentCount = item.itemCurrentCount;
        itemMaxCount = item.itemMaxCount;
        itemName = item.itemName;
        itemIconPath = item.itemIconPath;
        itemGameObjectPath = item.itemGameObjectPath;
        needIngredientsItemID = item.needIngredientsItemID;
        needIngredientsItemAmount = item.needIngredientsItemAmount;
    }
}
