using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    // �ǹ� ���� ��ġ
    public Vector3 BuildPosition;

    // �ش� GridObject�� Data�� �ִ� ���ǵ��� �����ͼ�
    // �ش� ��ġ�� �ǹ��� ���� �� �ִ��� Ȯ��

    // ���� ó��

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(BuildPosition, 0.1f);
    }
}
