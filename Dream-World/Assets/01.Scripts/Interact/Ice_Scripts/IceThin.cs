using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceThin : TriggerObject
{
    public float BreakTime = 3f;
    private float breakTime;

    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Debug.Log("플레이어와 충돌함");
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        while(breakTime <= BreakTime)
        {
            breakTime += Time.deltaTime;
            yield return null;
        }
        Manager.Instance.Sound.PlaySFX(123);
        Destroy(gameObject);
    }
}
