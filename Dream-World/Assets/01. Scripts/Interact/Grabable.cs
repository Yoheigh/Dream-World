using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : InteractionObject
{
    public override ObjectType ObjectType { get { return ObjectType.Grabable; } }

    public override void InteractWithPlayer(PlayerController _player)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    // 다시 자리에 놓았을 때 기능 활성화
    /* 오브젝트별로 캡슐화 필요 */
    public void Init()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
