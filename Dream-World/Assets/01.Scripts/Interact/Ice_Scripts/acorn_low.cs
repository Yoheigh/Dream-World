using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acorn_low : TriggerObject
{
    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.LogError("∞‘¿”∆˜¿Œ∆Æ »πµÊ!");
        Managers.Sound.PlaySFX(4);
        var obj = Instantiate(Managers.Build.BuildVFX);
        obj.transform.position = transform.position;
        Destroy(obj, 4f);
        Destroy(gameObject);
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
