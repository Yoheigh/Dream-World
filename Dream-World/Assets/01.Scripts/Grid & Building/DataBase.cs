using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : Singleton<DataBase>
{
    public Dictionary<int, GridObjectData> blockDatas = new Dictionary<int, GridObjectData>();
    public List<GridObjectData> gridObjectDatas = new List<GridObjectData>();

    public GridObjectData GetGridObjectData(int blockID)
    {
        if(blockDatas.ContainsKey(blockID))
        {
            return blockDatas[blockID];
        }

        return default;
    }

    protected override void Awake2()
    {
        foreach (var item in gridObjectDatas)
        {
            blockDatas.Add(item.blockID, item);
        }
    }
}

[System.Serializable]
public class GridObjectData
{
    public int blockID;
    public string blockName;
    public string blockPrefabPath;
    public bool isConstructable;
    public bool isAffectedByGravity;

    public GridObjectData(int _blockID)
    {
        GridObjectData blockData = DataBase.Instance.GetGridObjectData(_blockID);
    }

    public GridObjectData(int _blockID, string _blockName, string _blockPrefabPath, bool _isConstructable, bool _isAffectedByGravity)
    {
        blockID = _blockID;
        blockName = _blockName;
        blockPrefabPath = _blockPrefabPath;
        isConstructable = _isConstructable;
        isAffectedByGravity = _isAffectedByGravity;
    }
}
