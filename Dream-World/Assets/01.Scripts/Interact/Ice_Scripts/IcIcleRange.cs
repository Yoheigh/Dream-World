using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcIcleRange : TriggerObject
{
    public GameObject Icicle;

    protected override void TriggerWith(Collider other)
    {
        return;
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        Instantiate(Icicle, transform.position + Vector3.up * 10f, Quaternion.identity);

        Debug.Log("함정 발동");
        Destroy(gameObject);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //      Icicle.gameObject.SetActive(true);

    //      Debug.Log("함정 발동");
    //    }
    //}
}
