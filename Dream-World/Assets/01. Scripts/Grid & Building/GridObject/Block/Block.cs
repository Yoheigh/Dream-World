using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : GridObject
{
    //[SerializeField]
    //private Vector3 pivot;
    //public Vector3 Pivot
    //{
    //    get => pivot + transform.position;
    //    set { }
    //}

    public Block()
    {

    }

    public Block(int blockID)
    {
        //GridObjectData gridObjectData_ = DataBase.Instance.GetGridObjectData(blockID);
        //gridObjectData.blockID = gridObjectData_.blockID;
        //gridObjectData.blockName = gridObjectData_.blockName;
        //gridObjectData.blockPrefabPath = gridObjectData_.blockPrefabPath;
        //gridObjectData.isConstructable = gridObjectData_.isConstructable;
        //gridObjectData.isAffectedByGravity = gridObjectData_.isAffectedByGravity;
        gridObjectData = DataBase.Instance.GetGridObjectData(blockID);
    }
}
