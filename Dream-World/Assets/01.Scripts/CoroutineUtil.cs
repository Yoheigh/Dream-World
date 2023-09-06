using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CoroutineUtil : MonoBehaviour
{
    private static MonoBehaviour monoInstance;

    private static CoroutineUtil instance;
    public static CoroutineUtil Instance { get { Init(); return instance; } }

    public static void Init()
    {
        if(monoInstance == null)
        {
            monoInstance = new GameObject($"[{nameof(CoroutineUtil)}]").AddComponent<CoroutineUtil>();
            instance = monoInstance.GetComponent<CoroutineUtil>();
            DontDestroyOnLoad(monoInstance.gameObject);
        }
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }

    /*
     * 
     * 
     */

    public void Graph(float lerpTime, Action<float> callback = null, GraphType graphType = GraphType.Ease_Out)
    {
        StartCoroutine(GraphCoroutine(lerpTime, callback, graphType));
    }

    private IEnumerator GraphCoroutine(float lerpTime, Action<float> callback = null, GraphType graphType = GraphType.Ease_Out)
    {
        float currentTime = 0f;
        float t;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= lerpTime)
                currentTime = lerpTime;

            t = currentTime / lerpTime;

            switch (graphType)
            {
                case GraphType.Linear:                          // 리니어
                    break;

                case GraphType.Ease_Out:                        // 마지막에 천천히 느려짐
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                case GraphType.Ease_In:                         // 마지막에 천천히 빨라짐
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case GraphType.Smoothstep:                      // 부드러운 전환
                    t = t * t * (3f - 2f * t);
                    break;

                case GraphType.Smootherstep:                    // 더욱 부드러운 전환
                    t = t * t * t * (t * (6f * t - 15f) + 10f);
                    break;
            }

            // 콜백 실행
            callback?.Invoke(t);
            yield return null;
        }
    }

    // Sine 함수 콜백 ( 점차 증가하는 속도가 느려지는 그래프 (ease out) )
    public void GraphSine(float lerpTime, Action<float> callback = null)
    {
        StartCoroutine(GraphSineCoroutine(lerpTime, callback));
    }

    private IEnumerator GraphSineCoroutine(float lerpTime, Action<float> callback = null)
    {
        float currentTime = 0f;
        float t;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= lerpTime)
                currentTime = lerpTime;

            t = currentTime / lerpTime;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            // 콜백 실행
            callback?.Invoke(t);
            yield return null;
        }
    }
}
