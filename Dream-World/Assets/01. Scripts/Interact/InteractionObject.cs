using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Pickup,
    Grabable,
    StageObject
}

public abstract class InteractionObject : MonoBehaviour
{
    // ���ͷ��� ������Ʈ�� �ʿ��� �� ���� ������
    public ObjectType objectType;

    public abstract void InteractWithPlayer();
}
