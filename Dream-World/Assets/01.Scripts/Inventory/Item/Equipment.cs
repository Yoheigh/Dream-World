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
    public Vector3 EquipOffset;                     // 장비 동작 위치 오프셋
    public float EquipActionDelay;                  // 장비 기능 실제 작동까지 걸리는 시간
    public float EquipActionEndDelay;               // 장비 동작 완료까지 걸리는 시간

    public Vector3 EquipHandOffset;                 // 장비 손에 장착되는 로테이션 값
    public string EquipPrefabPath;                  // 장비 모델 경로
    public string EquipAnimName;                    // 플레이어 동작 애니메이션 이름
    //public string EquipAnimTop;                     /* 장비가 [위쪽 오브젝트]를 부셨을 때 */
    //public string EquipAnimBottom;                  /* 장비가 [아래쪽 오브젝트]를 부셨을 때 */
    //public string EquipAnimNone;                    /* 장비가 상호작용하지 못했을 떄 */

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

    public Equipment(Equipment _item)
    {
        itemID = _item.itemID;                       // 아이템 ID
        itemType = _item.itemType;                   // 아이템 타입
        itemName = _item.itemName;                   // 아이템 이름
        itemDescription = _item.itemDescription;     // 아이템 설명
        itemCount = _item.itemCount;                 // 아이템 개수 (기본 : 1)
        itemMaxCount = _item.itemMaxCount;           // 아이템 최대 개수 (기본 : 32)
        itemIcon = _item.itemIcon;                   // 아이템 스프라이트 아이콘

        EquipActionType = _item.EquipActionType;    // 장비 액션 타입 ( 근접, 원거리 등 )
        EquipEffectiveType = _item.EquipEffectiveType;

        EquipRange = _item.EquipRange;                        // 장비 동작 범위
        EquipOffset = _item.EquipOffset;                     // 장비 동작 위치 오프셋
        EquipActionDelay = _item.EquipActionDelay;                  // 장비 기능 실제 작동까지 걸리는 시간
        EquipActionEndDelay = _item.EquipActionEndDelay;               // 장비 동작 완료까지 걸리는 시간

        EquipHandOffset = _item.EquipHandOffset;                 // 장비 손에 장착되는 로테이션 값
        EquipPrefabPath = _item.EquipPrefabPath;                  // 장비 모델 경로
        EquipAnimName = _item.EquipAnimName;                    // 플레이어 동작 애니메이션 이름
    }
}