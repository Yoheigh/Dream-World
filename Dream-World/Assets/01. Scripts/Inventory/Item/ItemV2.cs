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

    // 기획자가 직관적으로 수정할 수 있게끔 Sprite로 선언
    public Sprite itemIcon;

    // 엑셀 및 Json 직렬화 데이터용
    public ItemV2(int _itemID)
    {
        // itemID를 통해 ItemDatabase에서 해당 아이템 데이터를 가져온다.

        // 가져온 데이터를 통해 아이템 데이터를 생성한다.

        // 굳이 두 번 거칠 이유가 있나 싶긴 하지만 아무튼
    }

    // 업캐스팅 테스트용
    /* 위쪽 생성자 없애면 굳이 만들 필요 없음 */
    public ItemV2() { }
}