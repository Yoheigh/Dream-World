using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    /*   void OnCollisionEnter(Collision col) // �浹�� ���۵� �� ȣ��
       {
           if (col.gameObject.CompareTag("Player")) // Collider�� �浹�� ��ü�� �±װ� "Player"�� ���
           {
               SceneManager.LoadScene("Clear"); // �� ü���� "Clear"
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
