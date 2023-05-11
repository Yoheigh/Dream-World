using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MenuUI : MonoBehaviour
{
    bool isOpen;
    public GameObject Target_MenuUI;

    public GameObject Target_1depth;

    public List<GameObject> Layers_2Depth;

    public List<GameObject> EquipMenuButtons;
    public List<GameObject> EquipExplains;

    public List<GameObject> StructureMenuButtons;
    public List<GameObject> StructureExplains;
    public List<GameObject> ItemButtons;
    public List<GameObject> Layers_3Depth;

    public CMoveText[] MoveTextScripts;


    void Update()
    {
        EnableMenuUI();
    }

    void EnableMenuUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isNotLowDepth = false;
            foreach (var obj in Layers_2Depth)
            {
                isNotLowDepth = isNotLowDepth || obj.activeSelf;             
            }

            if (Target_1depth.activeSelf)
            {
                Debug.Log("최상위 레벨 아무것도 안함");               
                return;
            }

            if (Layers_2Depth[0].activeSelf)
            {
                // 3뎁스가 켜져있으면 해당 3뎁스를 쓰고 마무리
                for (int i = 0; i < 3; ++i)
                {
                    if (Layers_3Depth[i].activeSelf)
                    {
                        Layers_3Depth[i].SetActive(false);
                        EquipExplains[i].SetActive(true);
                        ItemButtons[i].SetActive(true);

                        // 다른 애니메이션 중인 버튼 좌표와 일치 시키는 코드 추가 필요

                        return;
                    }
                }

                // 2뎁스 On 상태일때 1Depth -> 2Depth시 텍스트 애니메이션 틀어짐 처리
                // (Layer 2 레벨에 있는 스트링 Equipment의 경우 현재 3개에 각 2개 총 6개 0~5번) 스트링 객체에 애니메이션 위치 초기화 메서드 호출)
                for (int i = 0; i < 6; ++i)
                {
                    MoveTextScripts[i].InitPos();
                }

                // Text Animation 정렬을 마치고 난 후 현재 활성화중인 2레벨 Layer 비활성화 처리 
                Layers_2Depth[0].SetActive(false);
                // 1Depth Layer 1 꺼짐을 켜짐(활성화로)
                Target_1depth.SetActive(true);
            }


            if (Layers_2Depth[1].activeSelf)
            {
                // 3뎁스가 켜져있으면 해당 3뎁스를 쓰고 마무리
                for (int i = 3; i < 5; ++i)
                {
                    if (Layers_3Depth[i].activeSelf)
                    {
                        Layers_3Depth[i].SetActive(false);
                        StructureExplains[i%3].SetActive(true);
                        ItemButtons[i].SetActive(true);


                        // 다른 애니메이션 중인 버튼 좌표와 일치 시키는 코드 추가 필요

                        return;
                    }
                }

                // 2뎁스 On 상태일때 1Depth -> 2Depth시 텍스트 애니메이션 틀어짐 처리
                // (Layer 2 레벨에 있는 스트링 Equipment의 경우 현재 3개에 각 2개 총 4개 6~9번) 스트링 객체에 애니메이션 위치 초기화 메서드 호출)
                for (int j = 6; j < 10; ++j)
                {
                    MoveTextScripts[j].InitPos();
                }


                // Text Animation 정렬을 마치고 난 후 현재 활성화중인 2레벨 Layer 비활성화 처리 
                Layers_2Depth[1].SetActive(false);
                // 1Depth Layer 1 꺼짐을 켜짐(활성화로)
                Target_1depth.SetActive(true);
            }


        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Target_MenuUI.SetActive(!Target_MenuUI.activeSelf);
        }

    }
}

