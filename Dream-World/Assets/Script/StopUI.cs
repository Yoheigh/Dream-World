using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopUI : MonoBehaviour
{
    //bool isOpen;
    public GameObject Target_MenuUI;
    public GameObject Target_StopUI;

    bool IsPause;
    void Start()
    {
        IsPause = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Target_MenuUI.activeSelf)
            {
                if (IsPause == false)
                {
                    Time.timeScale = 0;
                    Target_StopUI.SetActive(true);
                    IsPause = true;
                    return;
                }
            }
            if (Target_StopUI.activeSelf)
            {
                if (IsPause == true)
                {
                    Time.timeScale = 1;
                    Target_StopUI.SetActive(false);
                    IsPause = false;
                    return;
                }
            }
        }
        //void Update()
        //{
        //    EnableMenuUI();
        //    //Time.timeScale = 1;
        //}
        //void EnableMenuUI()
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        if (!Target_MenuUI.activeSelf)
        //        {
        //            // 일시정지 활성화
        //            isOpen = !isOpen;
        //            Target_StopUI.SetActive(isOpen);
        //            Time.timeScale = 0;
        //        }
        //            //일시정지 비활성화
        //        if (Target_MenuUI.activeSelf)
        //        {
        //            Target_StopUI.SetActive(false);
        //            Time.timeScale = 1;
        //        }
        //    }

        //}
    }
}
