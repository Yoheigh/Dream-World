using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCube : Block
{
    public BlueCube()
    {
        gridObjectData = DataBase.Instance.GetGridObjectData(3);
    }
}
