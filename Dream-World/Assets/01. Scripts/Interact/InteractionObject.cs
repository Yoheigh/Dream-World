using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public abstract ObjectType ObjectType { get; }

    public abstract void InteractWithPlayer();
}
