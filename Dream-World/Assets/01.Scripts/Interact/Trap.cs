using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TriggerObject
{
    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        // 아야
        _player.Hit();
    }
}
