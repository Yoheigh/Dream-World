using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCube : Block
{
    public override void CheckCondition()
    {
        if (upGridObjectIndex == 3 || downGridObjectIndex == 3 || rightGridObjectIndex == 3 || leftGridObjectIndex == 3 || frontGridObjectIndex == 3 || backGridObjectIndex == 3)
        {
            GridSystem.Instance.StartCoroutine(Effect());
        }
    }

    public IEnumerator Effect()
    {
        Debug.Log("레드 큐브 효과 발동 3초 전");
        yield return new WaitForSeconds(1f);
        Debug.Log("레드 큐브 효과 발동 2초 전");
        yield return new WaitForSeconds(1f);
        Debug.Log("레드 큐브 효과 발동 1초 전");
        yield return new WaitForSeconds(1f);
        Debug.Log("레드 큐브 효과 발동");
    }

    public RedCube()
    {
        gridObjectData = DataBase.Instance.GetGridObjectData(2);
    }
}
