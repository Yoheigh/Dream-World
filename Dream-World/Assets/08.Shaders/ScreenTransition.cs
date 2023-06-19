using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public float CircleSize = 0f;
    public float TransitionSpeed = 2f;

    private readonly int _circleSizeId = Shader.PropertyToID("_Circle_Size");

    [SerializeField]
    private Image image;

    [SerializeField]
    private AnimationCurve[] curves = new AnimationCurve[2];

    private void Awake()
    {
        image = GetComponent<Image>();

        // 서클 크기 초기화
        image.materialForRendering.SetFloat(_circleSizeId, CircleSize);
    }

    public void CircleOut()
    {
        StopAllCoroutines();
        StartCoroutine(CircleOutCo());
    }

    public void CircleIn()
    {
        StopAllCoroutines();
        StartCoroutine(CircleInCo());
    }


    private IEnumerator CircleInCo()
    {
        CircleSize = 1f;

        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.unscaledDeltaTime;
            percent = current / TransitionSpeed;

            CircleSize = Mathf.Lerp(1f, 0f, curves[0].Evaluate(percent));
            image.materialForRendering.SetFloat(_circleSizeId, CircleSize);

            yield return null;
        }
    }

    private IEnumerator CircleOutCo()
    {
        CircleSize = 0f;

        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.unscaledDeltaTime;
            percent = current / TransitionSpeed;

            CircleSize = Mathf.Lerp(0f, 1f, curves[1].Evaluate(percent));
            image.materialForRendering.SetFloat(_circleSizeId, CircleSize);

            yield return null;
        }
    }
}
