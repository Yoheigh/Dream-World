using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntHell : TriggerObject
{
    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Destroy(gameObject, 3);

        _player.transform.Rotate(Vector3.up * Time.deltaTime * rotateSecond);
    }

    public float rotateSecond;

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSecond);
    }

    protected override void TriggerWith(Collider other)
    {
        return;
    }
}
