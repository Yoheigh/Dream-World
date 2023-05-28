using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeV2
{
    Ingredient,     // ���
    Equipment,      // ���
    Structure,      // ���๰
    Consumable      // �Ҹ�ǰ
}

[System.Serializable]
public class ItemV2
{
    [Header("������")]
    public int itemID;
    public ItemType itemType;
    public string itemName;
    public string itemDescription;

    // ��ȹ�ڰ� ���������� ������ �� �ְԲ� Sprite�� ����
    // ���� ������ ���� ������ ������!
    public Sprite itemIcon;

    // ������ ������ ������
    public ItemV2(int _itemID, ItemType _itemType, string _itemName, string _itemDescription, Sprite _itemIcon = null)
    {
        itemID = _itemID;
        itemType = _itemType;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemIcon = _itemIcon;
    }

    // ���� �� Json ����ȭ �����Ϳ�
    public ItemV2(int _itemID)
    {
        // itemID�� ���� ItemDatabase���� �ش� ������ �����͸� �����´�.

        // ������ �����͸� ���� ������ �����͸� �����Ѵ�.

        // ���� �� �� ��ĥ ������ �ֳ� �ͱ� ������ �ƹ�ư
    }
}
