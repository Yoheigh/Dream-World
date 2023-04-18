using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    public float gridX;
    public float gridY;
    public float gridZ;

    private T[,,] gridArray;
    private Vector3 originPos;

    public Grid(int _StageWidth, int _StageHeight, Vector3 _originPos, Func<T> _T)
    {
        gridX = _StageWidth;
        gridY = _StageHeight;
        gridZ = _StageWidth;

        originPos = _originPos;

        gridArray = new T[_StageWidth, _StageHeight, _StageWidth];

        for(int x = 0; x < gridArray.GetLength(0); x++) {
            for(int y = 0; y < gridArray.GetLength(1); y++) {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    gridArray[x, y, z] = _T();
                }
            }
        }
    }

    private void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPos).x);
        y = Mathf.FloorToInt((worldPosition - originPos).y);
        z = Mathf.FloorToInt((worldPosition - originPos).z);
    }

    public void SetGridObject(int x, int y, int z, T value)
    {
        try
        {
            gridArray[x, y, z] = value;
        }
        catch (System.Exception)
        {
            Debug.LogError("Grid에 Set 실패.");
        }
    }

    public T GetGridObject(int x, int y, int z)
    {
        try
        {
            return gridArray[x, y, z];
        }
        catch (System.Exception)
        {
            Debug.LogError("Grid에 Get 실패.");
            return default(T);
        }
    }

    public T GetGridObject(Vector3 worldPosition)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        return GetGridObject(x, y, z);
    }
}
