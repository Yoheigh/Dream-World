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
    public int itemID;
    [Tooltip("������ Ÿ��")]
    public ItemType itemType;
    [Tooltip("������ ����")]
    public int itemCurrentCount = 1;
    [Tooltip("������ �ִ� ����")]
    public int itemMaxCount;
    [Tooltip("������ �̸�")]
    public string itemName;
    [Tooltip("������ ������ ���� ���")]
    public string itemIconPath;
    [Tooltip("������ ������ ���� ���")]
    public string itemGameObjectPath;


    public int[] needIngredientsItemID;
    public int[] needIngredientsItemAmount;




    public Item()
    {

    }

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
