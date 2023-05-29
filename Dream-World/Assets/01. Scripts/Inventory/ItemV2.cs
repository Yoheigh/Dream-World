using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypeV2
{
    Ingredient,     // 재료
    Equipment,      // 장비
    Structure,      // 건축물
    Consumable      // 소모품
}

[System.Serializable]
public class ItemV2
{
    [Header("아이템")]
    public int itemID;
    public ItemTypeV2 itemType;
    public int itemCount;
    public int itemMaxCount;
    public string itemName;
    public string itemDescription;

    // 기획자가 직관적으로 수정할 수 있게끔 Sprite로 선언
    // 유지 보수를 위한 과정은 다음에
    public Sprite itemIcon;

    // 아이템 데이터 생성자
    public ItemV2(int _itemID, ItemTypeV2 _itemType, string _itemName, string _itemDescription = null, int _itemCount = 1, int _itemMaxCount = 64, Sprite _itemIcon = null)
    {
        itemID = _itemID;                       // 아이템 ID
        itemType = _itemType;                   // 아이템 타입
        itemName = _itemName;                   // 아이템 이름
        itemDescription = _itemDescription;     // 아이템 설명
        itemCount = _itemCount;                 // 아이템 개수 (기본 : 1)
        itemMaxCount = _itemMaxCount;           // 아이템 최대 개수 (기본 : 32)
        itemIcon = _itemIcon;                   // 아이템 스프라이트 아이콘
    }

    // 엑셀 및 Json 직렬화 데이터용
    public ItemV2(int _itemID)
    {
        // itemID를 통해 ItemDatabase에서 해당 아이템 데이터를 가져온다.

        // 가져온 데이터를 통해 아이템 데이터를 생성한다.

        // 굳이 두 번 거칠 이유가 있나 싶긴 하지만 아무튼
    }

    // 상속용
    public ItemV2() { }
}
