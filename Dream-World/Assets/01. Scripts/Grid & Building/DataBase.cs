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
[CreateAssetMenu(fileName = "New GridObjectData", menuName = "GridObjectData", order = 1)]
public class GridObjectData : ScriptableObject
{
    [Header("기본 블록 설정")]
    public int blockID;
    public string blockName;
    public string blockPrefabPath;
    public bool isConstructable;        // 해당 블럭 위에 블럭 설치 가능 여부
    public bool isAffectedByGravity;    // 중력에 영향을 받는지


    //public byte blockX = 1;
    //public byte blockZ = 1;
    //public byte blockHeight = 1;

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