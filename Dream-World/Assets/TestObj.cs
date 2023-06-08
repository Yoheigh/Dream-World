using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : TriggerObject
{
    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.Log("¿À");
        _player.Hit();
    }
}
