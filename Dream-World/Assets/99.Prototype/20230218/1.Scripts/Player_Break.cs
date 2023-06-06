using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Break : MonoBehaviour
{
    #region Singleton
    private static Player_Break Instance;
    public static Player_Break instance
    {
        get
        {
            if (Instance == null)
                return null;

            else return Instance;
        }
    }

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion
    public bool Destroy_tree; //���� ���� --> ���� ���� �ڿ� ���� ������ ���� �ο� ��
    bool Structure_Create;

    public int treeCount = 0; // �ı��� ����

    public GameObject Target_Structure;
    public GameObject Target_Structure2;
    public GameObject Target_Des;
    public GameObject Target_Axe;
    public GameObject Target_Axe_UI;

    void OnCollisionEnter(Collision col) // �浹�� ���۵� �� ȣ��
    {
        if (col.gameObject.CompareTag("Resources_Tree")) // "�����ڿ�" �±��� ������Ʈ�� �浹���� ���
        {
            Destroy(col.gameObject); // �ش� ������Ʈ �ı�
        }

        if (col.gameObject.CompareTag("Resources_Iron"))
        {
            Destroy(col.gameObject);
        }

    }
    public void GetAxe()
    {
        Target_Axe.SetActive(true);
        Target_Axe_UI.SetActive(true);
        Destroy_tree = true;
    }

    void OnCollisionStay(Collision col) // �浹�� ���ӵ� �� ȣ��
    {
        if (col.gameObject.CompareTag("Tree"))
        {
            if (Destroy_tree && Input.GetKey(KeyCode.J))
            {
                Destroy(col.gameObject);
                treeCount++;
                if (treeCount >= 5)
                {
                    Target_Structure.SetActive(true);
                    Target_Structure2.SetActive(false);
                }
            }
        }

        /*
        void OnCollisionStay(Collision col) // �浹�� ���ӵ� �� ȣ��
            {
                if (col.gameObject.CompareTag("Tree"))
                {
                    if (Destroy_tree && Input.GetKey(KeyCode.J))
                    {

                        treeCount++;

                        if (treeCount >= 5)
                        {
                            //Target_Structure.SetActive(true);
                            Debug.Log("�ٸ� ����");
                        }
                        Destroy(col.gameObject);
                    }
                }


            }*/
    }
}
