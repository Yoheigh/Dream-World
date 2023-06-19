using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    // 목표 :  엑셀 및 Json으로 받아 등록할 아이템 리스트
    public Dictionary<int, ItemV2> ItemDic = new Dictionary<int, ItemV2>();

    // 목표 :  엑셀 및 Json으로 받아 등록할 블록 리스트
    public Dictionary<int, GridObjectData> GridObjectDic = new Dictionary<int, GridObjectData>()
    {
        {0, new GridObjectData(0, "DefaultBlock", string.Empty, true, true) },
        {1, new GridObjectData(1, "DefaultBlock-TopOnly", string.Empty, true, false) },
        {2, new GridObjectData(1, "DefaultBlock-SideOnly", string.Empty, false, true) },
        {3, new GridObjectData(1, "DefaultBlock-MGSV", string.Empty, false, true) },
    };

    public ItemV2 GetItem(int index)
    {
        if (ItemDic.TryGetValue(index, out var item))
            return item;

        else
            throw new System.Exception("ItemDic에 해당 인덱스가 없습니다.");
    }
    
    public GridObjectData GetBlock(int index)
    {
        if (GridObjectDic.TryGetValue(index, out var block))
            return block;

        else
            throw new System.Exception("GridObjectDic에 해당 인덱스가 없습니다.");
    }
}
