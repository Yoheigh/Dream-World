using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();       // 리소스 리스트 목록 관리

    public Managers Manager => Managers.Instance;                                     // 매니저를 통해 Object 관리를 하기 위해 선언

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)    // 리소스 전용 Instantiate 함수
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to Load Prefab : {key}");
            return null;
        }

        //if (pooling)                                                                // 해당 오브젝트가 오브젝트 풀에 들어간다면
        //    return Manager.Pool.Pop(prefab);                                        // 오브젝트 풀링에 넣는 처리 후 반환
                                                                                    /* 함수명이 Pop인 이유는 Stack 기반의 ObjectPool이기 때문 */
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

    public T Load<T>(string key) where T : Object                                       // 동기 로드
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            if (typeof(T) == typeof(Sprite))                                             // 2D 스프라이트 처리
            {
                Texture2D tex = resource as Texture2D;
                Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                return spr as T;
            }

            return resource as T;
        }
        return Resources.Load<T>(key);
    }

    public void LoadAsync<T>(string key, Action<T> callback = null) where T : Object    // 비동기 로드
    {
        if (_resources.TryGetValue(key, out Object resource))
        {                                                                               // 키 값 검사
            callback?.Invoke(resource as T);
            return;                                                                     // 콜백 검사 후 반환
        }

        var asyncOperation = Addressables.LoadAssetAsync<T>(key);                       // 1번째 핸들 : 각 리소스 호출이 완료되었을 때 호출
        asyncOperation.Completed += (op) =>                                             // 리소스 로딩
        {                                                                               // 완료되면 콜백 검사 후 반환
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : Object
    {
        var OpHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));   // 2번째 핸들 : 모든 리소스 호출이 완료되었을 때 호출

        OpHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (obj) =>
                {
                    loadCount++;

                    // AsyncOperationHandle의 iList의 요소들은 PrimaryKey, InternalId, ProviderId, DependencyHashCode, ResourceLocationData를 가지고 있다.
                    // result : Assets/Prefabs/Box.prefab ( 게임 오브젝트의 주소)
                    Debug.Log($"result : {result}");

                    // result.PrimaryKey : Box ( 어드레서블에 등록된 키)
                    Debug.Log($"result.PrimaryKey : {result.PrimaryKey}");

                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }
}
