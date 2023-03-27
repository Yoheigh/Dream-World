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
    public bool Destroy_tree; //나무 삭제 --> 이후 나무 자원 생산 조건을 위한 부울 값
    bool Structure_Create;

    public int treeCount = 0; // 파괴한 나무

    public GameObject Target_Structure;
    public GameObject Target_Structure2;
    public GameObject Target_Des;
    public GameObject Target_Axe;
    public GameObject Target_Axe_UI;

    void OnCollisionEnter(Collision col) // 충돌이 시작될 때 호출
    {
        if (col.gameObject.CompareTag("Resources_Tree")) // "나무자원" 태그의 오브젝트와 충돌했을 경우
        {
            Destroy(col.gameObject); // 해당 오브젝트 파괴
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

    void OnCollisionStay(Collision col) // 충돌이 지속될 때 호출
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
        void OnCollisionStay(Collision col) // 충돌이 지속될 때 호출
            {
                if (col.gameObject.CompareTag("Tree"))
                {
                    if (Destroy_tree && Input.GetKey(KeyCode.J))
                    {

                        treeCount++;

                        if (treeCount >= 5)
                        {
                            //Target_Structure.SetActive(true);
                            Debug.Log("다리 생성");
                        }
                        Destroy(col.gameObject);
                    }
                }


            }*/
    }
}
