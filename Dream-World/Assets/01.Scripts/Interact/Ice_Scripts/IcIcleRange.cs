using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcIcleRange : TriggerObject
{
    public GameObject Icicle;

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Icicle.SetActive(true);

        Debug.Log("���� �ߵ�");
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
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
