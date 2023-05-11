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
                Debug.Log("�ֻ��� ���� �ƹ��͵� ����");               
                return;
            }

            if (Layers_2Depth[0].activeSelf)
            {
                // 3������ ���������� �ش� 3������ ���� ������
                for (int i = 0; i < 3; ++i)
                {
                    if (Layers_3Depth[i].activeSelf)
                    {
                        Layers_3Depth[i].SetActive(false);
                        EquipExplains[i].SetActive(true);
                        ItemButtons[i].SetActive(true);

                        // �ٸ� �ִϸ��̼� ���� ��ư ��ǥ�� ��ġ ��Ű�� �ڵ� �߰� �ʿ�

                        return;
                    }
                }

                // 2���� On �����϶� 1Depth -> 2Depth�� �ؽ�Ʈ �ִϸ��̼� Ʋ���� ó��
                // (Layer 2 ������ �ִ� ��Ʈ�� Equipment�� ��� ���� 3���� �� 2�� �� 6�� 0~5��) ��Ʈ�� ��ü�� �ִϸ��̼� ��ġ �ʱ�ȭ �޼��� ȣ��)
                for (int i = 0; i < 6; ++i)
                {
                    MoveTextScripts[i].InitPos();
                }

                // Text Animation ������ ��ġ�� �� �� ���� Ȱ��ȭ���� 2���� Layer ��Ȱ��ȭ ó�� 
                Layers_2Depth[0].SetActive(false);
                // 1Depth Layer 1 ������ ����(Ȱ��ȭ��)
                Target_1depth.SetActive(true);
            }


            if (Layers_2Depth[1].activeSelf)
            {
                // 3������ ���������� �ش� 3������ ���� ������
                for (int i = 3; i < 5; ++i)
                {
                    if (Layers_3Depth[i].activeSelf)
                    {
                        Layers_3Depth[i].SetActive(false);
                        StructureExplains[i%3].SetActive(true);
                        ItemButtons[i].SetActive(true);


                        // �ٸ� �ִϸ��̼� ���� ��ư ��ǥ�� ��ġ ��Ű�� �ڵ� �߰� �ʿ�

                        return;
                    }
                }

                // 2���� On �����϶� 1Depth -> 2Depth�� �ؽ�Ʈ �ִϸ��̼� Ʋ���� ó��
                // (Layer 2 ������ �ִ� ��Ʈ�� Equipment�� ��� ���� 3���� �� 2�� �� 4�� 6~9��) ��Ʈ�� ��ü�� �ִϸ��̼� ��ġ �ʱ�ȭ �޼��� ȣ��)
                for (int j = 6; j < 10; ++j)
                {
                    MoveTextScripts[j].InitPos();
                }


                // Text Animation ������ ��ġ�� �� �� ���� Ȱ��ȭ���� 2���� Layer ��Ȱ��ȭ ó�� 
                Layers_2Depth[1].SetActive(false);
                // 1Depth Layer 1 ������ ����(Ȱ��ȭ��)
                Target_1depth.SetActive(true);
            }


        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Target_MenuUI.SetActive(!Target_MenuUI.activeSelf);
        }

    }
}

