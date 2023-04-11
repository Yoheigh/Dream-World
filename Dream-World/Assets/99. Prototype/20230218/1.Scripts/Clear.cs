using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    /*   void OnCollisionEnter(Collision col) // 충돌이 시작될 때 호출
       {
           if (col.gameObject.CompareTag("Player")) // Collider가 충돌된 객체의 태그가 "Player"일 경우
           {
               SceneManager.LoadScene("Clear"); // 씬 체인지 "Clear"
           }
       }*/
    public GameObject Clear_UI;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Clear_UI.SetActive(true);
        }
    }
}
