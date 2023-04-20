using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waring_UI : MonoBehaviour
{
    // 시간을 표시하는 text UI를 Uinty에서 가져온당 (^_^)
    public Text WaringUI;

    // 3초 뒤에 비활성화 시킬 WaringUI를 설정한당 (^_^)
    public GameObject Target_WaringUI_OFF;

    // 제한 시간을 설정 해준당 (^_^)
    public float SetTime;
    private float setTime;


    private void Start()
    {
        setTime = SetTime;
    }

    private void Update()
    {
        if (Target_WaringUI_OFF.activeSelf)
        {
            // 남은 시간을 감소시켜준당 (^_^)
            setTime -= Time.deltaTime;

            // 텍스트를 표시한돠다다당 (^_^)
            WaringUI.text = "표시 해제 : " + Mathf.CeilToInt(setTime);

            if (setTime <= 0)
            {
                Target_WaringUI_OFF.SetActive(false);
                setTime = SetTime;
            }
        }
    }
    




      /*
    void Update()
    {
        // 초단위 변수를 만들어준돵 (^_^)
        float sec = setTime;

        // 남은 시간을 감소시켜준당 (^_^)
        setTime -= Time.deltaTime;

        // 텍스트를 표시한돠다다당 (^_^)
        WaringUI.text = "표시 해제 : " + Mathf.Ceil(sec);

        if (sec <= 0)
        {
            Target_WaringUI_OFF.SetActive(false);
                
        }
    }*/
}
