using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waring_UI : MonoBehaviour
{
    // �ð��� ǥ���ϴ� text UI�� Uinty���� �����´� (^_^)
    public Text WaringUI;

    // 3�� �ڿ� ��Ȱ��ȭ ��ų WaringUI�� �����Ѵ� (^_^)
    public GameObject Target_WaringUI_OFF;

    // ���� �ð��� ���� ���ش� (^_^)
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
            // ���� �ð��� ���ҽ����ش� (^_^)
            setTime -= Time.deltaTime;

            // �ؽ�Ʈ�� ǥ���ѵ´ٴٴ� (^_^)
            WaringUI.text = "ǥ�� ���� : " + Mathf.CeilToInt(setTime);

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
        // �ʴ��� ������ ������؉� (^_^)
        float sec = setTime;

        // ���� �ð��� ���ҽ����ش� (^_^)
        setTime -= Time.deltaTime;

        // �ؽ�Ʈ�� ǥ���ѵ´ٴٴ� (^_^)
        WaringUI.text = "ǥ�� ���� : " + Mathf.Ceil(sec);

        if (sec <= 0)
        {
            Target_WaringUI_OFF.SetActive(false);
                
        }
    }*/
}
