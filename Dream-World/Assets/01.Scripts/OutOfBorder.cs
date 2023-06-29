using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBorder : TriggerObject
{
    public Transform respawnPoint;

    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Manager.Instance.Flag.OutOfBorder(respawnPoint);
    }
}
