using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : InteractionObject
{
    public override void InteractWithPlayer()
    {
        // enum 등록 잠시 여기로
        objectType = ObjectType.Grabable;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    // 다시 자리에 놓았을 때 기능 활성화
    public void Init()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
