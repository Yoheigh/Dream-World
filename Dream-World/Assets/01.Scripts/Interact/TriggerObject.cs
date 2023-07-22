using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class TriggerObject : MonoBehaviour
{
    [Tooltip("한 번만 트리거되야하는 경우 체크")]
    public bool IsTriggerOnlyOnce = false;

    [Tooltip("주기적으로 다시 실행 가능해지는 딜레이")]
    public float DelayTime = 0f;

    private bool isTriggered = false;

    // 한 번 만난 플레이어 저장
    protected PlayerController player;

    // 플레이어와 OnTriggerEnter 했을 경우 작동하는 함수
    protected abstract void TriggerWithPlayer(PlayerController _player);

    protected abstract void TriggerWith(Collider other);

    // 작동 처리 함수
    public void PlayerEntered(PlayerController _player)
    {
        if (isTriggered == false)
        {
            if(IsTriggerOnlyOnce == true)
            {
                isTriggered = true;
                TriggerWithPlayer(_player);

                if (DelayTime != 0f)
                    StartDelay(DelayTime);
                return;
            }
            TriggerWithPlayer(_player);
        }
        else
            return;
    }

    private void StartDelay(float delayTime)
    {
        StartCoroutine(StartDelayCO(delayTime));
    }

    private IEnumerator StartDelayCO(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        isTriggered = false;
    }

    public void ObjectEntered(Collider other)
    {
        TriggerWith(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (player == null)
            //    player = GetComponent<PlayerController>();
            //else
                PlayerEntered(Manager.Instance.Player);
        }
        else
        {
            ObjectEntered(other);
        }

    }
}