using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : TriggerObject
{
    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.Log("¿∏ù–!!!!!");
        _player.Hit();
    }
}
