using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    public int upGridObjectIndex , downGridObjectIndex, leftGridObjectIndex, rightGridObjectIndex, frontGridObjectIndex, backGridObjectIndex;
    public int xPos, yPos, zPos;
    public GameObject gameObject;
    public GridObjectData gridObjectData;


    public void CheckArround()  //주변 오브젝트 체크 및 상태 업데이트 호출
    {
        if (yPos < 29   &&          GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos + 1, zPos) != null)
            upGridObjectIndex =     GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos + 1, zPos).GetGridObjectData().blockID;

        if (yPos > 0    &&          GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos - 1, zPos) != null)
            downGridObjectIndex =   GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos - 1, zPos).GetGridObjectData().blockID;

        if (xPos < 99   &&          GridSystem.Instance.StageGrid.GetGridObject(xPos - 1, yPos, zPos) != null)
            leftGridObjectIndex =   GridSystem.Instance.StageGrid.GetGridObject(xPos - 1, yPos, zPos).GetGridObjectData().blockID;

        if (xPos > 0    &&          GridSystem.Instance.StageGrid.GetGridObject(xPos + 1, yPos, zPos) != null)
            rightGridObjectIndex =  GridSystem.Instance.StageGrid.GetGridObject(xPos + 1, yPos, zPos).GetGridObjectData().blockID;

        if (zPos < 99   &&          GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos, zPos + 1) != null)
            frontGridObjectIndex =  GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos, zPos + 1).GetGridObjectData().blockID;

        if (zPos > 0    &&          GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos, zPos - 1) != null)
            backGridObjectIndex =   GridSystem.Instance.StageGrid.GetGridObject(xPos, yPos, zPos - 1).GetGridObjectData().blockID;

        CheckCondition();
    }

    public virtual void CheckCondition()   //나중에 주변 블럭의 따른 효과들을 적을 곳, 상속 받은 후 override 방식 사용 예정
    {

    }

    public void SetPosition(int x, int y, int z)    //주변을 체크 하기 위해 자기 자신 위치를 알고 있어야 하기 때문에 자기 위치 기억
    {
        xPos = x;
        yPos = y;
        zPos = z;
    }

    public void SetGameObject(GameObject gameObject_)
    {
        gameObject = gameObject_;
    }
    public GridObjectData GetGridObjectData()
    {
        try
        {
            return gridObjectData;
        }
        catch
        {
            return null;
        }
    }
}
