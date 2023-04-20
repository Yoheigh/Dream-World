using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuUI : MonoBehaviour
{
    bool isOpen;
    public GameObject Target_MenuUI;
    public GameObject Target_OFF1;
    public GameObject Target_OFF2;
    public GameObject Target_ON1;

    void Update()
    {
        EnableMenuUI();
    }

    void EnableMenuUI()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isOpen = !isOpen;
            Target_MenuUI.SetActive(isOpen);
            /*if (Target_OFF1.activeSelf)
            {
                Target_MenuUI.SetActive(true);
            }*/
            /*if(Target_MenuUI.activeSelf)
            {
                Target_MenuUI.SetActive(false);
            }
            if (!Target_MenuUI.activeSelf)
            {
                Target_MenuUI.SetActive(true);
            }*/

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Target_OFF1.activeSelf)
            {
                Target_OFF1.SetActive(false);
            }
            if (Target_OFF2.activeSelf)
            {
                Target_OFF2.SetActive(false);
            }    
            if (!Target_ON1.activeSelf)
            {
                Target_ON1.SetActive(true);
            }
        }
    }
}

