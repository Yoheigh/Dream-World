using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

// TriggerEnter 됐을 때 작동하는 함수
public abstract class TriggerObject : MonoBehaviour
{
    protected bool isTriggered = false;

    // 플레이어와 OnTriggerEnter 했을 경우 작동하는 함수
    protected abstract void TriggerWithPlayer(PlayerController _player);

    // 작동 처리 함수
    public void PlayerEntered(PlayerController _player)
    {
        if (isTriggered == false)
        {
            isTriggered = true;
            TriggerWithPlayer(_player);
        }
        else
            return;
    }
}