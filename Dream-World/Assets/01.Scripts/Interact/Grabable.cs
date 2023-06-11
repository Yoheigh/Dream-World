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

    // �ٽ� �ڸ��� ������ �� ��� Ȱ��ȭ
    /* ������Ʈ���� ĸ��ȭ �ʿ� */
    public void Init()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
