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

            // 플레이어 수량이 필요한 수량보다 같거나 이상일 경우, 초록색 텍스트
            if (playerCounts[i] >= needCounts[i])
            { 
                Texts[i].color = Color.green;
            }

            // 플레이어 수량이 필요한 수량보다 이하일 경우, 빨간색 텍스트
            else
            {
                if(!isCant)
                {
                    isCant = !isCant;
                }
                Texts[i].color = Color.red;
            }

            // 필요한 수량이 0일 경우, 흰색 텍스트
            if (needCounts[i] == 0)
            {
                Texts[i].color = Color.white;
            }

            UnMakeCheck_Button.SetActive(isCant);
        }
    }


}
