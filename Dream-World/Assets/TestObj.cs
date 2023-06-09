using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : TriggerObject
{
    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.Log("오");
        _player.Hit();
    }

    // 우오옷 된다
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("트리거");
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("콜라이더");
    //}
}
