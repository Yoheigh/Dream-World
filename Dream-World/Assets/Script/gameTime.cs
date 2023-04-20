using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameTime : MonoBehaviour
{
    // �ð��� ǥ���ϴ� text UI�� ����Ƽ���� �����´�.
    public Text gameTimeUI;

    // ��ü ���� �ð��� �������ش�.
    float setTime = 70;

    // �д����� �ʴ����� ����� ������ ������ش�.
    int min;
    float sec;

    void Update()
    {
        // ���� �ð��� ���ҽ����ش�.
        setTime -= Time.deltaTime;

        // ��ü �ð��� 60�� ���� Ŭ ��
        if (setTime >= 60f)
        {
            // 60���� ������ ����� ���� �д����� ����
            min = (int)setTime / 60;
            // 60���� ������ ����� �������� �ʴ����� ����
            sec = setTime % 60;
            // UI�� ǥ�����ش�
            gameTimeUI.text = "0" + min + ":" + (int)sec;
        }

        // sec�� 10�� �̸��� ��
        if (sec < 10)
        {
            gameTimeUI.text = "0" + min + ":" + "0" + (int)sec;
        }

        // ��ü�ð��� 60�� �̸��� ��
        if (setTime < 60f)
        {
            // �� ������ �ʿ�������Ƿ� �ʴ����� ������ ����
            gameTimeUI.text = "00:" + (int)setTime;
            if (setTime < 10)
            {
                gameTimeUI.text = "00:0" + (int)setTime;
            }
        }

        // ���� �ð��� 0���� �۾��� ��
        if (setTime <= 0)
        {
            // UI �ؽ�Ʈ�� 0�ʷ� ������Ŵ.
            gameTimeUI.text = "00:00";
        }
    }
}
