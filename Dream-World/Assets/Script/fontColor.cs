using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fontColor : MonoBehaviour
{
    public Text[] Texts;
    public int[] playerCounts;
    public int[] needCounts;
    public string name_;
    public GameObject UnMakeCheck_Button;
    public bool isCant = false;


    void Update()
    {
        for (int i = 0; i < playerCounts.Length; i++)
        {
            Texts[i].text = playerCounts[i] + " / " + needCounts[i];

            // �÷��̾� ������ �ʿ��� �������� ���ų� �̻��� ���, �ʷϻ� �ؽ�Ʈ
            if (playerCounts[i] >= needCounts[i])
            { 
                Texts[i].color = Color.green;
            }

            // �÷��̾� ������ �ʿ��� �������� ������ ���, ������ �ؽ�Ʈ
            else
            {
                if(!isCant)
                {
                    isCant = !isCant;
                }
                Texts[i].color = Color.red;
            }

            // �ʿ��� ������ 0�� ���, ��� �ؽ�Ʈ
            if (needCounts[i] == 0)
            {
                Texts[i].color = Color.white;
            }

            UnMakeCheck_Button.SetActive(isCant);
        }
    }


}
