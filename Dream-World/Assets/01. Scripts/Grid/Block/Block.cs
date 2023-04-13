using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockData blockData;
}

public struct BlockData
{
    public int blockID;
    public bool isConstructable;
    // public Vector3 blockPos;
}
