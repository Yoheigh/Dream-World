using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : TriggerObject
{
    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        _player.Hit();
    } 
}
