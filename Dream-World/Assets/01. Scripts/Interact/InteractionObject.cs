using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Pickup,         // 1ȸ�� �ݴ� ������Ʈ
    Grabable,       // ��� �ٴ� �� �ִ� ������Ʈ 
    Dragable,       // ����ٴ� �� �ִ� ������Ʈ
    StageObject,    // ��ȣ�ۿ����� �۵��ϴ� ������Ʈ ( ���� : ��ư, ���� )
}

public abstract class InteractionObject : MonoBehaviour
{
    // ���ͷ��� ������Ʈ�� �ʿ��� �� ���� ������
    public ObjectType objectType;

    public abstract void InteractWithPlayer();
}
