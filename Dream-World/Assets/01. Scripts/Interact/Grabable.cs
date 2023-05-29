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
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    // �ٽ� �ڸ��� ������ �� ��� Ȱ��ȭ
    public void Init()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
