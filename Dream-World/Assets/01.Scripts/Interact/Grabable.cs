using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Grabable : InteractionObject
{
    private Rigidbody rigid;

    public override ObjectType ObjectType { get { return ObjectType.Grabable; } }

    public override void InteractWithPlayer(PlayerController _player)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        rigid = gameObject.GetComponent<Rigidbody>();
        rigid.isKinematic = false;
        rigid.useGravity = false;
    }

    // �ٽ� �ڸ��� ������ �� ��� Ȱ��ȭ
    /* ������Ʈ���� ĸ��ȭ �ʿ� */
    public virtual void OnRelease()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        rigid.useGravity = true;

        rigid.AddForce((transform.forward + Vector3.up * 0.1f) * 5f, ForceMode.Impulse);
    }
}
