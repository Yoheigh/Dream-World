using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class MethodExtensions
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    //public static void BindEvent(this GameObject go
    //    , Action action = null
    //    , Action<BaseEventData> dragAction = null
    //    , Define.UIEvent type = Define.UIEvent.Click)
    //{
    //    UIBase.BindEvent(go, action, dragAction, type);
    //}

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    //public static void DestoryChilds(this GameObject go)
    //{
    //    Transform[] children = new Transform[go.transform.childCount];
    //    for (int i = 0; i < go.transform.childCount; i++)
    //    {
    //        children[i] = go.transform.GetChild(i);
    //    }

    //    foreach (Transform child in children)    //모든 자식 오브젝트 삭제 
    //    {
    //        Managers.Resource.Destroy(child.gameObject);
    //    }
    //}

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void TryGetValue<T>(this IList<T> list, T key, out T value)
    {
        if (list.Contains(key))
        {
            value = key;
            return;
        }

        value = default(T);
    }

    // List에서 랜덤한 아이템 가져오기
    public static T RandomItem<T>(this List<T> list)
    {
        if (list.Count == 0)
            throw new IndexOutOfRangeException("List is Empty!");

        var randomIndex = Random.Range(0, list.Count);

        return list[randomIndex];
    }

    // Vector3 값을 int로 반올림 (RoundToInt)
    public static Vector3 GetXYZRound(this Vector3 vector3)
    {
        int x, y, z;

        x = Mathf.RoundToInt(vector3.x);
        y = Mathf.RoundToInt(vector3.y);
        z = Mathf.RoundToInt(vector3.z);

        return new Vector3(x, y, z);
    }

    // Vector3 값을 int로 반올림 (RoundToInt), out 있음
    public static Vector3 GetXYZRound(this Vector3 vector3, out int _x, out int _y, out int _z)
    {
        int x, y, z;

        x = Mathf.RoundToInt(vector3.x);
        y = Mathf.RoundToInt(vector3.y);
        z = Mathf.RoundToInt(vector3.z);

        _x = x;
        _y = y;
        _z = z;

        return new Vector3(x, y, z);
    }

    // Vector3 값을 int로 반올림 (FloorToInt)
    public static Vector3 GetXYZFloor(this Vector3 vector3)
    {
        int x, y, z;

        x = Mathf.FloorToInt(vector3.x);
        y = Mathf.FloorToInt(vector3.y);
        z = Mathf.FloorToInt(vector3.z);

        return new Vector3(x, y, z);
    }

    // Vector3 값을 int로 반올림 (FloorToInt), out 있음
    public static Vector3 GetXYZFloor(this Vector3 vector3, out int _x, out int _y, out int _z)
    {
        int x, y, z;

        x = Mathf.FloorToInt(vector3.x);
        y = Mathf.FloorToInt(vector3.y);
        z = Mathf.FloorToInt(vector3.z);

        _x = x;
        _y = y;
        _z = z;

        return new Vector3(x, y, z);
    }

    // Vector3 배열 중에 인수로 받은 Vector3와 가장 가까운 Vector3를 반환
    public static Vector3 GetClosestVector3(this Vector3 vector3, Vector3[] vector3s)
    {
        float closest = Mathf.Infinity;
        Vector3 closestVec = Vector3.positiveInfinity;

        for (int i = 0;  i < vector3s.Length; i++)
        {
            float temp = Vector3.Distance(vector3, vector3s[i]);

            if (closest > temp)
            {
                closest = temp;
                closestVec = vector3s[i];
            }
        }
        return closestVec;
    }

    // 인수로 받은 Vector3의 x, y, z 값과 곱하여 반환
    public static Vector3 Multiply(this Vector3 vector3, Vector3 newVector3)
    {
        vector3.x *= newVector3.x;
        vector3.y *= newVector3.y;
        vector3.z *= newVector3.z;

        return vector3;
    }

    // 인수로 받은 Vector3의 x, y, z 값과 더하여 반환
    public static Vector3 Sum(this Vector3 vector3, Vector3 newVector3)
    {
        vector3.x += newVector3.x;
        vector3.y += newVector3.y;
        vector3.z += newVector3.z;

        return vector3;
    }

    // Ray를 쐈을 때 가장 가까운 GameObject를 반환 { Distance()보다 정확함, 피봇 중점 )
    public static GameObject GetClosestGameObject(GameObject from, Collider[] targets)
    {
        float closestDistance = Mathf.Infinity;
        GameObject target = null;

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 dirToTarget = (from.transform.position - targets[i].transform.position).normalized;
            
            if(Physics.Raycast(from.transform.position, dirToTarget, out RaycastHit hitinfo))
            {
                if(closestDistance > hitinfo.distance)
                {
                    hitinfo.distance = closestDistance;
                    target = targets[i].gameObject;
                }
            }
        }

        return target;
    }
}
