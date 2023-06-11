using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridV2
{
    public float gridX;
    public float gridY;
    public float gridZ;

    private GridObject[,,] gridObjects;
    public GridObject[,,] GridObjects { private set { } get { return gridObjects; } }


    //그리드 내부의 셋 후 시스템에 게임오브젝트 설치 요청
    public void SetGridObject(int x, int y, int z, GridObject value)
    {
        if (x > gridX - 1 || x < 0 || y > gridY - 1 || y < 0 || z > gridZ - 1 || z < 0)
            return;
        if (gridObjects[x, y, z].gameObject != null)
            GridSystem.Instance.DestoryStageBlock(gridObjects[x, y, z].gameObject);
        gridObjects[x, y, z] = value;
        value.SetPosition(x, y, z);
        value.CheckArround();
        if (GetGridObject(x - 1, y, z) != null) GetGridObject(x - 1, y, z).CheckArround();
        if (GetGridObject(x + 1, y, z) != null) GetGridObject(x + 1, y, z).CheckArround();
        if (GetGridObject(x, y - 1, z) != null) GetGridObject(x, y - 1, z).CheckArround();
        if (GetGridObject(x, y + 1, z) != null) GetGridObject(x, y + 1, z).CheckArround();
        if (GetGridObject(x, y, z - 1) != null) GetGridObject(x, y, z - 1).CheckArround();
        if (GetGridObject(x, y, z + 1) != null) GetGridObject(x, y, z + 1).CheckArround();

        GridSystem.Instance.GenerateStageBlock(x, y, z);
    }

    public void InGrid(int x, int y, int z, GridObject gridObject)
    {
        gridObjects[x, y, z] = gridObject;
    }

    public GridObject GetGridObject(int x, int y, int z)
    {
        try
        {
            return gridObjects[x, y, z];
        }
        catch
        {
            return null;
        }
    }

    public GridV2(int _StageWidth, int _StageHeight)
    {
        gridX = _StageWidth;
        gridY = _StageHeight;
        gridZ = _StageWidth;

        gridObjects = new GridObject[_StageWidth, _StageHeight, _StageWidth];
    }
}
