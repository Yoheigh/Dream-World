using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MethodExtensions
{
    // List���� ������ ������ ��������
    public static T RandomItem<T>(this List<T> list)
    {
        if (list.Count == 0)
            throw new IndexOutOfRangeException("List is Empty!");

        var randomIndex = Random.Range(0, list.Count);

        return list[randomIndex];
    }

    // Vector3 ���� int�� �ݿø� (RoundToInt)
    public static Vector3 GetXYZRound(this Vector3 vector3)
    {
        int x, y, z;

        x = Mathf.RoundToInt(vector3.x);
        y = Mathf.RoundToInt(vector3.y);
        z = Mathf.RoundToInt(vector3.z);

        return new Vector3(x, y, z);
    }
}
