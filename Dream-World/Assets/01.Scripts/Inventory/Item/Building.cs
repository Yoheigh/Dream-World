using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildCondition
{
    Top,
    Side
}

[CreateAssetMenu(fileName = "New Building", menuName = "Item/Building", order = 2)]
public class Building : ItemV2
{
    [Header("블록 데이터")]
    // public GridObjectData GridObjectData;
    [Tooltip("건물을 건설할 위치입니다.")]
    public BuildCondition buildCondition = BuildCondition.Top;

    [Tooltip("플레이어 정면(forward) + 건설 장소 오프셋입니다.")]
    public Vector3 buildOffset;

    [Tooltip("건물 프리팹 경로입니다.")]
    public string buildPrefabPath;

    public Building(int _itemID, ItemTypeV2 _itemType, string _itemName, string _itemDescription = null, int _itemCount = 1, int _itemMaxCount = 64, Sprite _itemIcon = null)
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
