using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridV2
{
    public float gridX;
    public float gridY;
    public float gridZ;

    private GridObject[,,] gridObjects;

    //그리드 내부의 셋 후 시스템에 게임오브젝트 설치 요청
    public void SetGridObject(int x, int y, int z, GridObject value)
    {
        if (gridObjects[x, y, z].gameObject != null)
            GridSystem.Instance.DestoryStageBlock(gridObjects[x, y, z].gameObject);
        gridObjects[x, y, z] = value;
        value.SetPosition(x, y, z);
        value.CheckArround();
        if (GetGridObject(x-1, y, z) != null) GetGridObject(x-1, y, z).CheckArround();
        if (GetGridObject(x+1, y, z) != null) GetGridObject(x+1, y, z).CheckArround();
        if (GetGridObject(x, y-1, z) != null) GetGridObject(x, y-1, z).CheckArround();
        if (GetGridObject(x, y+1, z) != null) GetGridObject(x, y+1, z).CheckArround();
        if (GetGridObject(x, y, z-1) != null) GetGridObject(x, y, z-1).CheckArround();
        if (GetGridObject(x, y, z+1) != null) GetGridObject(x, y, z+1).CheckArround();

        GridSystem.Instance.GenerateStageBlock(x,y,z);
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

        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int y = 0; y < gridObjects.GetLength(1); y++)
            {
                for (int z = 0; z < gridObjects.GetLength(2); z++)
                {
                    gridObjects[x, y, z] = new Air();
                }
            }
        }
    }
}
