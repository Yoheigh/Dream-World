using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcIcleRange : TriggerObject
{
    public GameObject Icicle;

    protected override void TriggerWith(Collider other)
    {
        Icicle.SetActive(true);

        Debug.Log("���� �ߵ�");
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Icicle.SetActive(true);

        Debug.Log("���� �ߵ�");
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //      Icicle.gameObject.SetActive(true);

    //      Debug.Log("���� �ߵ�");
    //    }
    //}
}
