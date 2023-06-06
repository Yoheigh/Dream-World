using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockV1 : MonoBehaviour
{
    [Header("Block")]
    private BlockData blockData;

    [SerializeField]
    private Vector3 pivot;
    public Vector3 Pivot
    {
        get => pivot + transform.position;
        set => pivot = Pivot;
    }

    [SerializeField]
    private Vector3 pivotRot;
    public Vector3 PivotRot
    {
        get => pivotRot;
        set => pivotRot = value;
    }

    public void RotateBlock(int rotate)
    {
        pivotRot.y += rotate;
    }

}

[System.Serializable]
public struct BlockData
{
    public int blockID;
    public string blockName;
    public string blockPrefabPath;
    public bool isConstructable;
    public bool isAffectedByGravity;

    public BlockData(int _blockID, string _blockName, string _blockPrefabPath, bool _isConstructable, bool _isAffectedByGravity)
    {
        blockID = _blockID;
        blockName = _blockName;
        blockPrefabPath = _blockPrefabPath;
        isConstructable = _isConstructable;
        isAffectedByGravity = _isAffectedByGravity;
    }
}
