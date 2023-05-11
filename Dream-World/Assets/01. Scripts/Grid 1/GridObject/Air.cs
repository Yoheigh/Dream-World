using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : GridObject
{
    public Air()
    {
        gridObjectData = DataBase.Instance.GetGridObjectData(0);
        gameObject = null;
    }
}
