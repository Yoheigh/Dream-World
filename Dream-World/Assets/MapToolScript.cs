using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapToolScript : MonoBehaviour
{
    public Button obj_Btn_1;
    public Button obj_Btn_2;
    public Button obj_Btn_3;
    public Button obj_Btn_4;
    public Button Up_Btn;
    public Button Down_Btn;

    public TMPro.TextMeshProUGUI yPosText;

    int currentYPos = 0;
    GridObject selectGridObject;

    private void Awake()
    {
        obj_Btn_1.onClick.AddListener(() =>
        {
            selectGridObject = new RedCube();
        });

        obj_Btn_2.onClick.AddListener(() =>
        {
            selectGridObject = new BlueCube();
        });

        Up_Btn.onClick.AddListener(() =>
        {
            currentYPos++;
            yPosText.text = currentYPos.ToString();
        });

        Down_Btn.onClick.AddListener(() =>
        {
            currentYPos--;
            yPosText.text = currentYPos.ToString();
        });
    }

    private void Update()
    {
        if (selectGridObject == null)
            return;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
            
        if(Input.GetMouseButton(0))
        {
            if(GridSystem.Instance.CheckCanCraft(Mathf.RoundToInt(point.x), currentYPos, Mathf.RoundToInt(point.z)))
            {
                Debug.Log(Mathf.RoundToInt(point.x)+" + "+currentYPos+" + "+ Mathf.RoundToInt(point.z));
                GridSystem.Instance.StageGrid.SetGridObject(Mathf.RoundToInt(point.x), currentYPos, Mathf.RoundToInt(point.z), selectGridObject);
            }

            else
            {
                Debug.Log("설치할 수 없습니다.");
            }
        }
    }
}
