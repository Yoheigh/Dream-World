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
    }

    public void Grabbed(bool _flag)
    {
        Debug.Log(" 니 머리 위에 나 있다");
        gameObject.GetComponent<Collider>().enabled = _flag;

    }
}
