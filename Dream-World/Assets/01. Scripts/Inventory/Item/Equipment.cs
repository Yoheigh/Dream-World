using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment", order = 1)]
public class Equipment : ItemV2
{
    // public EquipmentAction function;            // 아이템 동작 정의
    [Header("장비 설정")]
    public float EquipActionSpeed;          // 장비 동작 속도 -> 기본 : 1배속

    public float EquipRange;                // 장비 동작 범위
    public float EquipAnimationDelay;       // 장비 동작 후 딜레이
    public float EquipAfterDelay;           // 장비 동작 후 딜레이

    public void Action() { }           // 장비 실제 기능

    public void AfterAction() { }   // 장비 동작이 끝난 후 필요하다면 사용

    public Equipment(int _itemID, ItemTypeV2 _itemType, string _itemName, string _itemDescription = null, int _itemCount = 1, int _itemMaxCount = 64, Sprite _itemIcon = null)
    {
        itemID = _itemID;                       // 아이템 ID
        itemType = _itemType;                   // 아이템 타입
        itemName = _itemName;                   // 아이템 이름
        itemDescription = _itemDescription;     // 아이템 설명
        itemCount = _itemCount;                 // 아이템 개수 (기본 : 1)
        itemMaxCount = _itemMaxCount;           // 아이템 최대 개수 (기본 : 32)
        itemIcon = _itemIcon;                   // 아이템 스프라이트 아이콘
    }
}