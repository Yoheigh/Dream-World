using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : Singleton<DataBase>
{
    public SerializableDictionary<int, GridObjectData> blockDatas = new SerializableDictionary<int, GridObjectData>();

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
        blockDatas.Add(0,new GridObjectData(0, "Air", "None", false, false));
        blockDatas.Add(1, new GridObjectData(1, "Cube", "BlockPrefabs/Cube", false, false));
        blockDatas.Add(2, new GridObjectData(2, "RedCube", "BlockPrefabs/RedCube", false, false));
        blockDatas.Add(3, new GridObjectData(3, "BlueCube", "BlockPrefabs/BlueCube", false, false));
        blockDatas.Add(4, new GridObjectData(4, "PurpleCube", "BlockPrefabs/PurpleCube", false, false));
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
