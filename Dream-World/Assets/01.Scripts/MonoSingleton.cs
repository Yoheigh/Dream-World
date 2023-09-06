using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T s_instance;

    public static T Instance 
    { 
        get 
        {
            if (s_instance == null)
            {
                GameObject obj = GameObject.Find($"@{typeof(T)}");
                if(obj == null)
                {
                    obj = new GameObject($"@{typeof(T)}");
                    obj.AddComponent<T>();
                    Debug.Log($"@{typeof(T)} »ý¼º");
                }
                s_instance = obj.GetComponent<T>();
                DontDestroyOnLoad(s_instance.gameObject);
            }
            return s_instance; 
        } 
    }
}
