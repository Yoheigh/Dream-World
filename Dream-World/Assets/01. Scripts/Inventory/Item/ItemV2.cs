using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeV2
{
    Ingredient,     // 재료
    Equipment,      // 장비
    Building,       // 건축물 ( 23-06-01 : 현재는 건축물도 소모품으로 묶어서 취급 )
    Consumable      // 소모품
}

[System.Serializable]
// [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/ItemV2", order = 0)]
public class ItemV2 : ScriptableObject
{
    [Header("기본 아이템 설정")]
    public int itemID;
    public ItemTypeV2 itemType;
    public int itemCount = 1;
    public int itemMaxCount = 64;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;

    // 엑셀 및 Json 직렬화 데이터할 때 쓰려고 예시로 만듦
    /* itemID에 따라서 아이템 타입을 결정하고 resource에 등록시키는 작업 필요 */
    public ItemV2(int _itemID)
    {

    }

    // 미안하다 메모리야
    public ItemV2(ItemV2 _item)
    {
        itemID = _item.itemID;
        itemType = _item.itemType;
        itemCount = _item.itemCount;
        itemName = _item.itemName;
        itemDescription = _item.itemDescription;
        itemIcon = _item.itemIcon;
    }    

    /* 업캐스팅 분류작업 끝내면 없애도 됨 */
    public ItemV2() { }
}