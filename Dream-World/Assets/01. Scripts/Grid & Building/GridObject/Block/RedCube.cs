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
        Debug.Log("���� ť�� ȿ�� �ߵ� 3�� ��");
        yield return new WaitForSeconds(1f);
        Debug.Log("���� ť�� ȿ�� �ߵ� 2�� ��");
        yield return new WaitForSeconds(1f);
        Debug.Log("���� ť�� ȿ�� �ߵ� 1�� ��");
        yield return new WaitForSeconds(1f);
        Debug.Log("���� ť�� ȿ�� �ߵ�");
    }

    public RedCube()
    {
        gridObjectData = DataBase.Instance.GetGridObjectData(2);
    }
}
