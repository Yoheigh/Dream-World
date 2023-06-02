using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment", order = 1)]
public class Equipment : ItemV2
{
    //[Header("장비의 기능을 넣어주세요")]
    //public EquipmentAction equipAction;

    [Header("장비 기능 설정")]
    public EquipmentActionType EquipActionType = EquipmentActionType.Melee;     // 장비 액션 타입 ( 근접, 원거리 등 )
    public EffectiveType EquipEffectiveType;

    public float EquipRange;                        // 장비 동작 범위
    public float EquipActionDelay;                  // 장비 기능 실제 작동까지 걸리는 시간
    public float EquipActionEndDelay;               // 장비 동작 완료까지 걸리는 시간

    public string EquipPrefabPath;                  // 장비 모델 경로
    public string EquipAnimName;                    // 플레이어 동작 애니메이션 이름

    public Equipment(int _itemID, ItemTypeV2 _itemType, string _itemName, string _itemDescription = null, int _itemCount = 1, int _itemMaxCount = 64, Sprite _itemIcon = null, EquipmentAction _equipAction = null)
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