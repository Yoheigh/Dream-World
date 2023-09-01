using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();       // ���ҽ� ����Ʈ ��� ����

    public Managers Manager => Managers.Instance;                                     // �Ŵ����� ���� Object ������ �ϱ� ���� ����

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)    // ���ҽ� ���� Instantiate �Լ�
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to Load Prefab : {key}");
            return null;
        }

        //if (pooling)                                                                // �ش� ������Ʈ�� ������Ʈ Ǯ�� ���ٸ�
        //    return Manager.Pool.Pop(prefab);                                        // ������Ʈ Ǯ���� �ִ� ó�� �� ��ȯ
                                                                                    /* �Լ����� Pop�� ������ Stack ����� ObjectPool�̱� ���� */
        GameObject go = Object.Instantiate(prefab, parent);

        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        //if (Manager.Pool.Push(go))
        //    return;

        Object.Destroy(go);
    }

    public T Load<T>(string key) where T : Object                                       // ���� �ε�
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            if (typeof(T) == typeof(Sprite))                                             // 2D ��������Ʈ ó��
            {
                Texture2D tex = resource as Texture2D;
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                return spr as T;
            }

            return resource as T;
        }
        return Resources.Load<T>(key);
    }

    public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object    // �񵿱� �ε�
    {
        if (_resources.TryGetValue(key, out Object resource))
        {                                                                               // Ű �� �˻�
            callback?.Invoke(resource as T);
            return;                                                                     // �ݹ� �˻� �� ��ȯ
        }

        var asyncOperation = Addressables.LoadAssetAsync<T>(key);                       // 1��° �ڵ� : �� ���ҽ� ȣ���� �Ϸ�Ǿ��� �� ȣ��
        asyncOperation.Completed += (op) =>                                             // ���ҽ� �ε�
        {                                                                               // �Ϸ�Ǹ� �ݹ� �˻� �� ��ȯ
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
    {
        var OpHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));   // 2��° �ڵ� : ��� ���ҽ� ȣ���� �Ϸ�Ǿ��� �� ȣ��

        OpHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    loadCount++;

                    // AsyncOperationHandle�� iList�� ��ҵ��� PrimaryKey, InternalId, ProviderId, DependencyHashCode, ResourceLocationData�� ������ �ִ�.
                    // result : Assets/Prefabs/Box.prefab ( ���� ������Ʈ�� �ּ�)
                    Debug.Log($"result : {result}");

                    // result.PrimaryKey : Box ( ��巹���� ��ϵ� Ű)
                    Debug.Log($"result.PrimaryKey : {result.PrimaryKey}");

                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }
}
