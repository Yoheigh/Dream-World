using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : InteractionObject
{
    public override void InteractWithPlayer()
    {
        // enum ��� ��� �����
        objectType = ObjectType.Grabable;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void Grabbed(bool _flag)
    {
        Debug.Log(" �� �Ӹ� ���� �� �ִ�");
        gameObject.GetComponent<Collider>().enabled = _flag;

    }
}
