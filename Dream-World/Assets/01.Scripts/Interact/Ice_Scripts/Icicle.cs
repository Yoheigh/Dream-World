using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : TriggerObject
{
    public GameObject IcicleRE;

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        _player.Hit();
        Debug.Log("�÷��̾� Ÿ��");
        Destroy(IcicleRE, 0.2f);
    }
    protected override void TriggerWith(Collider other)
    {
        Destroy(IcicleRE, 0.2f);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("�÷��̾� Ÿ��");
    //        Destroy(IcicleRE, 3);
    //        if (other.gameObject.layer == LayerMask.NameToLayer("Block"))
    //        {
    //            Debug.Log("Ground(Block) Ÿ��");
    //            Destroy(IcicleRE, 3);
    //        }
    //    }
    //}
    //protected override void TriggerWithPlayer(PlayerController _player)
    //{
    //    _player.Hit();
    //    Destroy(gameObject);

    //    Debug.Log("�÷��̾�� �浹��");
    //}
}
