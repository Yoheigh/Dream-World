using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hpPoint;

    private void Start()
    {
        for (int i = 0; i < hpPoint.Length; i++)
        {
            hpPoint[i].gameObject.SetActive(true);
        }
    }

    public void Draw(int playerHP)
    {
        for (int i = 0; i < hpPoint.Length; i++)
        {
            if (i <= playerHP - 1)
                hpPoint[i].gameObject.SetActive(true);
            else
                hpPoint[i].gameObject.SetActive(false);
        }
    }
}
