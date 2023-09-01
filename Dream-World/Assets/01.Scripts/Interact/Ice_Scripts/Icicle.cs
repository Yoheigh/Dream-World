using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : TriggerObject
{
    protected override void TriggerWithPlayer(PlayerController _player)
    {
        _player.Hit();
        Debug.Log("�÷��̾� Ÿ��");
        GameObject obj = Instantiate(Managers.Instance.Build.BuildVFX, transform.position, Quaternion.identity);
        Destroy(obj, 4f);
        Destroy(gameObject);
    }
    protected override void TriggerWith(Collider other)
    {
        GameObject obj = Instantiate(Managers.Instance.Build.BuildVFX, transform.position, Quaternion.identity);
        Destroy(obj, 4f);
        Destroy(gameObject);
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
