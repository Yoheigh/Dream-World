using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flag : TriggerObject
{
    public GameObject flagUI;

    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Manager.Instance.Flag.NextSceneWithTransition();

        Time.timeScale = 0;
    }


    //void On(Collision col)
    //{
    //    if (col.gameObject.CompareTag("Player"))
    //    {
    //        flagUI.SetActive(true);
    //    }
    //}
}
