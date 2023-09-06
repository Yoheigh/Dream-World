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
                case GraphType.Linear:                          // ���Ͼ�
                    break;

                case GraphType.Ease_Out:                        // �������� õõ�� ������
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;

                case GraphType.Ease_In:                         // �������� õõ�� ������
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;

                case GraphType.Smoothstep:                      // �ε巯�� ��ȯ
                    t = t * t * (3f - 2f * t);
                    break;

                case GraphType.Smootherstep:                    // ���� �ε巯�� ��ȯ
                    t = t * t * t * (t * (6f * t - 15f) + 10f);
                    break;
            }

            // �ݹ� ����
            callback?.Invoke(t);
            yield return null;
        }
    }

    // Sine �Լ� �ݹ� ( ���� �����ϴ� �ӵ��� �������� �׷��� (ease out) )
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

            // �ݹ� ����
            callback?.Invoke(t);
            yield return null;
        }
    }
}
