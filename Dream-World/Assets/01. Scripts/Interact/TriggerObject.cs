using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

// TriggerEnter 됐을 때 작동하는 함수
public abstract class TriggerObject : MonoBehaviour
{
    // 플레이어와 OnTriggerEnter 했을 경우 작동하는 함수
    public abstract void TriggerWithPlayer(PlayerController _player);

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            var player = col.GetComponent<PlayerController>();
            TriggerWithPlayer(player);
        }
    }
}