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
        Debug.Log("(÷��÷��) ����!!!!!!!!");
        _player.Hit();

        // gameObject.GetComponent<BoxCollider>().isTrigger = true;
        // ������������ �����ϴ� �ڵ�
    }
}
