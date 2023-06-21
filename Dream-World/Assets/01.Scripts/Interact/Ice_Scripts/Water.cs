using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : TriggerObject
{
    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.Log("(첨벙첨벙) 으앆!!!!!!!!");
        _player.Hit();

        // gameObject.GetComponent<BoxCollider>().isTrigger = true;
        // 시작지점으로 스폰하는 코드
    }
}
