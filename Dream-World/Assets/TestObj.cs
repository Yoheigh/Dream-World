using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : TriggerObject
{
    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.Log("��");
        _player.Hit();
    }

    // ����� �ȴ�
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Ʈ����");
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("�ݶ��̴�");
    //}
}
